using UnityEngine;

public class HealthbarPlayer : Healthbar
{
    new private void OnEnable()
    {
        base.OnEnable();
        CombatEventBus.OnPlayerHeal += Heal;
    }
    new private void OnDisable()
    {
        base.OnDisable();
        CombatEventBus.OnPlayerHeal -= Heal;
    }

    private void Heal()
    {
        healthbar.fillAmount = (float)unitTarget.CurrentHealth / unitTarget.CurrentMaxHealth;
        whiteHealth.fillAmount = (float)unitTarget.CurrentHealth / unitTarget.CurrentMaxHealth;
    }
}
