using UnityEngine;
using UnityEngine.UI;

public class controlUiValue : MonoBehaviour
{
    //Read a value from an ui element functions
    public float ReadSlider(Slider slider)
    {
        return slider.value;
    }

    public bool ReadToggle(Toggle toggle)
    {
        return toggle.isOn;
    } 
}
