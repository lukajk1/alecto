using UnityEngine;

public class Bouncer : EnemyUnit
{
    public override string Name => "Bouncer";
    public override int BaseMaxHealth => 180;
    public override float BaseMoveSpeed => 5.2f;
    public override int ScoreWeight => 100;
    public override int BaseDamage => 45;
    public override float AttackCDLength => 3.0f;
    public override float AttackRange => 3f;
    public override int WaveWeight => 17;

    protected override void Awake()
    {
        base.Awake();
    }
}
