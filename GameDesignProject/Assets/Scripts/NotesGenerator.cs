using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class NotesGenerator
{
    [Serializable]
    public class NoteTime
    {
        public string shape;
        public float time;
    }

    private readonly List<NoteTime> _timestamps;
    private readonly NotesFactory _notesFactory;
    public List<Note> GeneratedNotes { get; private set; }


    public NotesGenerator(List<NoteTime> timestamps, int difficulty)
    {
        _timestamps = timestamps;
        _notesFactory = new NotesFactory(difficulty);
        GenerateNotes();
    }

    private void GenerateNotes()
    {
        GeneratedNotes = new List<Note>();

        foreach (var timestamp in _timestamps)
        {
            Debug.Log(timestamp.time);
            GeneratedNotes.Add(_notesFactory.GetNote(timestamp.shape, timestamp.time));
        }
    }
}