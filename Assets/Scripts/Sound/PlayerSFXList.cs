using UnityEngine;

public class PlayerSFXList : MonoBehaviour
{
    public static PlayerSFXList i;
    private void Awake()
    {
        if (i == null) i = this;
    }
    [SerializeField] public AudioClip walljump;
    [SerializeField] public AudioClip slowfall;
    [SerializeField] public AudioClip jumpTakeoff;
    [SerializeField] public AudioClip jumpLanding;
    [SerializeField] public AudioClip slide;
    [SerializeField] public AudioClip hurt;
    [SerializeField] public AudioClip footstep;
    [SerializeField] public AudioClip weaponBreak;
}
