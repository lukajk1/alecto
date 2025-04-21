using UnityEngine;

public class RepositionSpawnpoint : MonoBehaviour
{
    public void Reposition()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
            //Debug.Log("hit");
        }
    }
}
