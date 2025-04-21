using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class SlowMotion : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;
    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    private float saturationChange = -50f;
    private float lensDistortChange = -0.4f;
    private float chromaticAberrationChange = 0.7f;
    private bool _sloMo;
    private bool SloMo
    {
        get => _sloMo;
        set
        {
            if (_sloMo != value)
            {
                _sloMo = value;
                if (value)
                {
                    colorAdjustments.saturation.value = saturationChange;
                    lensDistortion.intensity.value = lensDistortChange;
                    chromaticAberration.intensity.value = chromaticAberrationChange;
                }
                else
                {
                    colorAdjustments.saturation.value = 0f;
                    lensDistortion.intensity.value = 0f;
                    chromaticAberration.intensity.value = 0f;
                }
            }
        }
    }

    private void Start()
    {
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out chromaticAberration);
    }
    private void Update()
    {

        if (Input.GetMouseButton(1)) // held down as opposed to distinct button press
        {
            Game.TimeScale = 0.5f;
            SloMo = true;
        }
        else
        {
            Game.TimeScale = 1f;
            SloMo = false;
        }
    }
}
