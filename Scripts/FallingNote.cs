using UnityEngine;

public class FallingNote : MonoBehaviour
{
    private float speed;
    private float duration = 1f;
    private int velocity = 100;
    private bool isOnKey = false;
    private Transform keyToReset;

    public void SetSpeed(float newSpeed) => speed = newSpeed;
    public void SetDuration(float newDuration) => duration = newDuration;
    public void SetVelocity(int newVelocity) => velocity = newVelocity;

    private void Update()
    {
        if (!isOnKey)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y < -1f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PianoKey") && !isOnKey)
        {
            isOnKey = true;
            PressKey(other);

            // Play sound in Auto mode
            if (ModeManager.Instance.GetCurrentMode() == ModeManager.PianoMode.Auto)
            {
                PlayNoteSound(other);
            }

            Destroy(gameObject, 0.1f);
        }
    }

    private void PressKey(Collider keyCollider)
    {
        keyToReset = keyCollider.transform;

        // Stop any ongoing reset to avoid multiple adjustments
        CancelInvoke(nameof(ResetKeyPosition));

        // Move the key slightly downward
        keyToReset.localPosition += new Vector3(0, -0.02f, 0);

        // Schedule key reset
        Invoke(nameof(ResetKeyPosition), 0.1f);
    }

    private void ResetKeyPosition()
    {
        if (keyToReset != null)
        {
            // Reset key position directly to avoid cumulative errors
            keyToReset.localPosition = new Vector3(
                keyToReset.localPosition.x,
                0, // Reset to original Y position (assuming 0 is the baseline)
                keyToReset.localPosition.z
            );
        }
    }

    private void PlayNoteSound(Collider keyCollider)
    {
        var notePlayer = keyCollider.GetComponentInParent<NoteAudioPlayer>();
        if (notePlayer != null)
        {
            notePlayer.PlayNoteWithFade(duration, velocity);
        }
    }
}
