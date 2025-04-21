using UnityEngine;
public class PreserveTextureScale : MonoBehaviour
{
    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();

        float tileX = transform.localScale.x;
        float tileY = transform.localScale.y;

        rend.material.mainTextureScale = new Vector2(tileX, tileY);
    }
}
