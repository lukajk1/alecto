using UnityEngine;

public struct RunStats
{
    public float elapsedRunTime;
    public int enemiesKilled;
    public int shotsFired;
    public int score;
    public int damageDealt;
    public int damageTaken;
    public int criticalHits;
    public int totalHitsLanded;

    public RunStats(float elapsedRunTime, int score, int enemiesKilled, int shotsFired, 
                        int damageDealt, int damageTaken, int criticalHits, int totalHitsLanded)
    {
        this.elapsedRunTime = elapsedRunTime;
        this.enemiesKilled = enemiesKilled;
        this.shotsFired = shotsFired;
        this.score = score;
        this.damageDealt = damageDealt;
        this.damageTaken = damageTaken;
        this.criticalHits = criticalHits;
        this.totalHitsLanded = totalHitsLanded;
    }

    public void ResetStats()
    {
        elapsedRunTime = 0;
        enemiesKilled = 0;
        shotsFired = 0;
        score = 0;
        damageDealt = 0;
        damageTaken = 0;
        criticalHits = 0;
        totalHitsLanded = 0;
    }
}

