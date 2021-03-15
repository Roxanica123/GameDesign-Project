using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Unity.Notifications.iOS;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    private Transform _notesTransform;
    private GameObject _notePrefab;
    private List<Note> _notesList;
    private List<Note> _notesToRemove;
    static Vector3 _spawnPoint;
    static Vector3 _endPoint;
    private float _timer = 0;
    private float _lastSpawnTime = 0;
    static BoxCollider2D _greenCollider;
    static BoxCollider2D _redCollider1;
    static BoxCollider2D _redCollider2;
    private ShapeRecognizer _shapeRecognizer;

    private enum ScoreZone
    {
        GREEN,
        RED
    }

    private class Note
    {
        private readonly float _timeOfNote;
        private readonly float duration = 2;
        private readonly GameObject _reference;

        public Note(float time, GameObject reference)
        {
            _timeOfNote = time;
            this._reference = reference;
        }

        private float GetY(float currTime)
        {
            return ((_timeOfNote - currTime) / duration) * (_spawnPoint.y - _endPoint.y);
        }

        public void UpdateY(float currTime)
        {
            Vector3 currPos = _reference.transform.position;
            Vector3 newPos = new Vector3(currPos.x, GetY(currTime), currPos.z);
            _reference.transform.position = newPos;
        }

        public bool HasPassedEndpoint()
        {
            return this._reference.transform.position.y < _endPoint.y;
        }

        public bool IsInFrontOfScoreZone(ScoreZone zone)
        {
            Vector3 point = this._reference.transform.position;
            if (zone == ScoreZone.GREEN)
                return _greenCollider.OverlapPoint(point);
            else
            {
                return _redCollider1.OverlapPoint(point) ||
                       _redCollider2.OverlapPoint(point);
            }
        }

        public GameObject GameObject => _reference;
    }


    void Start()
    {
        _shapeRecognizer = GameObject.Find("GameManager").GetComponent<ShapeRecognizer>();
        _spawnPoint = new Vector3(0, Screen.height / 2.0f, -5);
        _endPoint = new Vector3(0, -Screen.height / 2.0f, -5);
        this._notesTransform = GameObject.Find("Notes").transform;
        this._notePrefab = Resources.Load<GameObject>("Prefabs/Dummy");
        this._notesList = new List<Note>();
        this._notesToRemove = new List<Note>();
        _greenCollider = GameObject.Find("GreenZone").GetComponent<BoxCollider2D>();
        _redCollider1 = GameObject.Find("RedZone1").GetComponent<BoxCollider2D>();
        _redCollider2 = GameObject.Find("RedZone2").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        this._timer += Time.deltaTime;

        foreach (Note note in _notesList)
        {
            note.UpdateY(_timer);
            if (note.HasPassedEndpoint())
            {
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

    private GameObject SpawnNote(float time)
    {
        var newNote = Instantiate(this._notePrefab, new Vector3(0, 1500, -5), Quaternion.identity);
        newNote.transform.SetParent(this._notesTransform);
        _notesList.Add(new Note(time, newNote));
        return newNote;
    }

    public void OnNewShape()
    {
        string shape = _shapeRecognizer.Shape;

        foreach (Note note in _notesList)
        {
            if (note.IsInFrontOfScoreZone(ScoreZone.GREEN))
            {
                Debug.Log("Wooo hit a note in the green zone");
            }
            else if (note.IsInFrontOfScoreZone(ScoreZone.RED))
            {
                Debug.Log("Wooo hit a note in the red zone");
            }
        }
    }
}