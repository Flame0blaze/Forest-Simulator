using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class inputHandle : MonoBehaviour
{
    private TMP_InputField textBox;
    public Slider linkedSlider;
    public TextMeshProUGUI linkedLabel;


    void Awake()
    {
        textBox = GetComponent<TMP_InputField>();
    }
    public void processInput()
    {
        if (!float.TryParse(textBox.text, out float value)) textBox.text = linkedSlider.value.ToString("F2");
        value = Mathf.Clamp(value, linkedSlider.minValue, linkedSlider.maxValue);
        textBox.SetTextWithoutNotify(value.ToString("F2"));
        linkedSlider.value = value;
    }

    public void syncLabel()
    {
        linkedLabel.text = textBox.text;
    }
}
