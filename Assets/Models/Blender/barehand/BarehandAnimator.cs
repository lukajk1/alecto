using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarehandAnimator : WeaponAnimator
{
    [SerializeField] private PlayerInput playerInput;

    private bool _isFiring;
    private bool IsFiring
    {
        get => _isFiring; 
        set
        {
            if (_isFiring != value) 
            {
                _isFiring = value;
                animator.SetBool("isAttacking", _isFiring);
            }
        }
    }
    private InputAction attackAction;

    private void OnEnable()
    {
        attackAction = playerInput.actions["Attack"];
        attackAction.Enable();
    }
    private void OnDisable()
    {
        attackAction.Disable();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        IsFiring = attackAction.IsPressed();
    }
}
