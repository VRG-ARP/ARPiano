using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NoteSpawner noteSpawner; // Drag NoteSpawner GameObject here
    [SerializeField] private Transform pianoParent;   // Drag Piano_88 GameObject here
    private string selectedMidiPath; // Path to the selected MIDI file (dynamic)

    void Start()
    {
        // Load the selected MIDI file (if it exists)
        selectedMidiPath = PlayerPrefs.GetString("SelectedMIDIPath", "");
        if (!string.IsNullOrEmpty(selectedMidiPath))
        {
            Debug.Log($"Selected MIDI file: {selectedMidiPath}");
            LoadAndPlayMIDI(selectedMidiPath);
        }
        else
        {
            Debug.LogError("No MIDI file selected. Ensure a song is chosen in the Song Selection scene.");
        }
    }

    private void LoadAndPlayMIDI(string filePath)
    {
        try
        {
            // Read the MIDI file bytes from the file path
            byte[] midiBytes = System.IO.File.ReadAllBytes(filePath);

            if (midiBytes == null || midiBytes.Length == 0)
            {
                Debug.LogError("Failed to read MIDI file. Ensure the file exists and is not empty.");
                return;
            }

            Debug.Log($"Successfully loaded MIDI file from path: {filePath}");

            // Parse the MIDI file
            var midiReader = new MIDIReader(); // Ensure MIDIReader is implemented correctly
            var noteDataList = midiReader.ParseMIDIFile(midiBytes);

            if (noteDataList == null || noteDataList.Count == 0)
            {
                Debug.LogError("No notes were found in the MIDI file. Ensure the file contains valid MIDI data.");
                return;
            }

            // Initialize the NoteSpawner
            noteSpawner.Initialize(pianoParent, noteDataList);
            Debug.Log("NoteSpawner initialized with MIDI note data.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error loading and playing MIDI file: {ex.Message}");
        }
    }
}