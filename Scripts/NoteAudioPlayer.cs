using UnityEngine;
using System.Collections;

public class NoteAudioPlayer : MonoBehaviour
{
    public AudioClip noteClip; // Assign the correct note audio clip here

    public void PlayNoteWithFade(float duration, int velocity)
    {
        // Create a temporary GameObject for this note's audio
        GameObject tempAudioObject = new GameObject($"TempAudio_{name}");
        AudioSource tempAudioSource = tempAudioObject.AddComponent<AudioSource>();

        // Configure the temporary AudioSource
        tempAudioSource.clip = noteClip;
        tempAudioSource.volume = Mathf.Clamp(velocity / 127f, 0.2f, 1f); // Normalize velocity to volume
        tempAudioSource.spatialBlend = 0f; // Ensure 2D sound
        tempAudioSource.playOnAwake = false;

        // Play the note with fade-in and fade-out
        StartCoroutine(HandleNotePlayback(tempAudioSource, duration));
    }

    private IEnumerator HandleNotePlayback(AudioSource audioSource, float duration)
    {
        // Fade-in phase
        float fadeInTime = Mathf.Min(0.1f, duration / 4f);
        float targetVolume = audioSource.volume;
        audioSource.volume = 0f;

        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / fadeInTime;
            yield return null;
        }

        // Sustain phase
        yield return new WaitForSeconds(duration - fadeInTime);

        // Fade-out phase
        float fadeOutTime = Mathf.Min(0.1f, duration / 4f);
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }

        // Cleanup: Stop playback and destroy the temporary GameObject
        audioSource.Stop();
        Destroy(audioSource.gameObject);
    }
}
