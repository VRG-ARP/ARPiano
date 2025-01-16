using UnityEngine;
using MixedReality.Toolkit.UX;
using TMPro; // Import TextMeshPro namespace

public class FallSpeedSlider : MonoBehaviour
{
    public Slider slider; // Assign the MRTK slider in the Inspector
    public TextMeshPro valueDisplay; // For 3D TextMeshPro

    private bool isInitializing = true; // Prevent triggering value changes during initialization

    private void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider reference is missing!");
            return;
        }

        slider.OnValueUpdated.AddListener(HandleSliderValueChanged);

        // Ensure slider reflects the correct speed from FallSpeedManager
        if (FallSpeedManager.Instance != null)
        {
            float currentSpeed = FallSpeedManager.Instance.GetFallSpeed();
            slider.Value = Mathf.Round((currentSpeed - 0.1f) * 19f + 1f); // Map fall speed back to slider value (1-20 range)
            UpdateValueDisplay((int)slider.Value); // Set initial display
        }
        else
        {
            Debug.LogError("FallSpeedManager instance is missing in Persistent Scene!");
        }

        // End initialization phase
        isInitializing = false;
    }

    private void HandleSliderValueChanged(SliderEventData eventData)
    {
        if (isInitializing) return; // Ignore changes during initialization

        float newValue = Mathf.Round(eventData.NewValue); // Ensure whole number values
        slider.Value = newValue; // Update slider value to whole number

        if (FallSpeedManager.Instance != null)
        {
            FallSpeedManager.Instance.SetFallSpeed(newValue);
            Debug.Log($"Slider updated fall speed to: {newValue}");
        }

        UpdateValueDisplay((int)newValue); // Update the text display
    }

    private void UpdateValueDisplay(int value)
    {
        if (valueDisplay != null)
        {
            valueDisplay.text = $"Speed: {value}"; // Display speed value
        }
        else
        {
            Debug.LogError("Value display reference is missing!");
        }
    }
}
