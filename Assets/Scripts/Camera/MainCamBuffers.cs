using UnityEngine;

public class MainCamBuffers : MonoBehaviour
{
    public static MainCamBuffers i;
    private Camera cam;

    // the buffers store the exact value the property of the camera should be set to that frame
    private Vector3 _rotationBuffer;
    public Vector3 RotationBuffer
    {
        get => _rotationBuffer;
        set
        {
            if (value != _rotationBuffer)
            {
                _rotationBuffer = value;
            }
        }
    }    
    private float _fovBuffer;
    public float FOVBuffer
    {
        get => _fovBuffer;
        set
        {
            if (value != _fovBuffer)
            {
                _fovBuffer = value;
            }
        }
    }

    private void Awake()
    {
        if (i != null) Debug.LogError($"More than one instance of {i} in scene");
        i = this;
    }
    private void Start()
    {
        cam = Camera.main;
    }
    void LateUpdate()
    {
        // rotation
        cam.transform.localEulerAngles = RotationBuffer;
        RotationBuffer = Vector3.zero;

        // fov
    }
}
