using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(MeshRenderer))]
public class LaserSight : MonoBehaviour
{
    LineRenderer lr;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    private void LateUpdate()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, Camera.main.transform.position + Camera.main.transform.forward * 99f);
    }
}
