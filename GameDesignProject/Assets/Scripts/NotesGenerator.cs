using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NotesGenerator
{
    private Queue<float> _timestamps;
    private NotesFactory _notesFactory;
    public List<Note> GeneratedNotes { get; private set; }
    private float _last = 0;

    public NotesGenerator(float[] timestamps)
    {
        _timestamps = new Queue<float>(timestamps);
        _notesFactory = new NotesFactory();
        GenerateNotes();
    }

    private void GenerateNotes()
    {
        GeneratedNotes = new List<Note>();
        while (_timestamps.Count > 0)
        {
            var newNote = _notesFactory.GetRandomNote(0);
            var currentTimestamp = _timestamps.Dequeue();
            while (_timestamps.Count > 0 && currentTimestamp - _last < newNote.Duration)
            {
                currentTimestamp = _timestamps.Dequeue();
            }

            if (currentTimestamp - _last < newNote.Duration)
            {
                _notesFactory.GetNoteWithDuration(currentTimestamp - _last, 0);
            }

            newNote.SetTimeOfNote(currentTimestamp);
            GeneratedNotes.Add(newNote);
            _last = currentTimestamp;
        }
    }
}