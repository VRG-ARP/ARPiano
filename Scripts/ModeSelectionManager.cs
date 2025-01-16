using UnityEngine;

public class ModeSelectionManager : MonoBehaviour
{
    public void SelectFreestyleMode()
    {
        ModeManager.Instance.SetMode(ModeManager.PianoMode.Freestyle);
        Debug.Log("Freestyle mode selected.");
    }

    public void SelectAutoMode()
    {
        ModeManager.Instance.SetMode(ModeManager.PianoMode.Auto);
        Debug.Log("Auto mode selected.");
    }

    public void SelectPracticeMode()
    {
        ModeManager.Instance.SetMode(ModeManager.PianoMode.Practice);
        Debug.Log("Practice mode selected.");
    }
}
