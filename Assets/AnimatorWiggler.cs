using UnityEngine;

[RequireComponent (typeof(Animator))]
public class AnimatorWiggler : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("Transition");
        }
    }
}
