using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected virtual void Awake()
    {
        gameObject.tag = "Interact";
    }
    public abstract void OnInteract();
}
