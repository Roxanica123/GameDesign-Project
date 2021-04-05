using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    [Serializable]
    private class BeatmapFile
    {
        public float[] times;
    }
    
    private List<Note> _notesList;
    private List<Note> _notesToRemove;
    private ScoreManager _scoreManager;
    private NotesFactory _notesFactory;
    private ShapeRecognizer _shapeRecognizer;

    private AudioSource _audioSource;
    private Queue<float> _beatmapTimings;

    private void LoadBeatmap()
    {
        var audioClip = Resources.Load<AudioClip>("Sound/soundtrack");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = audioClip;

        var timings = JsonUtility.FromJson<BeatmapFile>(Resources.Load<TextAsset>("Beatmaps/soundtrack1.wav").text).times;
        _beatmapTimings = new Queue<float>(timings);
    }

    public void Play() => _audioSource.Play();
    public void Pause() => _audioSource.Pause();

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
        
        LoadBeatmap();
        Play();
    }

    private void Update()
    {
        foreach (Note note in _notesList)
        {
            note.UpdateY(_audioSource.time);
            if (note.HasPassedEndpoint())
            {
                if (note.Hit == false)
                    _scoreManager.MissedNote();
                Destroy(note.GameObject);
            }
        }
        _notesList.RemoveAll(note => note.HasPassedEndpoint());
        
        
        if (_beatmapTimings.Peek() - _audioSource.time <= 2)
            SpawnNote(_beatmapTimings.Dequeue());
    }

    // void Update()
    // {
    //     this._timer += Time.deltaTime;
    //
    //     foreach (Note note in _notesList)
    //     {
    //         note.UpdateY(_timer);
    //         if (note.HasPassedEndpoint())
    //         {
    //             if (note.Hit == false)
    //                 _scoreManager.UpdateScore(note.GetPosition());
    //             Destroy(note.GameObject);
    //         }
    //     }
    //
    //     _notesList.RemoveAll(note => note.HasPassedEndpoint());
    //
    //     if (_timer - _lastSpawnTime >= 2)
    //     {
    //         SpawnNote(_timer + 2);
    //         _lastSpawnTime = _timer;
    //     }
    // }

    
    
    private void SpawnNote(float time)
    {
        _notesList.Add(_notesFactory.GetRandomNote(time));
    }

    // public void OnNewShape()
    // {
    //     string shape = _shapeRecognizer.Shape;
    //     foreach (Note note in _notesList)
    //     {
    //         if (note.Hit == false && shape == note.Type)
    //         {
    //             var position = note.GetPosition();
    //             _scoreManager.UpdateScore(position);
    //             note.Hit = true;
    //             //this is broken tho, gotta get the rules straight
    //         }
    //     }
    // }

    public void OnNewShape()
    {
        string drawnShape = _shapeRecognizer.Shape;
        List<Note> notesToCheck = _notesList
            .Where(note => note.Type == drawnShape)
            .Where(note => note.Hit == false)
            .Where(note => _scoreManager.IsInAnyScoreZone(note.GetPosition()))
            .ToList();

        if (notesToCheck.Count > 0)
            if (_scoreManager.UpdateScore(notesToCheck[0].GetPosition()))
                notesToCheck[0].Hit = true;
    }
}