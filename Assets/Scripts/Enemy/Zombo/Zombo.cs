using UnityEngine;

public class Zombo : EnemyUnit
{
    public override string Name => "Zombo";
    public override int BaseMaxHealth => 450;
    public override float BaseMoveSpeed => 5f;
    public override int ScoreWeight => 100;
    public override int BaseDamage => 17;
    public override float AttackCDLength => 1.5f;
    public override float AttackRange => 1.5f;
    public override int WaveWeight => 10;
    protected override void Awake()
    {
        base.Awake();
        AttackReady = true;
    }
}
