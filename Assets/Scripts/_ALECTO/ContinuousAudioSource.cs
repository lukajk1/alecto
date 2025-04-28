using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioLowPassFilter))]
public class ContinuousAudioSource : MonoBehaviour
{
    AudioLowPassFilter lowPassFilter;
    [SerializeField] private CustomPhysics invertGrav;
    private void OnEnable()
    {
        invertGrav.OnSubmergedChanged += SetLowPass;
    }

    private void OnDisable()
    {
        invertGrav.OnSubmergedChanged -= SetLowPass;
    }

    void Start()
    {
        lowPassFilter = GetComponent<AudioLowPassFilter>();
        lowPassFilter.enabled = false;
    }

    void SetLowPass(bool value)
    {
        if (value)
        {
            lowPassFilter.enabled = true;
        }
        else
        {
            lowPassFilter.enabled = false;
        }
    }
}
