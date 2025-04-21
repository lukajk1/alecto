using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.InputSystem.HID;

public class BouncerController : UnitController
{
    //[SerializeField] private EnemyUnit enemyUnit;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform bouncerBody;

    private PlayerPhysicsBus playerPhysicsBus;
    private Vector3 targetBouncePos;
    private bool isGrounded;

    private float extraGravityForce = 8f;
    private float jumpForce = 8.5f;
    private bool attackUp = true;

    [SerializeField] private NavMeshAgent agent;

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
        enemyUnit.AttackReady = true;
        playerPhysicsBus = FindFirstObjectByType<PlayerPhysicsBus>();
    }

    protected override void Start()
    {
        base.Start();
    }
    private void FixedUpdate()
    {

        rb.AddForce(Vector3.down * extraGravityForce, ForceMode.Force);

        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, 0.9f);
        if (isGrounded)
        {
            Bounce();
        }



    }

    void Update()
    {
        if (Vector3.Distance(transform.position, Game.i.PlayerTransform.position) <= enemyUnit.AttackRange)
        {
            if (attackUp) Attack();
        }

        if (Vector3.Distance(transform.position, agent.transform.position) > 3f)
        {
            transform.position = agent.transform.position;
            //Debug.Log("snapping");
        }
    }
    void Attack()
    {
        Game.i.PlayerUnitInstance.TakeDamage(false, enemyUnit.BaseDamage);
        KnockbackPlayer();
        attackUp = false;
        StartCoroutine(timer.TimerCR(enemyUnit.AttackCDLength, () => attackUp = true));
    }


    private void KnockbackPlayer()
    {
        Vector3 xz = transform.position - Game.i.PlayerTransform.position;
        xz = xz.normalized * 22f;
        Vector3 y = Vector3.up * 3f;

        playerPhysicsBus.AddForceToPlayer(xz + y, ForceMode.Impulse);
    }
    private void Bounce()
    {
        //Vector3 targetPos = Game.i.PlayerTransform.position;
        //targetPos.y = transform.position.y;
        //bouncerBody.LookAt(targetPos);

        Vector3 dir = agent.velocity;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


}
