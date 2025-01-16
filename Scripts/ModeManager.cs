using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager Instance { get; private set; }

    public enum PianoMode { Freestyle, Auto, Practice }
    private PianoMode currentMode = PianoMode.Freestyle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMode(PianoMode mode)
    {
        currentMode = mode;
        Debug.Log($"Mode set to: {mode}");
    }

    public PianoMode GetCurrentMode()
    {
        return currentMode;
    }
}
