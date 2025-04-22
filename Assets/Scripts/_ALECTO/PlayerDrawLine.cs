using UnityEngine;

public class PlayerDrawLine : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject hitIndicator;

    private Vector3 originalIndicatorScale;

    private readonly float lineRange = 50f;

    private Vector3 origin;
    private bool _hitIndicatorActive;
    private bool HitIndicatorActive
    {
        get => _hitIndicatorActive;
        set
        {
            if (_hitIndicatorActive != value)
            {
                _hitIndicatorActive = value;
                hitIndicator.SetActive(value);
            }
        }
    }

    private Vector3 positionInFrontOfPlayer;

    private void Start()
    {
        originalIndicatorScale = hitIndicator.transform.localScale;
        positionInFrontOfPlayer = transform.position + (transform.forward * 5f);
    }
    bool anchor1AttachmentMade;
    SpringJoint spring;
    GameObject objectToAddSpringTo;
    Vector3 anchor1;
    private void Update()
    {
        origin = transform.position + transform.forward * 1.1f;

        bool clicked = Input.GetMouseButtonDown(0);

        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, lineRange))
        {
            hitIndicator.transform.position = hit.point;
            HitIndicatorActive = true;
            SetSizeOfIndicator();

            bool objectHasRigidbody = hit.collider.GetComponent<Rigidbody>() != null;

            if (
                clicked
                && objectHasRigidbody
                && !anchor1AttachmentMade 
                )
            {
                objectToAddSpringTo = hit.collider.gameObject;


                anchor1 = hit.transform.InverseTransformPoint(hit.point);
                anchor1AttachmentMade = true;

            }
            else if (
                clicked
                && objectHasRigidbody
                && anchor1AttachmentMade
                )
            {
                spring = objectToAddSpringTo.AddComponent<SpringJoint>();

                spring.anchor = anchor1;
                spring.autoConfigureConnectedAnchor = false;
                spring.damper = 5f;
                spring.minDistance = 0f;
                spring.maxDistance = 0f;
                spring.spring = 10f;
                spring.enableCollision = true;
                spring.connectedBody = hit.collider.GetComponent<Rigidbody>();
                spring.connectedAnchor = hit.collider.transform.InverseTransformPoint(hit.point);

                LineRenderer lr = spring.gameObject.AddComponent<LineRenderer>();

                spring.gameObject.AddComponent<SpringJointVisualizer>().Initialize(lr);

            }
        }
        else
        {
            HitIndicatorActive = false;
        }


    }


    void SetSizeOfIndicator()
    {
        float referenceDistance = 15f;

        float distance = Vector3.Distance(cam.transform.position, hitIndicator.transform.position);
        float scaleFactor = distance / referenceDistance;

        hitIndicator.transform.localScale = originalIndicatorScale * scaleFactor;

    }

}
