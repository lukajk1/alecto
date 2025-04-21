using UnityEngine;

public class Diver : EnemyUnit
{
    public override string Name => "Diver";
    public override int BaseMaxHealth => 450;
    public override float BaseMoveSpeed => 5f;
    public override int ScoreWeight => 100;
    public override int BaseDamage => 45;
    public override float AttackCDLength => 4f;
    public override float AttackRange => 17f;
    public override int WaveWeight => 18;

    protected override void Awake()
    {
        base.Awake();
    }
}
