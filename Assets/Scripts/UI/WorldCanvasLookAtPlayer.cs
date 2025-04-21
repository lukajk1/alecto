using UnityEngine;
using UnityEngine.UI;

public class WorldCanvasLookAtPlayer : MonoBehaviour
{
    private Camera playerCamera;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = Game.i.PlayerTransform;
        playerCamera = Game.i.PlayerCamera;
    }

    private void Update()
    {
        if (playerCamera != null)
        {
            transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward,
                             playerCamera.transform.rotation * Vector3.up);
        }
    }
}
