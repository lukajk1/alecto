using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    private float chanceOfDropping = 1f / 20f;
    private void OnEnable()
    {
        CombatEventBus.OnEnemyDeath += OnEnemyDeath;
    }    
    private void OnDisable()
    {
        CombatEventBus.OnEnemyDeath -= OnEnemyDeath;
    }
    void OnEnemyDeath(EnemyUnit enemyUnit, Vector3 deathPos)
    {
        float chance = Random.Range(0f, 1.0f);
        if (chance <= chanceOfDropping)
        {
            // determine which weapon to drop
            chance = Random.Range(0f, 1.0f);

            Weapon[] weapons = { new BloodSiphon(), new Spear(), new ChainDaggers() };
            int index = Mathf.FloorToInt(chance * weapons.Length);
            index = Mathf.Clamp(index, 0, weapons.Length - 1); // safety

            FindFirstObjectByType<SpawnDroppedWeapon>().CreateDrop(weapons[index], deathPos);
            SFXManager.i.PlaySFXClip(EnemySFXList.i.enemyDrop, Game.i.PlayerTransform.position);
        }
    }
}
