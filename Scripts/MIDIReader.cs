using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using System.IO;

public class MIDIReader
{
    public List<CustomNoteData> ParseMIDIFile(byte[] midiBytes)
    {
        var noteDataList = new List<CustomNoteData>();
        try
        {
            // Read the MIDI file from the byte array using a MemoryStream
            using (var stream = new MemoryStream(midiBytes))
            {
                MidiFile midiFile = MidiFile.Read(stream);

                var tempoMap = midiFile.GetTempoMap();
                var notes = midiFile.GetNotes();

                foreach (var note in notes)
                {
                    var velocity = note.Velocity; // Extract velocity from MIDI data
                    noteDataList.Add(new CustomNoteData(
                        note.NoteNumber,
                        (float)note.TimeAs<MetricTimeSpan>(tempoMap).TotalSeconds,
                        (float)note.LengthAs<MetricTimeSpan>(tempoMap).TotalSeconds,
                        velocity));
                }

            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing MIDI file: {ex.Message}");
        }

        return noteDataList;
    }
}

[System.Serializable]
public class NoteData
{
    public int pitch;
    public float startTime;
    public float duration;
}