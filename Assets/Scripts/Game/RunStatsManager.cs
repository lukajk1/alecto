using NUnit;
using UnityEngine;

public class RunStatsManager : MonoBehaviour
{
    private bool runOngoing;

    private float elapsedRunTime;
    private int enemiesKilled;
    private int shotsFired;
    private int score;
    private int damageDealt;
    private int damageTaken;
    private int criticalHits;
    private int totalHitsLanded;

    private void OnEnable()
    {
        MainEventBus.OnRunStart += Initialize;
        MainEventBus.OnRunEnd += EndRun;
        CombatEventBus.OnEnemyDeath += OnEnemyDeath;
        CombatEventBus.OnWeaponFired += OnWeaponFired;
        CombatEventBus.OnEnemyHit += OnEnemyHit;
        CombatEventBus.OnPlayerHit += OnPlayerHit;
    }

    private void OnDisable()
    {
        MainEventBus.OnRunStart -= Initialize;
        MainEventBus.OnRunEnd -= EndRun;
        CombatEventBus.OnEnemyDeath -= OnEnemyDeath;
        CombatEventBus.OnWeaponFired -= OnWeaponFired;
        CombatEventBus.OnEnemyHit -= OnEnemyHit;
        CombatEventBus.OnPlayerHit -= OnPlayerHit;
    }

    private void Initialize()
    {
        runOngoing = true;
    }
    private void EndRun()
    {
        runOngoing = false;
    }
    private void OnPlayerHit(int damage, bool isCrit)
    {
        damageTaken += damage;
    }

    private void OnEnemyDeath(EnemyUnit enemyUnit, Vector3 pos)
    {
        enemiesKilled++;
        Game.Score += enemyUnit.ScoreWeight;
    }

    private void OnWeaponFired(Weapon weapon)
    {
        shotsFired++;
    }

    private void OnEnemyHit(int damage, bool isCrit, Vector3 pos)
    {
        damageDealt += damage;
        if (isCrit)
        {
            criticalHits++;
            totalHitsLanded++;
        }
        else
        {
            totalHitsLanded++;
        }
    }

    private void Update()
    {
        if (Game.MenusOpen == 0 && runOngoing)
        {
            elapsedRunTime += Time.deltaTime;
        }
    }

    public RunStats RequestStats()
    {
        return new RunStats(
            elapsedRunTime,
            Game.Score,
            enemiesKilled,
            shotsFired,
            damageDealt,
            damageTaken,
            criticalHits,
            totalHitsLanded
            );
    }
    public void PrintStats()
    {
        int minutes = (int)(elapsedRunTime / 60f);
        int seconds = (int)(elapsedRunTime % 60f);
        Debug.Log($"===");
        Debug.Log($"run length: {minutes:00}:{seconds:00}"); 
        Debug.Log($"shots fired: {shotsFired:N0}");
        Debug.Log($"enemies slain: {enemiesKilled:N0}");
        Debug.Log($"critical hit %: {(criticalHits * 100f / totalHitsLanded):0.0}%");
        Debug.Log($"overall accuracy: {(totalHitsLanded * 100f / shotsFired):0.0}%");
        Debug.Log($"total damage dealt: {damageDealt:N0}");
    }
}
