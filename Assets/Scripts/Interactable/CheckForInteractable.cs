using UnityEngine;

public class CheckForInteractable : MonoBehaviour
{
    [SerializeField] private Camera fpCamera;

    private const float Range = 6.0f;

    private RaycastHit hit;
    [SerializeField] private InteractPrompt interactPrompt;

    bool currentInteractState;
    bool isInteractable;

    bool pressedInteract;
    private void Start()
    {
        
    }
    private void Update()
    {
        isInteractable = false;

        pressedInteract = Input.GetKeyDown(KeyCode.F);

        Vector3 pos = fpCamera.transform.position;
        Vector3 fPos = fpCamera.transform.forward;
        Debug.DrawLine(pos + (fPos * 0.3f), (pos + fPos) + (fPos * Range), Color.red, 2f);

        if (Physics.Raycast(pos + (fPos * 0.3f), fPos, out hit, Range)) // start the raycast a little further out so player-related colliders don't block it
        {
            isInteractable = hit.collider.CompareTag("Interact");
        }

        if (isInteractable != currentInteractState)
        {
            currentInteractState = isInteractable;
            interactPrompt.SetInteract(currentInteractState);
        }

        if (isInteractable && pressedInteract)
        {
            hit.collider.GetComponent<Interactable>().OnInteract();
        }
    }
}
