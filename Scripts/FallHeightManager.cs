using UnityEngine;

public class FallHeightManager : MonoBehaviour
{
    public static FallHeightManager Instance { get; private set; }

    private float fallHeight = 2f; // Default spawn height

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

    public float GetFallHeight()
    {
        return fallHeight;
    }

    public void SetFallHeight(float newHeight)
    {
        fallHeight = newHeight;
        Debug.Log($"Fall height updated to: {fallHeight}");
    }
}
