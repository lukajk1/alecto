using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private PlayerLookAndMove playerLookAndMove;

    float stepDistance = 180f; // units in between footsteps
    float distanceMoved;
    bool isMoving;

    Vector3 lastFootstepPos;

    private void Start()
    {
        playerLookAndMove = FindFirstObjectByType<PlayerLookAndMove>();
        lastFootstepPos = transform.position;
    }
    private void FixedUpdate()
    {
        isMoving = rb.linearVelocity.sqrMagnitude > 0.05f;

    }
    void Update()
    {
        //Debug.Log(distanceMoved);
        if (playerLookAndMove.IsGrounded && isMoving && !playerLookAndMove.IsCrouching)
        {
            distanceMoved += Vector3.Distance(transform.position, lastFootstepPos);
            if (distanceMoved >= stepDistance)
            {
                SFXManager.i.PlaySFXClip(PlayerSFXList.i.footstep, transform.position);

                lastFootstepPos = transform.position;
                distanceMoved = 0;
            }
        }
    }
}
