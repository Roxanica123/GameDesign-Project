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
        _shapeRecognizer = GameObject.Find("GameManager").GetComponent<ShapeRecognizer>();

        this._notesList = new List<Note>();
        this._notesToRemove = new List<Note>();

        this._notesFactory = new NotesFactory();
        this._scoreManager = new ScoreManager();
        _scoreManager.AddScoreZone(new ScoreZone(GameObject.Find("GreenZone").GetComponent<BoxCollider2D>(), 2, true));
        _scoreManager.AddScoreZone(new ScoreZone(GameObject.Find("RedZone1").GetComponent<BoxCollider2D>()));
        _scoreManager.AddScoreZone(new ScoreZone(GameObject.Find("RedZone2").GetComponent<BoxCollider2D>()));
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
                    _scoreManager.GetScoreUpdate(note.GetPosition());
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
                _scoreManager.GetScoreUpdate(position);
                note.Hit = true;
                _scoreManager.Update();
                //this is broken tho, gotta get the rules straight
            }
        }
    }
}