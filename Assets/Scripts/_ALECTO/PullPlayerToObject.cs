using UnityEngine;
using UnityEngine.Rendering;

public class PullPlayerToObject : MonoBehaviour
{

    [SerializeField] private SpringJoint spring;
    //[SerializeField] private LineRenderer lr;
    [SerializeField] private Rigidbody rb;

    Rigidbody anchoredRigidBody;
    RaycastAnchorVisualizer anchorViz;
    RaycastHit hit;
    Vector3 worldAnchor;
    Vector3 localAnchor;

    float extraGravityForce = 19f; // this is used to help the playercontroller stick to the ground better. However it needs to be compensated for.
    //float pullForce = 35f;
    //float lrWidth = 0.3f;
    bool hasRigidBody;

    bool anchored;
    private void Start()
    {
        anchorViz = FindFirstObjectByType<RaycastAnchorVisualizer>();

        //lr.positionCount = 2;
        //lr.useWorldSpace = true;
        //lr.startWidth = lrWidth;
        //lr.endWidth = lrWidth;
        //lr.enabled = false;
    }

    void Update()
    {
        bool rightClickDown = Input.GetMouseButton(1);

        if (rightClickDown && !anchored) SetAnchor();
        else if (!rightClickDown && anchored) ClearAnchor();

    }

    private void FixedUpdate()
    {
        if (anchored)
        {
            Vector3 direction = Vector3.zero;

            if (hasRigidBody) 
            {
                Vector3 anchorW = anchoredRigidBody.transform.TransformPoint(localAnchor);
                direction = anchorW - transform.position;

                anchoredRigidBody.AddForceAtPosition(
                    -direction * rb.mass,
                    anchorW, 
                    ForceMode.Force);
                rb.AddForce(direction * (anchoredRigidBody.mass + extraGravityForce), ForceMode.Force);
            }
            else
            {
                direction = worldAnchor - transform.position; 
                rb.AddForce(direction * rb.mass, ForceMode.Force);
            }

        }

        RenderLine();
    }
    void SetAnchor()
    {
        hit = anchorViz.RaycastToEnv();
        if (hit.collider != null)
        {
            worldAnchor = hit.point;
            anchored = true;
            anchoredRigidBody = hit.collider.gameObject.GetComponent<Rigidbody>();
            hasRigidBody = anchoredRigidBody != null;

            if (hasRigidBody)
            {
                localAnchor = hit.collider.gameObject.transform.InverseTransformPoint(hit.point);
            }
        }
    }

    void ClearAnchor()
    {
        anchored = false;
    }

    void RenderLine()
    {
        //if (anchored)
        //{
        //    if (hasRigidBody) 
        //    {
        //        lr.SetPosition(1, anchoredRigidBody.transform.TransformPoint(localAnchor));
        //    }
        //    else
        //    {
        //        lr.SetPosition(1, worldAnchor);
        //    }

        //    lr.SetPosition(0, transform.position + new Vector3(0f, -0.35f, 0f));
        //    //lr.enabled = true;
        //}
        //else
        //{
        //    lr.enabled = false;
        //}
    }
}
