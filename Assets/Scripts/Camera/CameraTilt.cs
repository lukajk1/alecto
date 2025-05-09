using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraTilt : MonoBehaviour
{
    Camera cam;
    Quaternion rotationBuffer;
    InputAction move;
    Coroutine leanCR;
    float leanAngle = 4f;

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
            lean = -1f;
        }        
        else if (Input.GetKey(KeyCode.D))
        {
            lean = 1f;
        }
        //else
        //{
        //    lean = 0f;
        //}

        Tilt = lean;

        if (leanCR != null)
            MainCamBuffer.i.RotationBuffer += rotationBuffer.eulerAngles;
    }


    IEnumerator LeanEase(float targetZ)
    {
        rotationBuffer = cam.transform.rotation;

        float duration = 0.11f;
        float time = 0f;

        float startZ = cam.transform.localEulerAngles.z;
        if (startZ > 180f) startZ -= 360f; // normalize to [-180, 180]

        while (time < duration)
        {
            float t = time / duration;
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
