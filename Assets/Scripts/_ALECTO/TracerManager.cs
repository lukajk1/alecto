using System.Collections.Generic;
using UnityEngine;

public class TracerManager : MonoBehaviour
{
    public static TracerManager i;

    [SerializeField] Tracer tracerPrefab;
    [SerializeField] int poolSize = 20;

    Queue<Tracer> pool = new Queue<Tracer>();

    void Start()
    {
        if (i != null) Debug.LogError($"More than one TracerManager in scene");
        i = this;

        for (int j = 0; j < poolSize; j++)
        {
            Tracer tracer = Instantiate(tracerPrefab, Vector3.zero, Quaternion.identity);
            pool.Enqueue(tracer);
        }
    }

    public void FireTracer(Vector3 target)
    {
        Tracer tracer = pool.Dequeue();

        tracer.gameObject.SetActive(true);
        tracer.Shoot(target);
        pool.Enqueue(tracer);
    }
}
