using System.Collections;
using UnityEngine;

public class GhoulController : UnitController
{
    [SerializeField] private GameObject criticalHitbox;
    [SerializeField] private Bobbing bobbing;
    [SerializeField] private Transform rootParent;

    [Header("laser")]
    [SerializeField] private Transform laserOrigin;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem laserSparks;

    private Material dissolveMaterial;
    private Rigidbody rb;
    private MeshCollider mesh;

    private float dissolveTime = 1.05f;
    private float cutoffHeightMax = 3.0f;
    private float cutoffHeightMin = -2.0f;

    private float maxXRotationForLookAt = 40f;
    
    private float movespeed;
    private bool attackUp = true;

    private float laserLength = 45f;
    private float laserWidth = 0.35f;
    private float delayToTrackingPlayerMovements = 0.75f;

    private bool _attacking;
    private bool Attacking
    {
        get => _attacking;
        set
        {
            if (_attacking != value)
            {
                _attacking = value;
                lineRenderer.enabled = value;
                lineRenderer.startWidth = laserWidth; // at some point animate to grow and shrink but whatever
                lineRenderer.endWidth = laserWidth;
                //SetLaser(value, new RaycastHit()); // just update the particle system accordingly
            }
        }
    }

    private bool _laserSparksActive;

    void OnEnable()
    {
        if (enemyUnit != null)
        {
            enemyUnit.OnUnitReady += Setup;
            enemyUnit.OnUnitDeath += OnDeath;
        }
    }

    void OnDisable()
    {
        if (enemyUnit != null)
        {
            enemyUnit.OnUnitReady -= Setup;
            enemyUnit.OnUnitDeath -= OnDeath;
        }
    }


    protected override void Start()
    {
        base.Start();

        dissolveMaterial = GetComponent<MeshRenderer>().material;
        dissolveMaterial.SetFloat("_CutoffHeight", cutoffHeightMax);

        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshCollider>();
        Physics.IgnoreCollision(mesh, Game.i.PlayerBodyCollider, true);
        Physics.IgnoreCollision(mesh, Game.i.PlayerHeadCollider, true);

        bobbing.StartBobbing();

        laserSparks.Stop();
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    private void Setup()
    {
        movespeed = enemyUnit.BaseMoveSpeed;
    }
    private void OnDeath()
    {
        bobbing.Stop();

        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        rb.AddForce(direction * 4.0f, ForceMode.Impulse);
        rb.AddForce(Vector3.up * 2.2f, ForceMode.Impulse);
        rb.angularVelocity = Game.i.PlayerCamera.transform.forward * 25f;
        StartCoroutine(Dissolve());

    }

    private void LateUpdate() // lateupdate is called after all update methods have run--this ensures the player has moved before the laser is updated for that frame
    {

        Vector3 direction = Game.i.PlayerTransform.position - transform.position;
        if (direction.sqrMagnitude < 0.0001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / delayToTrackingPlayerMovements);

        // clamp how far on x axis it can rotate
        Vector3 euler = transform.eulerAngles;
        euler.x = ClampAngle(euler.x, -maxXRotationForLookAt, maxXRotationForLookAt);
        transform.rotation = Quaternion.Euler(euler);


        if (InRangeOfPlayer(laserOrigin.position, enemyUnit.AttackRange)) // updatelaser uses the forward vector of the skull so it is also necessary to have this in lateupdate
        {
            Attacking = true;
            UpdateLaser();
        }
        else
        {
            Attacking = false;
        }
    }

    void UpdateLaser()
    {
        Vector3 startPos = laserOrigin.position;
        Vector3 endPos = startPos + (transform.forward * laserLength);


        RaycastHit hit;
        if (Physics.Raycast(startPos, transform.forward, out hit, laserLength))
        {
            SetLaser(true, hit);

            if (hit.collider.CompareTag("Player") && attackUp)
            {
                attackUp = false;
                StartCoroutine(timer.TimerCR(enemyUnit.AttackCDLength, () => attackUp = true));
                Game.i.PlayerUnitInstance.TakeDamage(false, enemyUnit.BaseDamage);
            }

            Vector3 lineRendererVector = endPos - startPos;
            Vector3 raycastVector = hit.point - startPos;

            if (raycastVector.sqrMagnitude < lineRendererVector.sqrMagnitude) // if the laser should stop short because of a collision, shorten it appropriately
            {
                endPos = hit.point;
            }

        }
        else
        {
            SetLaser(false, hit);
        }


        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    void SetLaser(bool isActive, RaycastHit hit)
    {
        if (_laserSparksActive != isActive)
        {
            _laserSparksActive = isActive;

            if (isActive && hit.collider != null)
            {
                laserSparks.Play();
            }
            else laserSparks.Stop();

        }

        if (isActive && hit.collider != null)
        {
            laserSparks.transform.position = hit.point;
            laserSparks.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }


    float ClampAngle(float angle, float min, float max)
    {
        angle = (angle > 180) ? angle - 360 : angle; // convert to [-180, 180]
        return Mathf.Clamp(angle, min, max);
    }

    private IEnumerator Dissolve()
    {
        float elapsed = 0f;
        while (elapsed < dissolveTime)
        { 
            elapsed += Time.deltaTime;
            dissolveMaterial.SetFloat("_CutoffHeight", Mathf.Lerp(cutoffHeightMax, cutoffHeightMin, elapsed / dissolveTime));
            if ((elapsed / dissolveTime) >= 0.5f) criticalHitbox.SetActive(false);
            yield return null;
        }
        Destroy(rootParent.gameObject);
    }

}
