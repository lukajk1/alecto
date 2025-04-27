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
    [SerializeField] private bool applyBuoyancy;

    private float GravityConstant;
    private bool submerged;

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

        if (submerged)
        {
            // buoyancy = fluid density * g * fluid volume displaced by object 

            rb.AddForce(Vector3.up * GravityConstant * 1.3f * rb.mass, ForceMode.Force);
            rb.angularDamping = 0.6f;
            rb.linearDamping = 0.6f;
        }
        else
        {
            rb.angularDamping = 0.05f;
            rb.linearDamping = 0f;
        }
    }
    void OnCollisionEnter(Collision collision)
    {

        if (rb.linearVelocity.magnitude > 3.5f) SFXManager.i.PlaySFXClip(PlayerSFXList.i.jumpLanding, transform.position, type: SFXManager.SoundType.HasFalloff);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Fluid"))
        {
            submerged = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        submerged = false;
    }

}
