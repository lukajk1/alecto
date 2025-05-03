using UnityEngine;

[RequireComponent (typeof(Collider))]
public class ColliderMatlInfo : MonoBehaviour
{
    public enum MatType
    {
        Metallic,
        PowderBased,
        Flesh
    }
    public MatType _MatType;


}
