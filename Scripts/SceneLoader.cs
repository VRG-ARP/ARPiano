using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string PersistentScene = "PersistentScene";

    public void LoadScene(string sceneName)
    {
        // Ensure PersistentScene is loaded
        if (!SceneManager.GetSceneByName(PersistentScene).isLoaded)
        {
            SceneManager.LoadScene(PersistentScene, LoadSceneMode.Additive);
        }

        // Unload all scenes except PersistentScene and the target scene
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene activeScene = SceneManager.GetSceneAt(i);
            if (activeScene.name != PersistentScene && activeScene.name != sceneName)
            {
                SceneManager.UnloadSceneAsync(activeScene);
            }
        }

        // Load the target scene if not already loaded
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}