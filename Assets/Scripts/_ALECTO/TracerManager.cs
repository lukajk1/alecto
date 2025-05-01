using UnityEngine;

public class TracerManager : MonoBehaviour
{
    public static TracerManager i;

    [SerializeField] GameObject tracer;
    void Awake()
    {
        if (i != null) Debug.LogError($"More than one instance of {i} in scene");
        i = this;
    }

}
