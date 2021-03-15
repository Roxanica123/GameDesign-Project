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
    static Vector3 _spawnPoint;
    static Vector3 _endPoint;
    private float _timer = 0;
    private float _lastSpawnTime = 0;
    static BoxCollider2D _greenCollider;
    private ShapeRecognizer _shapeRecognizer;

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

        public bool IsInFrontOfScoreZone()
        {
            return _greenCollider.OverlapPoint(this._reference.transform.position);
        }
    }


    void Start()
    {
        _shapeRecognizer = GameObject.Find("GameManager").GetComponent<ShapeRecognizer>();
        _spawnPoint = new Vector3(0, 1000, -5);
        _endPoint = new Vector3(0, -870, -5);
        this._notesTransform = GameObject.Find("Notes").transform;
        this._notePrefab = Resources.Load<GameObject>("Prefabs/Dummy");
        this._notesList = new List<Note>();
        _greenCollider = GameObject.Find("GreenZone").GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        this._timer += Time.deltaTime;
        foreach (Note note in _notesList)
            note.UpdateY(_timer);

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
            if (note.IsInFrontOfScoreZone())
            {
                Debug.Log("Wooo hit a note in the green zone");
            }
        }
    }
}