using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatUI : MonoBehaviour
{
    public static CombatUI Instance { get; private set; }

    [SerializeField] private RawImage hitmarker;

    private Coroutine hitmarkerCR;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("multiple singletons in scene");
        }
        Instance = this;
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
        ShowHitMarker(isCrit);
    }

    private void ShowHitMarker(bool isCrit)
    {
        if (isCrit) hitmarker.color = new Color(1f, 0, 0, 0.63f);
        else hitmarker.color = new Color(1f, 1f, 1f, 0.63f);

        if (hitmarkerCR != null)
        {
            StopCoroutine(hitmarkerCR);
        }
        hitmarkerCR = StartCoroutine(HitmarkerRoutine());
    }

    IEnumerator HitmarkerRoutine()
    {
        hitmarker.enabled = true;
        yield return new WaitForSeconds(0.3f);
        hitmarker.enabled = false;
    }
}
