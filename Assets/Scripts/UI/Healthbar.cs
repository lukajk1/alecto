using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] protected Unit unitTarget;
    [SerializeField] protected Image healthbar;
    [SerializeField] protected Image whiteHealth;

    protected int currentHealth = 0;
    protected Coroutine lerpWhiteHealth;
    protected float lerpDuration = 0.60f;
    protected void OnEnable()
    {
        if (unitTarget != null)
        {
            unitTarget.OnUnitDamaged += UpdateBar;
            unitTarget.OnUnitReady += Setup;
            unitTarget.OnUnitDeath += OnDeath;
        }
    }

    protected void OnDisable()
    {
        if (unitTarget != null)
        {
            unitTarget.OnUnitDamaged -= UpdateBar;
            unitTarget.OnUnitReady -= Setup;
            unitTarget.OnUnitDeath -= OnDeath;
        }

    }

    protected void Setup()
    {
        currentHealth = unitTarget.BaseMaxHealth; 
        healthbar.fillAmount = 1f;
        whiteHealth.fillAmount = 1f;
    }

    protected void OnDeath()
    {
        gameObject.SetActive(false);
    }

    protected void UpdateBar(bool isCrit, int damage)
    {
        currentHealth -= damage;
        healthbar.fillAmount = (float)currentHealth / unitTarget.BaseMaxHealth; // cast to float to avoid int division

        if (lerpWhiteHealth == null)
        {
            lerpWhiteHealth = StartCoroutine(LerpWhiteHealth());
        }
        else
        {
            StopCoroutine(lerpWhiteHealth);
            lerpWhiteHealth = StartCoroutine(LerpWhiteHealth());
        }
    }

    protected IEnumerator LerpWhiteHealth()
    {
        float elapsed = 0;
        while (elapsed < lerpDuration) 
        {
            elapsed += Time.deltaTime;
            whiteHealth.fillAmount = Mathf.Lerp(whiteHealth.fillAmount, healthbar.fillAmount, elapsed);
            yield return null;
        }
        lerpWhiteHealth = null;
    }
}
