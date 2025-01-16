using UnityEngine;

public class FallHeightButtons : MonoBehaviour
{
    public void SetLowHeight()
    {
        if (FallHeightManager.Instance != null)
        {
            FallHeightManager.Instance.SetFallHeight(1f); // Low height
            Debug.Log("Fall height set to Low (1f)");
        }
    }

    public void SetMediumHeight()
    {
        if (FallHeightManager.Instance != null)
        {
            FallHeightManager.Instance.SetFallHeight(2f); // Medium height
            Debug.Log("Fall height set to Medium (2f)");
        }
    }

    public void SetHighHeight()
    {
        if (FallHeightManager.Instance != null)
        {
            FallHeightManager.Instance.SetFallHeight(3f); // High height
            Debug.Log("Fall height set to High (3f)");
        }
    }
}
