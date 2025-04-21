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
        _3D
    }

    public void PlaySFXClip(AudioClip clip, Vector3 positionToPlaySound, bool varyPitch = true)
    {
        AudioSource audioSource = Instantiate(soundFXObject, positionToPlaySound, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.Play();
        
        if (varyPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.pitch *= Game.TimeScale; // scale pitch with timescale

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }    
    
    public void PlaySFXClip(SoundType type, AudioClip clip, Vector3 positionToPlaySound, bool varyPitch = true)
    {
        AudioSource audioSource = Instantiate(soundFXObject, positionToPlaySound, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.Play();
        audioSource.spatialBlend = 1;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 20f;
        
        if (varyPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }


}
