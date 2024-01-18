using UnityEngine;

public class ApplicationDetector : MonoBehaviour, IApplicationDetector
{
     public void Quit(RuntimePlatform platform)
    {
        if (platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }
    }
}