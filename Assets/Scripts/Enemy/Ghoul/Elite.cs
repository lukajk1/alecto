using UnityEngine;

public class Elite : EnemyUnit
{
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] SkinnedMeshRenderer rend;
    [SerializeField] Material oCamo;
    [SerializeField] Material eliteUV;

    bool invisible = true;
    public override string Name => "Elite";
    public override int BaseMaxHealth => 150;
    public override float BaseMoveSpeed => 4.5f;
    public override int ScoreWeight => 150;
    public override int BaseDamage => 30;
    public override float AttackCDLength => 1.0f;
    public override float AttackRange => 15f;
    public override int WaveWeight => 22;
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
        ragdoll.Enable();
    }

    public override void TakeDamage(bool isCrit, int damage)
    {
        base.TakeDamage(isCrit, damage);

        if (invisible)
        {
            rend.material = eliteUV;
            invisible = false;
        }
    }
}
