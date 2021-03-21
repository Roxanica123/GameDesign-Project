using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using Unity.Notifications.iOS;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    public static Vector3 SpawnPoint { get; private set; }
    public static Vector3 EndPoint { get; private set; }
    public Transform NotesTransform { get; private set; }
    public GameObject NotePrefab { get; private set; }
    private List<Note> _notesList;
    private List<Note> _notesToRemove;
    private float _timer = 0;
    private float _lastSpawnTime = 0;
    private ScoreManager scoreManager;

    private ShapeRecognizer _shapeRecognizer;

    void Start()
    {
        SpawnPoint = new Vector3(0, Screen.height / 2.0f, -5);
        EndPoint = new Vector3(0, -Screen.height / 2.0f, -5);
        NotesTransform = GameObject.Find("Notes").transform;
        NotePrefab = Resources.Load<GameObject>("Prefabs/Dummy");

        _shapeRecognizer = GameObject.Find("GameManager").GetComponent<ShapeRecognizer>();
        
        this._notesList = new List<Note>();
        this._notesToRemove = new List<Note>();

        this.scoreManager = new ScoreManager();
        scoreManager.AddScoreZone(new ScoreZone(GameObject.Find("GreenZone").GetComponent<BoxCollider2D>(), 2, true));
        scoreManager.AddScoreZone(new ScoreZone(GameObject.Find("RedZone1").GetComponent<BoxCollider2D>()));
        scoreManager.AddScoreZone(new ScoreZone(GameObject.Find("RedZone2").GetComponent<BoxCollider2D>()));
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

    private void SpawnNote(float time)
    {
        _notesList.Add(new Note(time, NotePrefab, NotesTransform));
    }

    public void OnNewShape()
    {
        string shape = _shapeRecognizer.Shape;

        foreach (Note note in _notesList)
        {
            var position = note.GetPosition();
            Debug.Log(scoreManager.GetScoreUpdate(position));
            Debug.Log(scoreManager.TotalScore);
        }
    }
}