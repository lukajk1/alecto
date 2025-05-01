using UnityEngine;
using System.Collections.Generic;

public class DecalManager : MonoBehaviour
{
    public static DecalManager i { get; private set; }

    [SerializeField] GameObject bulletDecal;
    [SerializeField] GameObject explosionDecal;

    private Dictionary<Decal, int> poolSizes = new Dictionary<Decal, int>
    {
        { Decal.BulletHole, 500 },
        { Decal.Explosion, 10 }
    };

    private Dictionary<Decal, Queue<GameObject>> decalPools = new Dictionary<Decal, Queue<GameObject>>();

    private void Awake()
    {
        if (i != null) Debug.LogError($"More than one instance of {i} in scene");
        i = this;

        foreach (var kvp in poolSizes)
        {
            decalPools[kvp.Key] = new Queue<GameObject>();
        }
    }

    public enum Decal
    {
        Explosion,
        BulletHole
    }

    public void PlaceDecal(Decal type, Vector3 position, Vector3 normalOrientation, Transform hitTransform)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normalOrientation)
                              * Quaternion.Euler(90f, 0f, 0f)
                              * Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

        GameObject decal = GetPooledDecal(type);
        decal.transform.SetPositionAndRotation(position, rotation);
        decal.transform.parent = hitTransform;
        decal.SetActive(true);

    }

    GameObject LookupDecal(Decal type)
    {
        switch (type)
        {
            case Decal.BulletHole: return bulletDecal;
            case Decal.Explosion: return explosionDecal;
            default: return explosionDecal;
        }
    }

    GameObject GetPooledDecal(Decal type)
    {
        Queue<GameObject> pool = decalPools[type];

        if (pool.Count < poolSizes[type])
        {
            GameObject newDecal = Instantiate(LookupDecal(type));
            pool.Enqueue(newDecal);
            return newDecal;
        }

        GameObject recycled = pool.Dequeue();
        pool.Enqueue(recycled);
        return recycled;
    }
}
