using UnityEngine;

[RequireComponent (typeof(MeshRenderer))]
public class HideMeshOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
