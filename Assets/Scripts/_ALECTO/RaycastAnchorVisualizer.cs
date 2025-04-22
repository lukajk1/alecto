using UnityEngine;

public class RaycastAnchorVisualizer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject hitIndicator;

    string pullableLayerName = "Pullable";
    int pullableLayer;
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
        pullableLayer = LayerMask.GetMask(pullableLayerName);
    }
    private void Update()
    {
        origin = transform.position + transform.forward * 1.1f;

        RaycastHit hit = RaycastToEnv();
        if (hit.collider != null)
        {
            hitIndicator.transform.position = hit.point;
            HitIndicatorActive = true;
            SetSizeOfIndicator();
        }
        else
        {
            HitIndicatorActive = false;
        }
    }

    public RaycastHit RaycastToEnv()
    {
        RaycastHit hit;
        Physics.Raycast(origin, transform.forward, out hit, lineRange, pullableLayer, QueryTriggerInteraction.Collide);
        return hit;
    }


    void SetSizeOfIndicator()
    {
        float referenceDistance = 15f;

        float distance = Vector3.Distance(cam.transform.position, hitIndicator.transform.position);
        float scaleFactor = distance / referenceDistance * 0.85f;

        hitIndicator.transform.localScale = originalIndicatorScale * scaleFactor;

    }

}
