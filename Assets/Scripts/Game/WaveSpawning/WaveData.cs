using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct WaveData
{
    public int minEnemies;
    public int maxEnemies;
    public List<GameObject> enemyTypes;

    public WaveData(int minEnemies, int maxEnemies, List<GameObject> enemyTypes)
    {
        this.minEnemies = minEnemies;
        this.maxEnemies = maxEnemies;
        this.enemyTypes = enemyTypes;
    }
}
