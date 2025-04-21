using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public static WaveManager i;
    [SerializeField] private GameObject spawnIndicator;

    [Header("enemy prefabs")]
    [SerializeField] private GameObject zomboPrefab;
    [SerializeField] private GameObject casterPrefab;
    [SerializeField] private GameObject diverPrefab;
    [SerializeField] private GameObject bouncerPrefab;
    [SerializeField] private GameObject ghoulPrefab;
    [SerializeField] private GameObject spinnerPrefab;

    [SerializeField] private GameObject arena1SpawnpointsParent;
    private List<Vector3> arena1Spawnpoints = new List<Vector3>();

    private float minRangeFromPlayerForSpawning = 12f;
    private float spawnDelay = 1.5f;
    private float firstSpawnDelay = 1.7f;

    public int EnemyWeightCap = 1; // throwaway value, just here to avoid nullref
    private int killCount = 0;

    private Dictionary<string, int> enemyWeightDict;
    private Dictionary<string, int> killCountReqDict;

    private float cycleIntervalLower = 5f;
    private float cycleIntervalUpper = 10f;

    private bool cycleReady = false;
    private int currentTotalWeightOfEnemies = 0;
    private void Awake()
    {
        if (i != null) Debug.LogError("multiple wavespawners");
        i = this;
        
        enemyWeightDict = new Dictionary<string, int>()
        {
            {"Zombo", 10 },
            {"Caster", 15 },
            {"Bouncer", 17 },
            {"Ghoul", 22 },
            {"Spinner", 27 },
            {"Diver", 18 },
        };

        killCountReqDict = new Dictionary<string, int>()
        {
            {"Zombo", 0 },
            {"Caster", 0 },
            {"Bouncer", 15 },
            {"Diver", 20 },
            {"Ghoul", 0 },
            {"Spinner", 40 },
        };
    }
    private void OnEnable()
    {
        MainEventBus.OnRunStart += OnRunStart;
        CombatEventBus.OnEnemyDeath += OnEnemyDeath;
    }
    private void OnDisable()
    {
        MainEventBus.OnRunStart -= OnRunStart;
        CombatEventBus.OnEnemyDeath -= OnEnemyDeath;
    }
    private void Start()
    {
        RepositionSpawnpoint[] points = GetComponentsInChildren<RepositionSpawnpoint>();
        foreach (var point in points)
        {
            point.Reposition();
        }

        StartCoroutine(WaitOneFrame()); // + spawn delay 
    }
    private IEnumerator WaitOneFrame()
    {
        yield return null;
        foreach (Transform t in arena1SpawnpointsParent.transform)
        {
            arena1Spawnpoints.Add(t.position);
            t.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(firstSpawnDelay);
        cycleReady = true;
        //Debug.Log(arena1Spawnpoints.Count);
    }
    private void Update()
    {
        if (cycleReady) Cycle();
    }
    private void Cycle()
    {
        EnemyWeightCap++;
        cycleReady = false;
        List<string> enemyBatch = GetEnemyBatch();
        SpawnBatch(enemyBatch);

        float randomIntervalBeforeNextCycle = Random.Range(cycleIntervalLower, cycleIntervalUpper);
        StartCoroutine(CycleTimer(randomIntervalBeforeNextCycle));
    }
    
    private IEnumerator CycleTimer(float duration) 
    {
        yield return new WaitForSeconds(duration);
        cycleReady = true;
    }
    private GameObject DecodeEnemyType(string enemyType)
    {
        switch (enemyType)
        {
            case "Zombo":
                return zomboPrefab;
            case "Caster":
                return casterPrefab;
            case "Bouncer":
                return bouncerPrefab;
            case "Diver":
                return diverPrefab;
            case "Ghoul":
                return ghoulPrefab;
            case "Spinner":
                return spinnerPrefab;
            default:
                return null;
        }
    }

    private List<string> GetEnemyBatch()
    {
        List<string> availablePool = new List<string>();

        foreach (var kvp in killCountReqDict) // get the pool of enemies that can be picked based on the killcount requirements
        {
            if (kvp.Value <= killCount)
            {
                availablePool.Add(kvp.Key);
            }
        }

        int lowestWeightInPool = 999;
        string lowestEnemy = "";
        foreach (var enemy in availablePool)
        {
            if (enemyWeightDict[enemy] < lowestWeightInPool)
            {
                lowestWeightInPool = enemyWeightDict[enemy];
                lowestEnemy = enemy;
            }
        }

        List<string> enemyBatch = new List<string>();
        int i = 0;
        int remainingWeight = EnemyWeightCap - currentTotalWeightOfEnemies;
        while (remainingWeight > lowestWeightInPool) // randomly add to the batch from the pool of available enemies until remainingweight is less than the lowest cost unit in the pool
        {
            string randomEnemy = availablePool[Random.Range(0, availablePool.Count)];
            if (enemyWeightDict[randomEnemy] < remainingWeight)
            {
                enemyBatch.Add(randomEnemy);
                remainingWeight -= enemyWeightDict[randomEnemy];
            }
            else
            {
                availablePool.Remove(randomEnemy); // remove if enemy weight exceeds the remainingweight
            }
            i++;
            if (i > 333) break; // safety..
        }

        return enemyBatch;
    }

    private void SpawnBatch(List<string> batch)
    {
        for (int i = 0; i < batch.Count; i++)
        {
            Vector3 prospectiveSpawnpoint;
            int attempts = 0;
            do
            {
                if (arena1Spawnpoints.Count > 1)
                {
                    prospectiveSpawnpoint = arena1Spawnpoints[Random.Range(0, arena1Spawnpoints.Count)];
                }
                else
                {
                    //Debug.Log(arena1Spawnpoints.Count);
                    prospectiveSpawnpoint = arena1Spawnpoints[0];
                }
                if (attempts > 100) break;
            }
            while (Vector3.Distance(prospectiveSpawnpoint, Game.i.PlayerTransform.position) < minRangeFromPlayerForSpawning);


            GameObject spawnIndicatorInstance = Instantiate(spawnIndicator, prospectiveSpawnpoint + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized, Quaternion.identity);

            string enemyType = batch[Random.Range(0, batch.Count)];
            GameObject prefab = DecodeEnemyType(enemyType);

            if (prefab != null)
            {
                spawnIndicatorInstance.GetComponent<SpawnIndicator>().Setup(spawnDelay, prefab);
                currentTotalWeightOfEnemies += prefab.GetComponentInChildren<EnemyUnit>().WaveWeight;
            }
            else Debug.LogError("invalid enemy in wavemanager");
        }
    }
    void OnRunStart() { }

    void OnEnemyDeath(EnemyUnit enemyUnit, Vector3 pos)
    {
        currentTotalWeightOfEnemies -= enemyUnit.WaveWeight;
        killCount++;
    }

    public bool CommandSpawnEnemy(string name)
    {
        return true;
    }
}
