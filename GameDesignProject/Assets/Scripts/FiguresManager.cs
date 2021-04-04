using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    private List<Note> _notesList;
    private List<Note> _notesToRemove;
    private float _timer = 0;
    private float _lastSpawnTime = 0;
    private ScoreManager _scoreManager;
    private NotesFactory _notesFactory;
    private ShapeRecognizer _shapeRecognizer;

    void Start()
    {
        this._notesList = new List<Note>();
        this._notesToRemove = new List<Note>();
        this._notesFactory = new NotesFactory();
        _shapeRecognizer = transform.GetComponent<ShapeRecognizer>();
        _scoreManager = transform.GetComponent<ScoreManager>();
        GameObject.FindGameObjectsWithTag("ScoreZone")
            .Select(obj => obj.GetComponent<ScoreZone>())
            .ToList()
            .ForEach(zone => this._scoreManager.AddScoreZone(zone));
    }

    void Update()
    {
        this._timer += Time.deltaTime;

        foreach (Note note in _notesList)
        {
            note.UpdateY(_timer);
            if (note.HasPassedEndpoint())
            {
                if (note.Hit == false)
                    _scoreManager.UpdateScore(note.GetPosition());
                Destroy(note.GameObject);
            }
        }

        _notesList.RemoveAll(note => note.HasPassedEndpoint());

        if (_timer - _lastSpawnTime >= 2)
        {
            SpawnNote(_timer + 2);
            _lastSpawnTime = _timer;
        }
    }

    private void SpawnNote(float time)
    {
        _notesList.Add(_notesFactory.GetRandomNote(time));
    }

    public void OnNewShape()
    {
        string shape = _shapeRecognizer.Shape;
        foreach (Note note in _notesList)
        {
            if (note.Hit == false && shape == note.Type)
            {
                var position = note.GetPosition();
                _scoreManager.UpdateScore(position);
                note.Hit = true;
                //this is broken tho, gotta get the rules straight
            }
        }
    }
}