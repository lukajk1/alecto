using UnityEngine;

[RequireComponent (typeof(PlayerLookAndMove))]
public class PlayerSprint : MonoBehaviour
{
    PlayerLookAndMove lookAndMove;
    float bonusSpeed = 4.5f;
    bool _isSprinting;
    bool IsSprinting
    {
        get => _isSprinting;
        set
        {
            if (_isSprinting != value)
            {
                _isSprinting = value;
                if (value)
                    lookAndMove.MoveSpeed += bonusSpeed;
                else 
                    lookAndMove.MoveSpeed -= bonusSpeed;
            }
        }
    }

    void Start()
    {
        lookAndMove = GetComponent<PlayerLookAndMove>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && lookAndMove.IsGrounded)
            IsSprinting = true;
        else
            IsSprinting = false;
    }
}
