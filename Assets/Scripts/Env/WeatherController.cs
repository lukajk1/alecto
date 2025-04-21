using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WeatherController : MonoBehaviour
{
    public enum Rain 
    { 
        Heavy, 
        Light,
        None
    }

    [SerializeField] private ParticleSystem particleSystemRain;
    [SerializeField] private AudioSource rainSound;
    [SerializeField] private AudioSource windSound;
    [SerializeField] private WindZone wind;

    private const string fogColorBlue = "4fb3ff";
    private const string fogColorOvercast = "E9E9E9";
    private const string fogColorDawn = "E08F82";
    private const string fogColorRainy = "B4C1E7";

    private const string lightRainColor = "aeaeae";
    private const string heavyRainColor = "DCE9FF";

    private const string normalAmbientColor = "444349";  
    //private const string normalAmbientColor = "38373d";  
    private const string blueAmbientColor = "222c57";  
    private const string dawnAmbientColor = "222c57";  

    private const float lightFogDensity = 0.002f;
    private const float moderateFogDensity = 0.0045f;
    private const float heavyFogDensity = 0.0050f;

    public static WeatherController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("multiple weather controllers in scene");
        }
        Instance = this;
    }

    private void Start()
    {
        SetHeavyRain();
    }
    public bool SetWeatherFromString(string weather)
    {
        switch (weather)
        {
            case "blue":
                SetBlue();
                return true;
            case "overcast":
                SetOvercast();
                return true;
            case "dawn":
                SetDawn();
                return true;
            case "heavyrain":
                SetHeavyRain();
                return true;
            case "lightrain":
                SetLightRain();
                return true;
            case "foggy":
                SetFoggy();
                return true;
            default:
                return false;
        }
    }

    private void SetBlue()
    {
        SetEnvLighting(lightFogDensity, fogColorBlue, blueAmbientColor);
        SetRain(Rain.None);
    }
    private void SetDawn()
    {
        SetEnvLighting(moderateFogDensity, fogColorDawn, dawnAmbientColor);
        SetRain(Rain.None);
    }
    private void SetOvercast()
    {
        SetEnvLighting(lightFogDensity, fogColorOvercast);
        SetRain(Rain.None);
    }
    private void SetFoggy()
    {
        SetEnvLighting(moderateFogDensity, fogColorRainy);
        SetRain(Rain.None);
    }
    private void SetHeavyRain()
    {
        SetEnvLighting(moderateFogDensity, fogColorRainy);
        SetRain(Rain.Heavy);
    }
    private void SetLightRain()
    {
        SetEnvLighting(moderateFogDensity, fogColorRainy);
        SetRain(Rain.Light);
    }

    private void SetEnvLighting(float fogDensity, string fogColor, string ambientLightColor = normalAmbientColor)
    {
        RenderSettings.fogDensity = fogDensity; 
        RenderSettings.fogColor = HexToColor(fogColor);
        RenderSettings.ambientLight = HexToColor(ambientLightColor);
    }   

    private void SetRain(Rain rainType)
    {
        switch (rainType) 
        {
            case Rain.Heavy:
                var emission = particleSystemRain.emission;
                emission.rateOverTime = 700f;
                var main = particleSystemRain.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(31.4f, 42f);
                main.startColor = HexToColor(heavyRainColor);

                wind.windMain = 3f;
                particleSystemRain.gameObject.SetActive(true);
                rainSound.Play();
                windSound.Play();
                break;

            case Rain.Light:
                emission = particleSystemRain.emission;
                emission.rateOverTime = 150f;
                main = particleSystemRain.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(24f, 34f);
                main.startColor = HexToColor(lightRainColor);

                wind.windMain = 0f;
                particleSystemRain.gameObject.SetActive(true);
                rainSound.Play();
                windSound.Pause();
                break;

            case Rain.None:
                wind.windMain = 0f;
                particleSystemRain.gameObject.SetActive(false);
                rainSound.Pause();
                windSound.Play();
                break;
        }
    }

    public static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length != 6 && hex.Length != 8)
            throw new System.ArgumentException("Hex code must be 6 or 8 characters long.");

        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = hex.Length == 8 ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

        return new Color32(r, g, b, a);
    }
}
