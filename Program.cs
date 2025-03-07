using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public static class Program
{
    public static void Main(string[] args)
    {
        var midiFile = MidiFile.Read("../../Music/test2.mid");
        foreach (var track in midiFile.GetTrackChunks())
        {
            Console.WriteLine("Track!");
            Console.WriteLine(track.ChunkId);
            Console.WriteLine(track.GetDuration(TimeSpanType.Metric, midiFile.GetTempoMap()));
            foreach (var note in track.GetNotes())
            {
                Console.WriteLine("Note!");
                Console.WriteLine(note.NoteNumber);
            }
        }
    }
}