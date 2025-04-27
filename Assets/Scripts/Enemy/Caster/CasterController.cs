using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.AI;

public class CasterController : UnitController
{
    //[SerializeField] private EnemyUnit enemyUnit;
    [SerializeField] private GameObject casterBullet;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Animator animator;
    [SerializeField] private DissolveEffect dissolve;

    private float movespeed;
    private NavMeshAgent agent;
    private bool allowedToMove = false;

    private float bulletMaxDuration = 5f;
    private float bulletSpeed = 7.4f;
    private bool canAttack = true;
    private float windupLength = 0.9f;

    private float damping = 6f;

    [SerializeField] private Bobbing bobbing;
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

    private void Setup()
    {
        movespeed = enemyUnit.BaseMoveSpeed;
        allowedToMove = true;
    }

    protected override void Start()
    {
        base.Start();
        bobbing.StartBobbing();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movespeed;
        agent.destination = Game.i.PlayerTransform.position;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            agent.transform.position = hit.position;
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Game.i.PlayerTransform.position) > enemyUnit.AttackRange)
        {
            agent.isStopped = false;
            if (allowedToMove) agent.destination = Game.i.PlayerTransform.position;
        }
        else
        {
            agent.isStopped = true;
            TryAttack();
        }

        // look at player w/ smoothing
        Vector3 dir = Game.i.PlayerTransform.position - transform.position;
        dir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, damping * Time.deltaTime);

    }
    private void OnDeath()
    {
        dissolve.Begin();
    }

    private void TryAttack()
    {
        if (!canAttack ) return;

        //Debug.Log("attempting to attack");
        Ray ray = new Ray(transform.position, Game.i.PlayerTransform.position - transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AttackWindup());

        }
    }
    private IEnumerator AttackWindup()
    {
        canAttack = false;
        animator.SetTrigger("Cast");


        //Debug.Log("starting");
        float elapsed = 0f;
        while (elapsed < windupLength)
        {
            elapsed += Time.deltaTime;
            if (enemyUnit.IsDead) yield break;
            yield return null;
        }
        //Debug.Log("casting now");

        Vector3 dir = Game.i.PlayerTransform.position - transform.position;

        GameObject bullet = Instantiate(casterBullet, bulletOrigin.position, Quaternion.identity);
        bullet.GetComponent<CasterBullet>().Initialize(dir.normalized, bulletMaxDuration, enemyUnit.BaseDamage, bulletSpeed);

        SFXManager.i.PlaySFXClip(EnemySFXList.i.casterAttack, transform.position, type: SFXManager.SoundType.HasFalloff);

        StartCoroutine(AttackCD());

    }
    private IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(enemyUnit.AttackCDLength);
        canAttack = true;
    }

}
