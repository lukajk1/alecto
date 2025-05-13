using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EliteController : MonoBehaviour
{
    NavMeshAgent navAgent;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (navAgent.enabled)
            navAgent.destination = Game.i.PlayerTransform.position;
    }
}
