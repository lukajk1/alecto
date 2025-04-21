using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public abstract string Name { get; }
    private int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value < 0)
            {
                _currentHealth = 0;
            }
            else if (value > CurrentMaxHealth)
            {
                return;
            }
            else
            {
                _currentHealth = value;
            }
        }
    }
    public abstract int BaseMaxHealth { get; }
    public int CurrentMaxHealth { get; protected set; }
    public abstract float BaseMoveSpeed { get; } // in units per s
    public float CurrentMoveSpeed { get; protected set; }
    public bool IsDead { get; protected set; }

    public event Action<bool, int> OnUnitDamaged;
    public event Action OnUnitReady;
    public event Action OnUnitDeath;
    protected virtual void Awake()
    {
        CurrentMaxHealth = BaseMaxHealth;
        CurrentHealth = CurrentMaxHealth;
        CurrentMoveSpeed = BaseMoveSpeed;
    }
    private void Start()
    {
        OnUnitReady?.Invoke();
    }

    public virtual void TakeDamage(bool isCrit, int damage)
    {
        if (IsDead) return;

        if (damage > 0)
        {
            if (damage >= CurrentHealth)
            {

                //Debug.Log("damage "+damage);
                //Debug.Log("currenthealth "+CurrentHealth);
                Die();
            }
            else
            {
                CurrentHealth -= damage;
                BCTakeDamage(isCrit, damage);
            }
        }
    }
    protected void BCTakeDamage(bool isCrit, int damage)
    {
        OnUnitDamaged?.Invoke(isCrit, damage);
    }
    public void KillViaCommand()
    {
        Die();
    }
    protected virtual void Die()
    {
        IsDead = true;
    }

    protected void LocalizedOnDeathEvent()
    {

        OnUnitDeath?.Invoke(); // for "local" processes that need to know about the unit dying
    }
}
