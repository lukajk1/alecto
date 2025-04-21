using UnityEngine;

public class BouncerSpin : MonoBehaviour
{
    public float rotationDegPerSec = 400f;
    private Vector3 rotVector = new Vector3(1f, 0f, 0f);

    private void Update()
    {
        transform.Rotate(rotVector, rotationDegPerSec * Time.deltaTime);
    }
}
