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
    private void Start()
    {
        originalIndicatorScale = hitIndicator.transform.localScale;
    }

    private void Update()
    {
        origin = transform.position + transform.forward * 1.1f;


        RaycastHit hit;
        if (Physics.Raycast(origin, transform.forward, out hit, lineRange))
        {
            hitIndicator.transform.position = hit.point;
            HitIndicatorActive = true;
            SetSizeOfIndicator();
        }
        else
        {
            HitIndicatorActive = false;
        }



        if (hit.collider.GetComponent<Rigidbody>() != null)
        {

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
