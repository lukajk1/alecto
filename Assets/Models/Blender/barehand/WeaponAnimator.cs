using UnityEngine;

public abstract class WeaponAnimator : MonoBehaviour 
{
    protected Animator animator;
    public virtual void SwitchIn()
    {
        if (animator != null) animator.SetTrigger("switchIn");
    }
    public virtual void SwitchOut()
    {
        if (animator != null) animator.SetTrigger("switchOut");
    }
}
