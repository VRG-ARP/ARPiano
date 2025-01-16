using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectionManager : MonoBehaviour
{
    private string selectedSongPath;

    public void SelectSong(string midiFilePath)
    {
        selectedSongPath = midiFilePath;
        Debug.Log($"Selected song: {midiFilePath}");
    }

    public void StartPianoScene()
    {
        if (string.IsNullOrEmpty(selectedSongPath))
        {
            Debug.LogError("No song selected!");
            return;
        }

        // Save the selected file path in PlayerPrefs
        PlayerPrefs.SetString("SelectedMIDIPath", selectedSongPath);

        // Load the Piano scen

        // Unload the Song Selection Scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Song Selection")
        {
            SceneManager.UnloadSceneAsync(currentSceneName);
            Debug.Log("Song Selection Scene unloaded.");
        }
        else
        {
            Debug.LogWarning("Song Selection Scene is not loaded or invalid.");
        }
    }
}
