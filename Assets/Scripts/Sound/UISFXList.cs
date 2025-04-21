using UnityEngine;

public class UISFXList : MonoBehaviour
{
    public static UISFXList i;
    private void Awake()
    {
        if (i == null) i = this;
    }

    [SerializeField] public AudioClip weaponFire;
    [SerializeField] public AudioClip reloadFinished;
    [SerializeField] public AudioClip outOfBullets;
    [SerializeField] public AudioClip enemyCritHit;
    [SerializeField] public AudioClip enemyBodyHit;
    [SerializeField] public AudioClip enemyDeath;
    [Header("Weapons")]
    [SerializeField] public AudioClip daggerSwitchIn;

}
