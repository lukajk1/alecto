using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class ShootBlackHole : MonoBehaviour
{
    [SerializeField] private GameObject blackHole;
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;

    private float saturationChange = -30f;
    private float _saturation;
    private float Saturation
    {
        get => _saturation;
        set
        {
            if (_saturation != value)
            {
                _saturation = value;
                colorAdjustments.saturation.value = value;
            }
        }
    }

    private void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject projectile = Instantiate(blackHole, transform.position + transform.forward, Quaternion.identity);
            projectile.GetComponent<BlackHoleProjectile>().Initialize(transform.forward);
        }

        if (Input.GetMouseButton(1)) // held down as opposed to distinct button press
        {
            Game.TimeScale = 0.5f;
            Saturation = saturationChange;
        }
        else
        {
            Game.TimeScale = 1f;
            Saturation = 0f;
        }

    }
}
