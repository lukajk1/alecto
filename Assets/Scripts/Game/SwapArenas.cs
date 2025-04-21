using System;
using System.Collections.Generic;
using UnityEngine;

public class SwapArenas : MonoBehaviour
{
    public static SwapArenas I;
    private List<(Transform, GameObject)> arenas;

    [SerializeField] private Transform arena1SpawnPoint;
    [SerializeField] private GameObject arena1;
    [SerializeField] private Transform arena2SpawnPoint;
    [SerializeField] private GameObject arena2;

    private int currentArena;
    private void Awake()
    {
        if (I != null)
        {
            Debug.LogError("multiple singletons in scene");
        }
        I = this;

        arenas = new List<(Transform, GameObject)>
        {
            (arena1SpawnPoint, arena1),
            (arena2SpawnPoint, arena2)
        };
    }

    private void OnEnable()
    {
        Game.InitializeRun += Initialize;
    }
    private void OnDisable()
    {
        Game.InitializeRun -= Initialize;
    }
    private void Initialize() // triggered by event
    {
        foreach (var pair in arenas)
        {
            pair.Item2.SetActive(false);
        }

        currentArena = UnityEngine.Random.Range(0, arenas.Count);
        RandomlySwitchArenas();
    }

    public void RandomlySwitchArenas()
    {
        int newArena;
        do
        {
            newArena = UnityEngine.Random.Range(0, arenas.Count);
        } while (newArena == currentArena);

        SwitchToArena(newArena);
    }

    public void SwitchToArena(int newArena)
    {
        arenas[currentArena].Item2.SetActive(false);
        arenas[newArena].Item2.SetActive(true);
        Game.i.PlayerTransform.position = arenas[newArena].Item1.position;

        currentArena = newArena;
    }
}
