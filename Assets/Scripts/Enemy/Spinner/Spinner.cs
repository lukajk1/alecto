using UnityEngine;

public class Spinner : EnemyUnit
{
    public override string Name => "Serafim";
    public override int BaseMaxHealth => 1000;
    public override float BaseMoveSpeed => 3f;
    public override int ScoreWeight => 350;
    public override int BaseDamage => 45;
    public override float AttackCDLength => 10f;
    public override float AttackRange => 25f;
    public override int WaveWeight => 27;

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
