using UnityEngine;

public class Caster : EnemyUnit
{
    public override string Name => "Caster";
    public override int BaseMaxHealth => 525;
    public override float BaseMoveSpeed => 5f;
    public override int ScoreWeight => 115;
    public override int BaseDamage => 35;
    public override float AttackCDLength => 2.2f;
    public override float AttackRange => 150f;
    public override int WaveWeight => 15;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Die()
    {
        IsDead = true;
        LocalizedOnDeathEvent();
        CombatEventBus.BCOnEnemyDeath(this, transform.position);
        SFXManager.i.PlaySFXClip(UISFXList.i.enemyDeath, transform.position);
    }
}
