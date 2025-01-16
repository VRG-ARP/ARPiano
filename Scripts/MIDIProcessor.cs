using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MIDIProcessor
{
    public static List<CustomNoteData> GetNoteDataList(MidiFile midiFile)
    {
        var noteDataList = new List<CustomNoteData>();
        var tempoMap = midiFile.GetTempoMap();
        var notes = midiFile.GetNotes();

        foreach (var note in notes)
        {
            int pitch = note.NoteNumber;
            float startTime = (float)note.TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds;
            float duration = (float)note.LengthAs<MetricTimeSpan>(tempoMap).TotalSeconds;
            int velocity = note.Velocity; // Extract velocity from the MIDI note

            noteDataList.Add(new CustomNoteData(pitch, startTime, duration, velocity));
        }

        return noteDataList;
    }
}
