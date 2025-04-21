using UnityEngine;

public class InteractableIndicator : MonoBehaviour
{
    private Camera playerCamera;

    private Transform playerTransform;
    private float activationDistance = 12f;
    private Canvas canvas;
    private bool isVisible = false;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }

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

        if (playerTransform != null)
        {
            // Calculate squared distance to avoid unnecessary sqrt operation
            float sqrDistance = (transform.position - playerTransform.position).sqrMagnitude;
            float sqrActivationDistance = activationDistance * activationDistance;

            if (sqrDistance <= sqrActivationDistance && !isVisible)
            {
                // Enable canvas if within range
                SetVisibility(true);
            }
            else if (sqrDistance > sqrActivationDistance && isVisible)
            {
                // Disable canvas if out of range
                SetVisibility(false);
            }
        }
    }

    private void SetVisibility(bool visible)
    {
        isVisible = visible;
        if (canvas != null)
        {
            canvas.enabled = visible;
        }
    }
}
