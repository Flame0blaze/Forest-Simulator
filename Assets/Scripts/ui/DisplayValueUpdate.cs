using UnityEngine;
using TMPro;

public class DisplayValueUpdate : MonoBehaviour
{
    public TextMeshProUGUI label;
    public void updateValue(float value)
    {
        label.text = value.ToString("F2");
    }

}
