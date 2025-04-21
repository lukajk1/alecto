using UnityEngine;

public class WeaponCameraSyncFov : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private void Start()
    {
        //cam = GetComponent<Camera>();
    }
    public void SyncFOV(float fov)
    {
        cam.fieldOfView = fov;
    }
}
