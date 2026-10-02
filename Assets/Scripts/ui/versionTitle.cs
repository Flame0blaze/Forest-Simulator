using TMPro;
using UnityEngine;

public class versionTitle : MonoBehaviour
{
    public string versionPrefix = "version";
    
    private TextMeshProUGUI label;



    void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        label.text = $"{versionPrefix}: {Application.version}";
    }
}
