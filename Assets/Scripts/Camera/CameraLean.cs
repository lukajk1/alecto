using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLean : MonoBehaviour
{
    Camera cam;
    Quaternion rotationBuffer;
    InputAction move;
    Coroutine leanCR;

    [Range(0.1f, 8f)] [SerializeField] private float leanAngle; 
    [Range(0.1f, 1f)] [SerializeField] private float leanTransitionDuration;

    float _tilt;

    float Tilt
    {
        get => _tilt;
        set
        {
            if (_tilt != value)
            {
                _tilt = value;

                if (leanCR != null)
                    StopCoroutine(leanCR);
                
                leanCR = StartCoroutine(LeanEase(value * leanAngle));
            }
        }
    }

    bool leaning;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float lean = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            lean = 1f;
        }        
        else if (Input.GetKey(KeyCode.D))
        {
            lean = -1f;
        }

        Tilt = lean;

        if (leanCR != null)
            MainCamBuffers.i.RotationBuffer += rotationBuffer.eulerAngles;
    }


    IEnumerator LeanEase(float targetZ)
    {
        rotationBuffer = cam.transform.rotation;

        float time = 0f;

        float startZ = cam.transform.localEulerAngles.z;
        if (startZ > 180f) startZ -= 360f; // normalize to [-180, 180]

        while (time < leanTransitionDuration)
        {
            float t = time / leanTransitionDuration;
            float z = Mathf.Lerp(startZ, targetZ, t);
            rotationBuffer = Quaternion.Euler(0f, 0f, z);
            time += Time.deltaTime;
            yield return null;
        }

        while (Tilt != 0)
        {
            rotationBuffer = Quaternion.Euler(0f, 0f, targetZ);
            yield return null;
        }

        leanCR = null;
    }
}
