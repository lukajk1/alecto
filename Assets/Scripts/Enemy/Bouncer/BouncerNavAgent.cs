using UnityEngine;
using UnityEngine.AI;

public class BouncerNavAgent : MonoBehaviour
{

    [SerializeField] private Transform bouncerBody;
    [SerializeField] private Bouncer bouncerUnit;
    private float movespeed;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = bouncerUnit.CurrentMoveSpeed;
    }

    private void Update()
    {
        agent.destination = Game.i.PlayerTransform.position;
    }
}
