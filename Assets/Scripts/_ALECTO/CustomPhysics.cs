using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CustomPhysics : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private bool normalGravity;
    [SerializeField] private bool invertedVerticalGravity;
    [SerializeField] private bool horizontalGravity;
    [SerializeField] private bool useBlackHoleAttraction;
    [SerializeField] private float customGravityScalar = 0f;
    //[SerializeField] private float approximateVolumeInCubicMeters = 1f;
    [SerializeField] private float mass = 10f;

    private float GravityConstant;
    public event Action<bool> OnSubmergedChanged;
    private bool _submerged;
    private bool Submerged
    {
        get => _submerged;
        set
        {
            if (_submerged != value)
            {
                _submerged = value;
                OnSubmergedChanged?.Invoke(value);
            }
        }
    }

    private GameObject blackHole;

    private bool shot;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GravityConstant = Game.GravityConstant; 
        blackHole = GameObject.FindGameObjectWithTag("BlackHole");
        rb.mass = mass;
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

        if (Submerged)
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
            Submerged = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Fluid"))
        {
            Submerged = false;
        }
    }

}
