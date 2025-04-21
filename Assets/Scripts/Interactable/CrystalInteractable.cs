using UnityEngine;

public class CrystalInteractable : Interactable
{
    public override void OnInteract()
    {
        SwapArenas.I.RandomlySwitchArenas();
        Destroy(gameObject);
    }
}
