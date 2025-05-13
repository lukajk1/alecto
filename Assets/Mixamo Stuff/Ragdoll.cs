using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(Animator))]
[RequireComponent (typeof(EnemyUnit))]
[RequireComponent (typeof(NavMeshAgent))]
public class Ragdoll : MonoBehaviour
{
    Animator controller;
    NavMeshAgent navAgent;
    Rigidbody[] rigidbodies;
    private void Start()
    {
        controller = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

        rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            AgentLimb limb = rb.gameObject.AddComponent<AgentLimb>();
            limb.Initialize(GetComponent<EnemyUnit>());
        }

        SetRagdollPhysics(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Enable();
        }
    }
    public void Enable()
    {
        controller.enabled = false;
        navAgent.enabled = false;
        SetRagdollPhysics(true);
    }

    void SetRagdollPhysics(bool active)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.useGravity = active;
            rb.isKinematic = !active;
        }
    }
}
