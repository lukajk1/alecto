using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DiverController : UnitController
{
    //[SerializeField] private EnemyUnit enemyUnit;
    [SerializeField] private TrailRenderer trailRenderer;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private PlayerPhysicsBus playerPhysicsBus;

    private float dashDuration = 4.2f;
    private float diveAttackDamageRadius = 3.2f;
    void OnEnable()
    {
        if (enemyUnit != null)
        {
            enemyUnit.OnUnitReady += Setup;
        }
    }

    void OnDisable()
    {
        if (enemyUnit != null)
        {
            enemyUnit.OnUnitReady -= Setup;
        }
    }

    private void Setup()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyUnit.CurrentMoveSpeed;
        agent.destination = Game.i.PlayerTransform.position;
        enemyUnit.AttackReady = true;
        playerPhysicsBus = FindFirstObjectByType<PlayerPhysicsBus>();
        trailRenderer.enabled = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Game.i.PlayerTransform.position) > enemyUnit.AttackRange && !isAttacking)
        {
            agent.isStopped = false;
            agent.destination = Game.i.PlayerTransform.position;
        }
        else
        {
            if (enemyUnit.AttackReady && playerPhysicsBus.IsGrounded) TryAttack();
        }

        //Debug.Log("attackready" + enemyUnit.AttackReady);
        //Debug.Log("isgrounded" +playerPhysicsBus.IsGrounded);
    }

    private void TryAttack()
    {
        Ray ray = new Ray(transform.position, Game.i.PlayerTransform.position - transform.position); // only attack if LOS on player
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Jump(10f, 1.4f));
            enemyUnit.AttackReady = false;
            StartCoroutine(timer.TimerCR(enemyUnit.AttackCDLength, () => enemyUnit.AttackReady = true));
        }
    }

    private IEnumerator Jump(float targetHeight, float duration)
    {
        trailRenderer.enabled = true; 
        isAttacking = true;
        agent.enabled = false;

        float startHeight = transform.position.y;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newY = Mathf.Lerp(startHeight, startHeight + targetHeight, elapsedTime / duration);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }

        transform.position = new Vector3(transform.position.x, startHeight + targetHeight, transform.position.z);
        bool hasDamaged = false;

        Vector3 dashTargetPos = Game.i.PlayerTransform.position;
        elapsedTime = 0f;
        while (Vector3.Distance(transform.position, dashTargetPos) >= 0.6f)
        {
            while (Game.IsPaused) yield return null;

            elapsedTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, dashTargetPos, elapsedTime / dashDuration);

            if (!hasDamaged)
            {
                if (Vector3.Distance(transform.position, Game.i.PlayerTransform.position) <= diveAttackDamageRadius)
                {
                    Game.i.PlayerUnitInstance.TakeDamage(false, enemyUnit.BaseDamage);
                    hasDamaged = true;

                    Vector3 xz = transform.position - Game.i.PlayerTransform.position;
                    xz = xz.normalized * 13f;
                    Vector3 y = Vector3.up * 3f;

                    playerPhysicsBus.AddForceToPlayer(xz + y, ForceMode.Impulse);
                }
            }

            yield return null;
        }

        isAttacking = false;
        agent.enabled = true; 
        trailRenderer.enabled = false;
    }

}
