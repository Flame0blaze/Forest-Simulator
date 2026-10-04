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
    private TMP_InputField textBox;



    void Awake() //Find and get current ui element
    {
        slider = GetComponent<Slider>();
        label = GetComponent<TextMeshProUGUI>();
        textBox = GetComponent<TMP_InputField>();
    }
    void Start()
    {
        setValues();
    }

    public void setValues()
    {
        if (slider != null) slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(setting.ToString(), slider.value));
        if (label != null) label.SetText(PlayerPrefs.GetFloat(setting.ToString(), 15f).ToString("F2"));
        if (textBox != null) textBox.SetTextWithoutNotify(PlayerPrefs.GetFloat(setting.ToString(), 15f).ToString("F2"));
    }

    public void updatePref(float value)    //Broadcasting that a setting changed (which one, new value)
    {
        PlayerPrefs.SetFloat(setting.ToString(), value);
        PlayerPrefs.Save();

        SettingsEvents.SettingChangedSignal(setting, value);
    }
    public void strUpdatePref(string value)
    {
        if (value == "") value = "0";
        float fvalue = float.Parse(value);
        fvalue = Mathf.Clamp(fvalue, 0f, 100f);
        PlayerPrefs.SetFloat(setting.ToString(), fvalue);
        PlayerPrefs.Save();

        SettingsEvents.SettingChangedSignal(setting, fvalue);
    }
}