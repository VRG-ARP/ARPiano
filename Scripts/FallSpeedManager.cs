using UnityEngine;

public class FallSpeedManager : MonoBehaviour
{
    public static FallSpeedManager Instance { get; private set; }

    private float fallSpeed; // Current fall speed

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Set default fall speed based on slider value 5
            SetFallSpeed(5);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetFallSpeed()
    {
        return fallSpeed;
    }

    public void SetFallSpeed(float sliderValue)
    {
        // Map the slider value (1 to 20) to a meaningful fall speed (e.g., 0.1 to 1.0)
        fallSpeed = Mathf.Lerp(0.1f, 1.0f, (sliderValue - 1) / 19f);
        Debug.Log($"Fall speed updated to: {fallSpeed}");
    }
}
