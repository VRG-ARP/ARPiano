public class CustomNoteData
{
    public int pitch;          // MIDI pitch value (e.g., 60 for middle C)
    public float startTime;    // When the note starts (in seconds)
    public float duration;     // How long the note lasts (in seconds)
    public int velocity;       // MIDI velocity (volume/intensity)

    public CustomNoteData(int pitch, float startTime, float duration, int velocity)
    {
        this.pitch = pitch;
        this.startTime = startTime;
        this.duration = duration;
        this.velocity = velocity;
    }
}
