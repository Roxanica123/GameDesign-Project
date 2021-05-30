using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Events;
using JetBrains.Annotations;

public class FiguresManager : MonoBehaviour
{
    public UnityEvent onNoteHit = new UnityEvent();
    
    [Serializable]
    private class BeatmapFile
    {
        public List<NotesGenerator.NoteTime> times;
        public float[] tempo;
    }

    private AudioClip _drumHitClip;

    private List<Note> _notesList;
    private float[] _tempo;
    private ScoreManager _scoreManager;
    private ShapeRecognizer _shapeRecognizer;
    private AudioSource _audioSource;
    private NotesGenerator _notesGenerator;
    private PlayerData _playerData;
    private string path;

    public List<Note> currentNotes => _notesList;
    public float[] Tempo => _tempo;
    public AudioSource audioSource => _audioSource;

    [Serializable]
    private class Level
    {
        public int index;
        public string trackName;
        public string filename;
        public int score;
        public int stars;
        public int difficulty;
    }

    [Serializable]
    private class PlayerData
    {
        public List<Level> levelsList;
        public int starsCounter;
        public int difficulty;
    }

    private List<NotesGenerator.NoteTime> LoadBeatmap()
    {
        string filename = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].filename;
        var audioClip = Resources.Load<AudioClip>($"Sound/{filename}");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = audioClip;

        var rez = JsonUtility.FromJson<BeatmapFile>(Resources.Load<TextAsset>($"Beatmaps/{filename}").text);
        this._tempo = rez.tempo;
        return rez.times;
    }


    public void Play() => _audioSource.Play();
    public void Pause() => _audioSource.Pause();

    void Start()
    {
        path = Application.persistentDataPath + "/GameDesignProject/savefiles/savefile.json";
        Debug.Log("Received index: " + PlayerPrefs.GetInt("levelIndex"));
        LoadPlayerData();
        this._notesList = new List<Note>();
        _shapeRecognizer = transform.GetComponent<ShapeRecognizer>();
        _scoreManager = transform.GetComponent<ScoreManager>();
        GameObject.FindGameObjectsWithTag("ScoreZone")
            .Select(obj => obj.GetComponent<ScoreZone>())
            .ToList()
            .ForEach(zone => this._scoreManager.AddScoreZone(zone));

        _drumHitClip = Resources.Load<AudioClip>("Sound_Effects/drum-hit");

        _notesGenerator = new NotesGenerator(LoadBeatmap(),
            _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].difficulty);
        _notesList = _notesGenerator.GeneratedNotes;
        Play();
    }

    private void LoadPlayerData()
    {
        //_playerData = JsonUtility.FromJson<PlayerData>(Resources.Load<TextAsset>($"Levels/levels").text);
        _playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(path));
        for (int i = 0; i < _playerData.levelsList.Count; ++i)
        {
            Debug.Log("Name: " + _playerData.levelsList[i].trackName);
        }
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
        _scoreManager.Glow(_notesList);

        if (_notesList.Count == 0 && EndGameMenu.Ended == false)
        {
            int prevScore = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].score;
            int prevStars = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].stars;
            int difficulty = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].difficulty;
            if (prevScore < _scoreManager.TotalScore)
                _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].score = _scoreManager.TotalScore;

            int stars = _scoreManager.GetStars(difficulty);
            Debug.Log("Difficulty" + difficulty);


            if (prevStars < stars)
            {
                _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].stars = stars;
                _playerData.starsCounter += stars - prevStars;
            }


            string saveData = JsonUtility.ToJson(_playerData);
            File.WriteAllText(path, saveData);
            PlayerPrefs.DeleteKey("levelIndex");
            EndGameMenu.EndGame(_scoreManager.TotalScore, _scoreManager.CombosLost, stars);
        }
    }


    public void OnNewShape()
    {
        string drawnShape = _shapeRecognizer.Shape;
        List<Note> notesToCheck = _notesList
            .Where(note => note.Type == drawnShape)
            .Where(note => note.Hit == false)
            .Where(note => _scoreManager.IsInAnyScoreZone(note.GetPosition()) != null)
            .ToList();

        if (notesToCheck.Count > 0)
            if (_scoreManager.UpdateScore(notesToCheck[0].GetPosition()))
            {
                notesToCheck[0].Hit = true;
                _audioSource.PlayOneShot(_drumHitClip);
                onNoteHit.Invoke();
            }
    }
}