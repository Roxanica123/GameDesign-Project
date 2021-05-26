﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class NotesGenerator
{
    private Queue<float> _timestamps;
    private NotesFactory _notesFactory;
    public List<Note> GeneratedNotes { get; private set; }
    private float _last = 0;
    private float _tapProbability = (float) 0.6;

    public NotesGenerator(float[] timestamps, int difficulty)
    {
        _timestamps = new Queue<float>(timestamps);
        _notesFactory = new NotesFactory(difficulty);
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
                newNote = _notesFactory.GetNoteWithDuration(currentTimestamp - _last, 0);
                if (newNote == null) return;
            }

            newNote.SetTimeOfNote(currentTimestamp);
            GeneratedNotes.Add(newNote);
            _last = currentTimestamp;
        }
    }
}