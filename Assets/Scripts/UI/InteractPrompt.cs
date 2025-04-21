using UnityEngine;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt;

    public void SetInteract(bool value)
    {
        interactPrompt.SetActive(value);
    }
}
