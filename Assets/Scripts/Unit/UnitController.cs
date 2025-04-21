using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    [SerializeField] protected EnemyUnit enemyUnit;
    protected Timer timer;
    protected virtual void Start()
    {
        timer = gameObject.AddComponent<Timer>();
    }

    protected bool InRangeOfPlayer(Vector3 posToCheckFrom, float range)
    {
        if (Vector3.Distance(Game.i.PlayerTransform.position, posToCheckFrom) <= range)
        {
            return true;
        }
        else return false;
    }
}
