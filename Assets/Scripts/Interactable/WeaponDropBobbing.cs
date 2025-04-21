using UnityEngine;

public class WeaponDropBobbing : MonoBehaviour
{
    private float bobbingSpeed = 2f;
    private float bobbingHeight = 0.2f;

    private float originalY;
    private bool canStartBobbing;
    private void Update()
    {
        if (canStartBobbing) ApplyBobbing();
    }
    public void StartBobbing()
    {
        if (canStartBobbing) return;

        originalY = transform.position.y;
        canStartBobbing = true;
    }
    void ApplyBobbing()
    {
        float newY = originalY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
