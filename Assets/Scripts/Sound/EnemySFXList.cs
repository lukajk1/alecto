using UnityEngine;

public class EnemySFXList : MonoBehaviour
{
    public static EnemySFXList i;
    [SerializeField] public AudioClip enemySpawn;
    [SerializeField] public AudioClip casterAttack;
    [SerializeField] public AudioClip enemyDrop;
    private void Awake()
    {
        if (i == null) i = this;
    }
}
