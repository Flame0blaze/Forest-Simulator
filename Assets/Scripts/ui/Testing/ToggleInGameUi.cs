using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ToggleInGameUi : MonoBehaviour
{
    public RawImage rawImage;
    public byte alpha = 255;
    public bool isActive = true;
    public float latency = 5;
    void Start()
    {
        if (isActive)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alpha);
            StartCoroutine(DisableImageRoutine());
        }
        else rawImage.gameObject.SetActive(false);
        
    }

    private IEnumerator DisableImageRoutine()
    {
        yield return new WaitForSeconds(latency);
        rawImage.gameObject.SetActive(false);
    }

}
