using UnityEngine;
using System;

public static class SettingsEvents
{
    //Settings events
    public static event Action<PlayerPrefSettings, float> OnSettingChanged;


    //Settings invokes
    public static void SettingChangedSignal(PlayerPrefSettings setting, float value)
    {
        OnSettingChanged?.Invoke(setting, value);
    }
}
