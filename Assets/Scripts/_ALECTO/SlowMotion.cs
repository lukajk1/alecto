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
    private float contrastChange = 34f;

    float sloMoScale = 0.4f;

    private bool _sloMo;
    private bool SloMo
    {
        get => _sloMo;
        set
        {
            if (_sloMo != value)
            {
                _sloMo = value;
                ToggleScreenEffects(value);
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

        if (Input.GetKey(KeyCode.E)) // held down as opposed to distinct button press
        {
            Game.TimeScale = sloMoScale;
            SloMo = true;
        }
        else
        {
            Game.TimeScale = 1f;
            SloMo = false;
        }
    }
    void ToggleScreenEffects(bool value)
    {
        if (value)
        {
            colorAdjustments.saturation.value = saturationChange;
            lensDistortion.intensity.value = lensDistortChange;
            chromaticAberration.intensity.value = chromaticAberrationChange;
            colorAdjustments.contrast.value = contrastChange;
        }
        else
        {
            colorAdjustments.saturation.value = 0f;
            lensDistortion.intensity.value = 0f;
            chromaticAberration.intensity.value = 0f;
            colorAdjustments.contrast.value = 0f;
        }
    }
}
