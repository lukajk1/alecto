using UnityEngine;

[RequireComponent (typeof(PlayerLookAndMove))]
public class PlayerSprint : MonoBehaviour
{
    PlayerLookAndMove lookAndMove;
    Camera mainCam;
    float bonusSpeed = 3.9f;
    float bonusFOV = 4.5f;
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
                {
                    lookAndMove.MoveSpeed += bonusSpeed;
                    mainCam.fieldOfView += bonusFOV;
                }
                else
                {
                    lookAndMove.MoveSpeed -= bonusSpeed;
                    mainCam.fieldOfView -= bonusFOV;
                }

            }
        }
    }

    void Start()
    {
        lookAndMove = GetComponent<PlayerLookAndMove>();
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            IsSprinting = true;
        else
            IsSprinting = false;
    }
}
