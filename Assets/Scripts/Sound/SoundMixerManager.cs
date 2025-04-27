using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer; 
    public static SoundMixerManager i;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Game.OnTimeScaleChanged += ProcessTimeScale;
    }

    private void OnDisable()
    {
        Game.OnTimeScaleChanged -= ProcessTimeScale;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20f);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    private void ProcessTimeScale(float value)
    {
        if (value < 1f)
        {
            SetEcho(true);
        }
        else
        {
            SetEcho(false);
        }
    }

    public void SetEcho(bool active)
    {
        audioMixer.SetFloat("echoWetMix", active? 1f : 0f);
        audioMixer.SetFloat("echoPitch", active? 0.5f : 1f);
    }
}
