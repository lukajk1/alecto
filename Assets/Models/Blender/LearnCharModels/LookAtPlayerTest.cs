using UnityEngine;

public class LookAtPlayerTest : MonoBehaviour
{
    [SerializeField] Transform myBone;
    void LateUpdate()
    {
        myBone.LookAt(Game.i.PlayerTransform.position);
    }
}
