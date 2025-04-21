using UnityEngine;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.InputSystem.HID;

public class ParticleEffectsManager : MonoBehaviour
{
    [SerializeField] private Camera fpCamera;
    [SerializeField] private ParticleSystem bloodSplatter;
    private Queue<ParticleSystem> pool = new Queue<ParticleSystem>();

    [SerializeField] private Canvas damageNumberCanvas;

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            var instance = Instantiate(bloodSplatter, transform);
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }
    }

    private void OnEnable()
    {
        CombatEventBus.OnEnemyHit += OnEnemyHit;
    }

    private void OnDisable()
    {
        CombatEventBus.OnEnemyHit -= OnEnemyHit;
    }

    private void OnEnemyHit(int damage, bool isCrit, Vector3 pos)
    {
        BloodSplatter(pos);

        if (Vector3.Distance(Game.i.PlayerTransform.position, pos) < 20f)
        {
            ShowDamageNumbers(damage, isCrit, Vector3.Lerp(fpCamera.transform.position, pos + new Vector3(0, 1.4f, 0), 0.8f));
        }
    }

    private void ShowDamageNumbers(int damage, bool isCrit, Vector3 pos)
    {
        Canvas obj = Instantiate(damageNumberCanvas, pos, Quaternion.identity);
        obj.GetComponent<DamageNumbers>().Activate(damage, isCrit);
    }

    private void BloodSplatter(Vector3 pos)
    {
        if (pool.Count == 0) return;

        var ps = pool.Dequeue();
        ps.transform.position = pos;
        ps.gameObject.SetActive(true);
        ps.Play();

        StartCoroutine(ReturnToPoolAfterPlay(ps));
    }

    private System.Collections.IEnumerator ReturnToPoolAfterPlay(ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.main.duration);
        ps.gameObject.SetActive(false);
        pool.Enqueue(ps);
    }

}
