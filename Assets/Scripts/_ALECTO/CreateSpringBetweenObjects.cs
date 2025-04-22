using UnityEngine;

public class CreateSpringBetweenObjects : MonoBehaviour
{

    //bool anchor1AttachmentMade;
    //SpringJoint spring;
    //GameObject objectToAddSpringTo;
    //Vector3 anchor1;
    //Vector3 pullMyselfAnchor = Vector3.zero;

    //private void Update()
    //{

    //    bool clicked = Input.GetMouseButtonDown(0);
    //    bool rightClickDown = Input.GetMouseButton(1);

    //    if (
    //        clicked
    //        && objectHasRigidbody
    //        && !anchor1AttachmentMade
    //        )
    //    {
    //        objectToAddSpringTo = hit.collider.gameObject;


    //        anchor1 = hit.transform.InverseTransformPoint(hit.point);
    //        anchor1AttachmentMade = true;

    //    }
    //    else if (
    //        clicked
    //        && objectHasRigidbody
    //        && anchor1AttachmentMade
    //        )
    //    {
    //        spring = objectToAddSpringTo.AddComponent<SpringJoint>();

    //        spring.anchor = anchor1;
    //        spring.autoConfigureConnectedAnchor = false;
    //        spring.damper = 5f;
    //        spring.minDistance = 0f;
    //        spring.maxDistance = 0f;
    //        spring.spring = 10f;
    //        spring.enableCollision = true;
    //        spring.connectedBody = hit.collider.GetComponent<Rigidbody>();
    //        spring.connectedAnchor = hit.collider.transform.InverseTransformPoint(hit.point);

    //        LineRenderer lr = spring.gameObject.AddComponent<LineRenderer>();

    //        spring.gameObject.AddComponent<SpringJointVisualizer>().Initialize(lr);

    //    }
    //}
}
