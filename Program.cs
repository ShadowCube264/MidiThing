using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public static class Program
{
    public static void Main(string[] args)
    {
        var midiFile = MidiFile.Read("../../Music/test2.mid");
        var trackNum = 0;
        var formatted = "";
        var tempoMap = midiFile.GetTempoMap();

        foreach (var track in midiFile.GetTrackChunks())
        {
            if (track.GetNotes().Count != 0)
            {
                formatted += formatted.Equals("") ? trackNum : ";" + trackNum;
                trackNum += 1;
                var lastNoteTime = 0.0;

                var tooHighOffset = 0;
                var tooLowOffset = 0;
                foreach (var note in track.GetNotes())
                {
                    if (note.NoteNumber > 78)
                    {
                        tooHighOffset = Math.Max(tooHighOffset, (int)Math.Ceiling((note.NoteNumber - 78)/12.0));
                    }
                    else if (note.NoteNumber < 54)
                    {
                        tooLowOffset = Math.Max(tooLowOffset, (int)Math.Ceiling((54 - note.NoteNumber)/12.0));
                    }
                }
                bool shouldTranspose = (tooHighOffset > 0 && tooLowOffset == 0) || (tooHighOffset == 0 && tooLowOffset > 0);

                foreach (var note in track.GetNotes())
                {
                    var noteNum = (int)note.NoteNumber;
                    if (shouldTranspose)
                    {
                        noteNum += tooHighOffset * 12;
                        noteNum -= tooLowOffset * 12;
                    }

                    formatted += ":" + ConvertPitch(noteNum) + ",";

                    var timeSpan = (MetricTimeSpan)note.TimeAs(TimeSpanType.Metric, tempoMap);
                    var time = timeSpan.TotalMilliseconds;
                    formatted += (int)((time - lastNoteTime)/50);
                    lastNoteTime = time;
                }
            }
        }
        Console.WriteLine(formatted);
    }

    public static int ConvertPitch(int midNum)
    {
        var newPitch = midNum - 54;
        return Math.Clamp(newPitch, 0, 24);
    }
}