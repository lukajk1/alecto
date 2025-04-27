using UnityEngine;

public class SubmergeBody : MonoBehaviour
{
    private void FixedUpdate()
    {
        PerformOverlap();
    }
    private void PerformOverlap()
    {
        Vector3 center = transform.position;
        Vector3 halfExtents = transform.lossyScale * 0.5f; // half of world-scale dimensions
        Quaternion orientation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, orientation);

        foreach (Collider hit in hits)
        {
            Debug.Log("Overlap detected: " + hit.name);
        }
    }
}
