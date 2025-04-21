using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageNumbers : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageTxt;

    private Color startingColor;

    private float height = 30f;

    private Camera playerCamera;
    private Transform playerTransform;

    private float fadeDuration = 0.8f;
    private float activationDistance = 99f;
    private Canvas canvas;
    private bool isVisible = false;

    private Color critColor = new Color(1f, 0.3f, 0.3f);
    private Color normalColor = Color.white;

    private void Setup()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.enabled = false;
        }

        playerTransform = Game.i.PlayerTransform;
        playerCamera = Game.i.PlayerCamera;
    }

    private void Update()
    {
        if (playerCamera != null)
        {
            transform.LookAt(transform.position + playerCamera.transform.rotation * Vector3.forward,
                             playerCamera.transform.rotation * Vector3.up);
        }

        if (playerTransform != null)
        {
            // Calculate squared distance to avoid unnecessary sqrt operation
            float sqrDistance = (transform.position - playerTransform.position).sqrMagnitude;
            float sqrActivationDistance = activationDistance * activationDistance;

            if (sqrDistance <= sqrActivationDistance && !isVisible)
            {
                // Enable canvas if within range
                SetVisibility(true);
            }
            else if (sqrDistance > sqrActivationDistance && isVisible)
            {
                // Disable canvas if out of range
                SetVisibility(false);
            }
        }
    }

    public void Activate(int damage, bool isCrit)
    {
        Setup();
        damageTxt.text = damage.ToString();

        if (isCrit) damageTxt.color = critColor;
        else damageTxt.color = normalColor;
        startingColor = damageTxt.color;
        StartCoroutine(DamageTextBounceAndFadeOut());
    }

    private IEnumerator DamageTextBounceAndFadeOut()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 startPosition = rectTransform.anchoredPosition += new Vector2(0, 0f);
        float elapsedTime = 0f;
        float xOffset = Random.Range(-1, 1f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            damageTxt.color = new Color(startingColor.r, startingColor.g, startingColor.b, alpha);

            float x = Mathf.Lerp(startPosition.x, startPosition.x + xOffset, elapsedTime / fadeDuration); // Horizontal movement (adjust as needed)
            float y = startPosition.y + height * (4 * elapsedTime / fadeDuration * (1 - elapsedTime / fadeDuration)); // Parabolic movement

            damageTxt.rectTransform.anchoredPosition = new Vector2(x, y);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void SetVisibility(bool visible)
    {
        isVisible = visible;
        if (canvas != null)
        {
            canvas.enabled = visible;
        }
    }
}
