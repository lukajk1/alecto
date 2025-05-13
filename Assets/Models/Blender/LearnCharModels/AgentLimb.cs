using UnityEngine;

public class AgentLimb : MonoBehaviour
{
    Unit agent;
    public void Initialize(Unit agent)
    {
        this.agent = agent;
    }

    //public void Hit(DamageData dmgData)
    //{
    //    agent.TakeDamage(dmgData);
    //}    
    public void Hit(float damage)
    {
        agent.TakeDamage(false, (int)damage);
    }
}
