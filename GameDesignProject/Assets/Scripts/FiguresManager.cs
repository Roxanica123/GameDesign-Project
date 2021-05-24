using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FiguresManager : MonoBehaviour
{
    [Serializable]
    private class BeatmapFile
    {
        public float[] times;
    }

    private const string SOUNDTRACK_NAME = "soundtrack1";
    private AudioClip _drumHitClip;

    private List<Note> _notesList;
    private ScoreManager _scoreManager;
    private NotesFactory _notesFactory;
    private ShapeRecognizer _shapeRecognizer;
    private AudioSource _audioSource;
    private NotesGenerator _notesGenerator;


    private float[] LoadBeatmap()
    {
        var audioClip = Resources.Load<AudioClip>($"Sound/{SOUNDTRACK_NAME}");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = audioClip;

        return JsonUtility.FromJson<BeatmapFile>(Resources.Load<TextAsset>($"Beatmaps/{SOUNDTRACK_NAME}").text)
            .times;
    }

    public void Play() => _audioSource.Play();
    public void Pause() => _audioSource.Pause();

    void Start()
    {
        this._notesList = new List<Note>();
        this._notesFactory = new NotesFactory();
        _shapeRecognizer = transform.GetComponent<ShapeRecognizer>();
        _scoreManager = transform.GetComponent<ScoreManager>();
        GameObject.FindGameObjectsWithTag("ScoreZone")
            .Select(obj => obj.GetComponent<ScoreZone>())
            .ToList()
            .ForEach(zone => this._scoreManager.AddScoreZone(zone));

        _drumHitClip = Resources.Load<AudioClip>("Sound_Effects/drum-hit");

        _notesGenerator = new NotesGenerator(LoadBeatmap());
        _notesList = _notesGenerator.GeneratedNotes;
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


        if (_notesList.Count == 0 && EndGameMenu.Ended == false)
        {
            EndGameMenu.EndGame(_scoreManager.TotalScore);
        }
    }


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
            {
                notesToCheck[0].Hit = true;
                _audioSource.PlayOneShot(_drumHitClip);
            }
    }
}