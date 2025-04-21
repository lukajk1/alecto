using UnityEngine;
using UnityEngine.AI;

public class ZomboController : UnitController
{
    //[SerializeField] private EnemyUnit enemyUnit;
    private float movespeed;
    private NavMeshAgent agent;

    private bool allowedToMove = false;
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
        movespeed = enemyUnit.CurrentMoveSpeed;
        allowedToMove = true;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movespeed;
        agent.destination = Game.i.PlayerTransform.position;
    }

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (allowedToMove)
        {
            agent.destination = Game.i.PlayerTransform.position;
        }

        if (enemyUnit.AttackReady && Vector3.Distance(Game.i.PlayerTransform.position, transform.position) < enemyUnit.AttackRange)
        {
            enemyUnit.AttackReady = false;
            Game.i.PlayerUnitInstance.TakeDamage(false, enemyUnit.BaseDamage);
            StartCoroutine(timer.TimerCR(enemyUnit.AttackCDLength, () => enemyUnit.AttackReady = true));
        }

    }

}
