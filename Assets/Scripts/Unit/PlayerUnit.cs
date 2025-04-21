using System.Collections;
using UnityEngine;

public class PlayerUnit : Unit
{
    public override string Name => "Player";
    public override int BaseMaxHealth => 330;
    public override float BaseMoveSpeed => 6.8f;

    public int DebugDamageModifier = 1;

    //private float hitstopEaseDuration = 0.15f;
    //private float hitstopDuration = 0.3f;

    private float LifestealBuffer = 0;
    private float LifestealRatio = 0.002f;
    protected override void Die()
    {
        IsDead = true;
        MainEventBus.BCOnRunEnd();
    }
    public override void TakeDamage(bool isCrit, int damage)
    {
        if (IsDead) return;

        if (damage > 0)
        {
            CurrentHealth -= damage * DebugDamageModifier;
            BCTakeDamage(isCrit, damage);
            CombatEventBus.BCOnPlayerHit(damage, isCrit);

            SFXManager.i.PlaySFXClip(UISFXList.i.enemyBodyHit, Game.i.PlayerTransform.position, false); 
            if (damage >= CurrentHealth) Die();
            else
            {
                //StartCoroutine(Hitstop());
            }

        }
    }

    public void Lifesteal(int damageDealtToEnemy)
    {
        LifestealBuffer += damageDealtToEnemy * LifestealRatio;
        CurrentHealth += (int)LifestealBuffer;
        LifestealBuffer -= (int)LifestealBuffer;

        CombatEventBus.BCOnPlayerHeal();
    }

    //private IEnumerator Hitstop() 
    //{
    //    Game.TimeScale = 0.3f;
    //    yield return new WaitForSeconds(hitstopDuration);
    //    Game.TimeScale = 1f;
    //}
}
