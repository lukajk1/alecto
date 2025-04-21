using UnityEngine;

public class SettingsData
{
    private int _fov;
    private int _volMaster;
    private int _volSFX;
    private int _volMusic;
    private float _mouseSensitivity;
    private int _vSync;

    public int FOV
    {
        get => _fov;
        set => _fov = Mathf.Clamp(value, 10, 110);
    }

    public int VolMaster
    {
        get => _volMaster;
        set => _volMaster = Mathf.Clamp(value, 0, 100);
    }

    public int VolSFX
    {
        get => _volSFX;
        set => _volSFX = Mathf.Clamp(value, 0, 100);
    }

    public int VolMusic
    {
        get => _volMusic;
        set => _volMusic = Mathf.Clamp(value, 0, 100);
    }

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set => _mouseSensitivity = value; // Add clamping if desired
    }

    public int VSync
    {
        get => _vSync;
        set => _vSync = Mathf.Clamp(value, 0, 1);
    }

    private int defaultFOV = 95;
    private int defaultVolMaster = 72;
    private int defaultVolSFX = 100;
    private int defaultVolMusic = 100;
    private float defaultSensitivity = 45f;
    private int defaultVSync = 1;

    public SettingsData()
    {
    }

    public void PopulateDataFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("FOV")) { FOV = PlayerPrefs.GetInt("FOV"); }
        else { FOV = defaultFOV; }

        if (PlayerPrefs.HasKey("VolMaster")) { VolMaster = PlayerPrefs.GetInt("VolMaster"); //Debug.Log("found key" + VolMaster);
                                                                                            }
        else { VolMaster = defaultVolMaster; 
                //Debug.Log("couldn't find master key");
                }

        if (PlayerPrefs.HasKey("VolSFX")) { VolSFX = PlayerPrefs.GetInt("VolSFX"); }
        else { VolSFX = defaultVolSFX; }

        if (PlayerPrefs.HasKey("VolMusic")) { VolMusic = PlayerPrefs.GetInt("VolMusic"); }
        else { VolMusic = defaultVolMusic; }

        if (PlayerPrefs.HasKey("MouseSensitivity")) { MouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity"); }
        else { MouseSensitivity = defaultSensitivity; }

        if (PlayerPrefs.HasKey("VSync")) { VSync = PlayerPrefs.GetInt("VSync"); }
        else { VSync = defaultVSync; }

    }
    public void Save()
    {
        PlayerPrefs.SetInt("FOV", FOV);
        PlayerPrefs.SetInt("VolMaster", VolMaster);
        PlayerPrefs.SetInt("VolSFX", VolSFX);
        PlayerPrefs.SetInt("VolMusic", VolMusic);
        PlayerPrefs.SetFloat("MouseSensitivity", MouseSensitivity);
        PlayerPrefs.SetInt("VSync", VSync);

        PlayerPrefs.Save();
    }


}
