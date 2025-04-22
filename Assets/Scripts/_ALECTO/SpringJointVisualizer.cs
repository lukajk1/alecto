using UnityEngine;

public class SpringJointVisualizer : MonoBehaviour
{
    SpringJoint springJoint;
    LineRenderer lr;
    Transform connectedBodyTransform;

    float lrWidth = 0.3f;

    bool initialized;
    public void Initialize(LineRenderer lr)
    {
        this.lr = lr;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.startWidth = lrWidth;
        lr.endWidth = lrWidth;
        lr.enabled = true;

        springJoint = GetComponent<SpringJoint>();
        connectedBodyTransform = springJoint.connectedBody.gameObject.transform;
        Debug.Log(springJoint.connectedBody.gameObject.name);

        initialized = true;
    }

    void Update()
    {
        //if (initialized)
        //{
        //    lineRenderer.SetPosition(0, transform.position);
        //    lineRenderer.SetPosition(1, springJoint.connectedBody.position);
        //    lineRenderer.enabled = true;
        //}

        if (initialized)
        {
            lr.SetPosition(0, transform.TransformPoint(springJoint.anchor));
            lr.SetPosition(1, connectedBodyTransform.TransformPoint(springJoint.connectedAnchor));
            //lr.SetPosition(1, transform.TransformPoint(springJoint.connectedAnchor));
            //lr.SetPosition(1, Game.i.PlayerTransform.position);
        }
    }
}
