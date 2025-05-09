using UnityEngine;

public class MainCamBuffer : MonoBehaviour
{
    public static MainCamBuffer i;
    private Camera cam;
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
