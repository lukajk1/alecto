using UnityEngine;

public class EnvParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainParticles;
    private ParticleSystem[] envParticles;
    private void Awake()
    {
        envParticles = new ParticleSystem[] 
        { 
            rainParticles
        };

    }
    private void Start()
    {
        Game.PauseUpdated += SetPaused;
    }
    private void OnDestroy()
    {
        Game.PauseUpdated -= SetPaused;
    }
    private void SetPaused(bool value)
    {
        if (value)
        {
            foreach (ParticleSystem p in envParticles)
            {
                p.Pause();
            }
        }
        else
        {
            foreach (ParticleSystem p in envParticles)
            {
                p.Play();
            }
        }

    }
}
