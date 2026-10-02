using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerPrefSettings
{
    Sensivity
}

public class SaveValues : MonoBehaviour
{
    public PlayerPrefSettings setting;

    private Slider slider;
    private TextMeshProUGUI label;



    void Awake()
    {
        slider = GetComponent<Slider>();
        label = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        if (slider != null) slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(setting.ToString(), slider.value));
        if (label != null) label.text = PlayerPrefs.GetFloat(setting.ToString(), 0f).ToString("F2");
        
    }

    public void updatePref(float value)
    {
        PlayerPrefs.SetFloat(setting.ToString(), value);
        PlayerPrefs.Save();

        SettingsEvents.SettingChangedSignal(setting, value);
    }

}