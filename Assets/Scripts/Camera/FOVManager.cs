using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FOVManager : MonoBehaviour
{
    private Camera cam;

    public float FOV
    {
        get
        {
            return cam.fieldOfView;
        }
        set
        {
            if (value > 0 && value < 180)
            {
                cam.fieldOfView = value;
            }
        }
    }

    void Start()
    {
        cam = GetComponent<Camera>();
    }
}
