using UnityEngine;

public class CasingOnCollisionSound : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SFXManager.i.PlaySFXClip(PlayerSFXList.i.casingBounce, transform.position);
        Destroy(this);
    }
}
