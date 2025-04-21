using UnityEngine;

public class EnvSFXList : MonoBehaviour
{
    public static EnvSFXList i;
    private void Awake()
    {
        if (i == null) i = this;
    }
    [SerializeField] public AudioClip crystalSpawn;
}
