using UnityEngine;

public class PlayerPhysicsBus : MonoBehaviour
{
    public static PlayerPhysicsBus I;

    public bool IsGrounded 
    { 
        get 
        { 
            return playerLookAndMove.IsGrounded; 
        } 
    }

    private PlayerKnockback playerKnockback;
    private PlayerLookAndMove playerLookAndMove;

    private void Awake()
    {
        if (I != null) Debug.LogError("too many physics bus in scene");
        I = this;
    }
    private void Start()
    {
        playerKnockback = FindFirstObjectByType<PlayerKnockback>();
        playerLookAndMove = FindFirstObjectByType<PlayerLookAndMove>();
    }
    public void AddForceToPlayer(Vector3 force, ForceMode forceMode)
    {
        playerKnockback.AddForceToPlayer(force, forceMode);
    }


}
