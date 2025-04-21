using UnityEngine;

[RequireComponent(typeof(SpringJoint))]
[RequireComponent(typeof(LineRenderer))]
public class SpringJointVisualizer : MonoBehaviour
{
    private SpringJoint springJoint;
    private LineRenderer lineRenderer;

    void Awake()
    {
        springJoint = GetComponent<SpringJoint>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    void Update()
    {
        if (springJoint.connectedBody != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, springJoint.connectedBody.position);
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
