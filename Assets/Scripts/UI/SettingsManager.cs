using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Canvas settingsPage;
    [SerializeField] private Button confirm;

    [SerializeField] private Slider sliderFOV;
    [SerializeField] private Slider sliderVolMaster;
    [SerializeField] private Slider sliderVolSFX;
    [SerializeField] private Slider sliderVolMusic;
    [SerializeField] private Slider sliderSensitivity;

    [SerializeField] private TMP_InputField fieldFOV;
    [SerializeField] private TMP_InputField fieldVolMaster;
    [SerializeField] private TMP_InputField fieldVolSFX;
    [SerializeField] private TMP_InputField fieldVolMusic;
    [SerializeField] private TMP_InputField fieldSensitivity;

    [SerializeField] private Toggle toggleVSync;

    public static SettingsManager i;

    public SettingsData currentSettings;

    private bool suppressCallback = false; 
    public enum InputType
    {
        Slider,
        Field
    }
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

    private void Start()
    {
        settingsPage.gameObject.SetActive(false);

        confirm.onClick.AddListener(SaveAndClose);

        sliderFOV.onValueChanged.AddListener(value => SetAndSyncValue(InputType.Slider, value, sliderFOV, fieldFOV));
        fieldFOV.onEndEdit.AddListener(_ => SetAndSyncValue(InputType.Field, 0f, sliderFOV, fieldFOV)); // ignore 'value' for field

        sliderVolMaster.onValueChanged.AddListener(value => SetAndSyncValue(InputType.Slider, value, sliderVolMaster, fieldVolMaster));
        fieldVolMaster.onEndEdit.AddListener(_ => SetAndSyncValue(InputType.Field, 0f, sliderVolMaster, fieldVolMaster));

        sliderVolSFX.onValueChanged.AddListener(value => SetAndSyncValue(InputType.Slider, value, sliderVolSFX, fieldVolSFX));
        fieldVolSFX.onEndEdit.AddListener(_ => SetAndSyncValue(InputType.Field, 0f, sliderVolSFX, fieldVolSFX));

        sliderVolMusic.onValueChanged.AddListener(value => SetAndSyncValue(InputType.Slider, value, sliderVolMusic, fieldVolMusic));
        fieldVolMusic.onEndEdit.AddListener(_ => SetAndSyncValue(InputType.Field, 0f, sliderVolMusic, fieldVolMusic)); 

        sliderSensitivity.onValueChanged.AddListener(value => SetAndSyncValue(InputType.Slider, value, sliderSensitivity, fieldSensitivity));
        fieldSensitivity.onEndEdit.AddListener(_ => SetAndSyncValue(InputType.Field, 0f, sliderSensitivity, fieldSensitivity));

        toggleVSync.onValueChanged.AddListener(isOn => currentSettings.VSync = isOn ? 1 : 0);

        currentSettings = new SettingsData();
        currentSettings.PopulateDataFromPlayerPrefs();
        SetUIFromData(currentSettings);
        ApplySettingsToGame(currentSettings);
    }


    private void SetAndSyncValue(InputType inputType, float value, Slider slider, TMP_InputField field)
    {
        if (suppressCallback) return;
        suppressCallback = true;

        if (inputType == InputType.Slider)
        {
            field.text = slider.value.ToString();
        }
        else
        {
            if (float.TryParse(field.text, out float fieldValue))
            {
                slider.value = Mathf.Clamp(fieldValue, slider.minValue, slider.maxValue);
            }

        }
        SetToDataFromUI();
        suppressCallback = false;
    }
    private void Update()
    {
    }
    private void SetUIFromData(SettingsData settings)
    {
        sliderFOV.value = settings.FOV;
        sliderVolMaster.value = settings.VolMaster;
        //Debug.Log("setting slider to " + settings.VolMaster);
        sliderVolSFX.value = settings.VolSFX;
        sliderVolMusic.value = settings.VolMusic;
        sliderSensitivity.value = settings.MouseSensitivity;
        toggleVSync.isOn = settings.VSync == 1;
    }
    private void SetToDataFromUI()
    {
        currentSettings.FOV = (int)sliderFOV.value;
        currentSettings.VolMaster = (int)sliderVolMaster.value;
        currentSettings.VolSFX = (int)sliderVolSFX.value;
        currentSettings.VolMusic = (int)sliderVolMusic.value;
        currentSettings.MouseSensitivity = sliderSensitivity.value; 
        currentSettings.VSync = toggleVSync.isOn ? 1 : 0;

    }

    private void ApplySettingsToGame(SettingsData settings)
    {
        SoundMixerManager manager = FindAnyObjectByType<SoundMixerManager>();
        manager.SetMasterVolume(settings.VolMaster / 100f);
        manager.SetSFXVolume(settings.VolSFX / 100f);
        manager.SetMusicVolume(settings.VolMusic / 100f);

        CameraDampenAndFOV dampen = FindAnyObjectByType<CameraDampenAndFOV>();
        if (dampen != null) 
        { 
            dampen.SetFOV(settings.FOV);
        }

        QualitySettings.vSyncCount = settings.VSync; // 0 = off, 1 = match refresh rate

    }

    public void Open() 
    {
        settingsPage.gameObject.SetActive(true);
    }

    void SaveAndClose()
    {
        currentSettings.Save();
        ApplySettingsToGame(currentSettings);
        settingsPage.gameObject.SetActive(false);
    }

}
