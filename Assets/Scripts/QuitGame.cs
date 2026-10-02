using UnityEngine;

public class QuitGame : MonoBehaviour
{
    
    public void Leave()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #endif
    }

}
