using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject fallingNotePrefab;
    private Queue<CustomNoteData> noteQueue = new Queue<CustomNoteData>();
    private float timer = 0f;
    private Dictionary<int, Vector3> keyTransforms = new Dictionary<int, Vector3>();

    public void Initialize(Transform pianoParent, List<CustomNoteData> notes)
    {
        foreach (Transform child in pianoParent)
        {
            if (child.name.StartsWith("Key"))
            {
                if (int.TryParse(child.name.Replace("Key", ""), out int keyNumber))
                {
                    // Search for colliders in child objects
                    Collider keyCollider = child.GetComponentInChildren<Collider>();
                    if (keyCollider != null)
                    {
                        keyTransforms[keyNumber] = keyCollider.bounds.center;
                        //Debug.Log($"Key {keyNumber} - Bounds Center: {keyCollider.bounds.center}");
                    }
                    else
                    {
                        //Debug.LogWarning($"Key {keyNumber} does not have a collider in its children!");
                    }
                }
            }
        }

        // Sort and enqueue notes
        notes.Sort((a, b) => a.startTime.CompareTo(b.startTime));
        foreach (var note in notes)
        {
            noteQueue.Enqueue(note);
        }
    }


    private void Update()
    {
        if (ModeManager.Instance.GetCurrentMode() == ModeManager.PianoMode.Freestyle)
        {
            return; // No note spawning in Freestyle mode
        }

        if (noteQueue == null || keyTransforms == null || noteQueue.Count == 0) return;

        timer += Time.deltaTime;

        while (noteQueue.Count > 0 && noteQueue.Peek().startTime <= timer)
        {
            SpawnFallingNote(noteQueue.Dequeue());
        }
    }
    private void SpawnFallingNote(CustomNoteData note)
    {
        if (!keyTransforms.ContainsKey(note.pitch)) return;

        // Get the current fall height from FallHeightManager
        float currentFallHeight = FallHeightManager.Instance != null ? FallHeightManager.Instance.GetFallHeight() : 5f;

        Vector3 spawnPosition = keyTransforms[note.pitch] + Vector3.up * currentFallHeight;
        GameObject fallingNote = Instantiate(fallingNotePrefab, spawnPosition, Quaternion.identity);

        // Set the layer of the spawned note to "FallingNotes"
        fallingNote.layer = LayerMask.NameToLayer("FallingNote");

        var fallingNoteScript = fallingNote.GetComponent<FallingNote>();
        if (fallingNoteScript != null)
        {
            float currentSpeed = FallSpeedManager.Instance.GetFallSpeed();
            //Debug.Log($"Applying fall speed: {currentSpeed}, Fall height: {currentFallHeight}");
            fallingNoteScript.SetSpeed(currentSpeed);
            fallingNoteScript.SetDuration(note.duration);
            fallingNoteScript.SetVelocity(note.velocity);
        }
    }
}
