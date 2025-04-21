using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class SpinnerController : UnitController
{
    //[SerializeField] private EnemyUnit enemyUnit;
    //[SerializeField] private GameObject criticalHitbox;

    private Material dissolveMaterial;
    [Header("Physics Components")]
    [SerializeField] private Rigidbody rb1;
    [SerializeField] private Rigidbody rb2;
    [SerializeField] private Rigidbody rb3;

    [SerializeField] private MeshCollider meshInner;
    [SerializeField] private MeshCollider meshOuter;
    [SerializeField] private SphereCollider eye;

    private List<Rigidbody> bodies;
    private List<MeshCollider> meshes;
    private List<MeshRenderer> renderers;

    private float dissolveTime = 1.5f;
    private float cutoffHeightMax = 3.0f;
    private float cutoffHeightMin = -2.0f;

    private float rotSpeed = 50f;
    private Vector3 innerRingRot;
    private Vector3 outerRingRot;

    [Header("Objects")]
    [SerializeField] private Transform innerRing;
    [SerializeField] private Transform outerRing;

    [SerializeField] private MeshRenderer innerRenderer;
    [SerializeField] private MeshRenderer outerRenderer;
    [SerializeField] private MeshRenderer eyeRenderer;

    [Header("Attack")]
    [SerializeField] private GameObject attackPrefab;

    private float movespeed;
    private bool attackUp = true;
    private void Awake()
    {
        bodies = new List<Rigidbody> { rb1, rb2, rb3 };
        meshes = new List<MeshCollider> { meshInner, meshOuter };
        renderers = new List<MeshRenderer> { innerRenderer, outerRenderer, eyeRenderer };

        innerRingRot = new Vector3(rotSpeed, rotSpeed, rotSpeed * 1f);
        outerRingRot = new Vector3(-rotSpeed, -rotSpeed, -rotSpeed * 1f);
    }

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

        //dissolveMaterial = GetComponent<MeshRenderer>().material;
        //dissolveMaterial.SetFloat("_CutoffHeight", cutoffHeightMax);

        foreach (var mesh in meshes)
        {
            Physics.IgnoreCollision(mesh, Game.i.PlayerBodyCollider, true);
            Physics.IgnoreCollision(mesh, Game.i.PlayerHeadCollider, true);
        }
        Physics.IgnoreCollision(eye, Game.i.PlayerBodyCollider, true);
        Physics.IgnoreCollision(eye, Game.i.PlayerHeadCollider, true);

        Bobbing[] bobbingScripts = GetComponentsInChildren<Bobbing>();
        foreach (var script in bobbingScripts)
        {
            script.StartBobbing();
        }
    }

    private void Update()
    {
        SpinRings();

        if (attackUp)
        {
            if (Vector3.Distance(transform.position, Game.i.PlayerTransform.position) < enemyUnit.AttackRange)
            {
                attackUp = false;
                StartCoroutine(timer.TimerCR(enemyUnit.AttackCDLength, () => attackUp = true));
                Attack();
            }
        }
    }
    void Attack()
    {
        Instantiate(attackPrefab, Game.i.PlayerTransform.position, Quaternion.identity);
    }
    void SpinRings()
    {
        innerRingRot = new Vector3(rotSpeed, rotSpeed, rotSpeed);
        outerRingRot = new Vector3(-rotSpeed, -rotSpeed, -rotSpeed);
        outerRing.Rotate(outerRingRot * Time.deltaTime);
        innerRing.Rotate(innerRingRot * Time.deltaTime);
    }
    private void OnDeath()
    {
        foreach (var mesh in meshes)
        {
            mesh.convex = true;
        }

        foreach (var rb in bodies)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = false;
            rb.useGravity = true;        
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(direction * 5.5f, ForceMode.Impulse);
        }


        foreach (var renderer in renderers)
        {
            StartCoroutine(Dissolve(renderer));
        }
    }

    private void Setup()
    {
        movespeed = enemyUnit.BaseMoveSpeed;
    }


    private IEnumerator Dissolve(MeshRenderer renderer)
    {
        float elapsed = 0f;
        while (elapsed < dissolveTime)
        {
            elapsed += Time.deltaTime;
            renderer.material.SetFloat("_CutoffHeight", Mathf.Lerp(cutoffHeightMax, cutoffHeightMin, elapsed / dissolveTime));
            if ((elapsed / dissolveTime) >= 0.5f)
            {
                EnemyCritical[] criticals = GetComponentsInChildren<EnemyCritical>();
                foreach (EnemyCritical critical in criticals)
                {
                    critical.gameObject.SetActive(false);
                }

            }
            yield return null;
        }
        Destroy(gameObject);
    }
}
