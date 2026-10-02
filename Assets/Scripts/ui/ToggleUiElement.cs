using UnityEngine;
using UnityEngine.UI;

public class ToggleUiElement : MonoBehaviour
{
    public GameObject rawImage;
    
    public void Toggle()
    {
        rawImage.SetActive(!rawImage.activeSelf);
    }
}
