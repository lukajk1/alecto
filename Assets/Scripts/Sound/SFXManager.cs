using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource soundFXObject;
    public static SFXManager i;

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

    public enum SoundType
    {
        NoDirectionOrFalloff,
        HasFalloff
    }

    public void PlaySFXClip(AudioClip clip, Vector3 positionToPlaySound, bool varyPitch = true, SoundType type = SoundType.NoDirectionOrFalloff, float minDist = 0f, float maxDist = 0f)
    {
        AudioSource audioSource = Instantiate(soundFXObject, positionToPlaySound, Quaternion.identity);
        audioSource.clip = clip;

        if (varyPitch)
            audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.pitch *= Game.TimeScale;

        if (type == SoundType.HasFalloff)
        {
            audioSource.spatialBlend = 1;
            audioSource.minDistance = minDist;
            audioSource.maxDistance = maxDist;
        }

        float clipDuration = clip.length / audioSource.pitch;
        float extraEchoDuration = 0f;

        if (Game.TimeScale < 1f)
        {
            AudioEchoFilter echo = audioSource.gameObject.AddComponent<AudioEchoFilter>();
            echo.delay = 300f;
            echo.decayRatio = 0.4f;
            echo.wetMix = 1f;
            echo.dryMix = 1f;

            // Estimate time for echo to decay
            float decayThreshold = 0.01f;
            int echoCount = Mathf.CeilToInt(Mathf.Log(decayThreshold) / Mathf.Log(echo.decayRatio));
            extraEchoDuration = echoCount * (echo.delay / 1000f);
        }

        audioSource.Play();
        Destroy(audioSource.gameObject, clipDuration + extraEchoDuration);
    }



}
