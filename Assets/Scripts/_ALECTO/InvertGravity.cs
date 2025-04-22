using UnityEngine;
using UnityEngine.Audio;

public class InvertGravity : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private bool normalGravity;
    [SerializeField] private bool invertedVerticalGravity;
    [SerializeField] private bool horizontalGravity;
    [SerializeField] private bool useBlackHoleAttraction;
    [SerializeField] private float customGravityScalar = 0f;

    private float GravityConstant;

    private GameObject blackHole;

    private bool shot;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GravityConstant = Game.GravityConstant; 
        blackHole = GameObject.FindGameObjectWithTag("BlackHole");
    }
    void FixedUpdate()
    {
        if (invertedVerticalGravity) rb.AddForce(new Vector3(0f, GravityConstant, 0f), ForceMode.Acceleration);
        if (normalGravity) rb.AddForce(new Vector3(0f, -GravityConstant, 0f), ForceMode.Acceleration);
        if (horizontalGravity) rb.AddForce(new Vector3(0f, 0f, -GravityConstant), ForceMode.Acceleration);

        if (useBlackHoleAttraction)
        {
            Vector3 normalizedRelativeDirectionVector = (blackHole.transform.position - transform.position).normalized;
            rb.AddForce(normalizedRelativeDirectionVector * GravityConstant, ForceMode.Acceleration);
        }

        if (customGravityScalar > 0f)
        {
            rb.AddForce(new Vector3(0f, -GravityConstant * customGravityScalar, 0f), ForceMode.Acceleration);
        }
    }
    void OnCollisionEnter(Collision collision)
    {

        if (rb.linearVelocity.magnitude > 3.5f) SFXManager.i.PlaySFXClip(PlayerSFXList.i.jumpLanding, transform.position);
    }

}
