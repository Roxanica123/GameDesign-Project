using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
    private Queue<float> _beatmapTimings;
    private PlayerData _playerData;
    private string path;

    [Serializable]
    private class Level
    {
        public int index;
        public string trackName;
        public string filename;
        public int score;
        public int stars;
    }

    [Serializable]
    private class PlayerData
    {
        public List<Level> levelsList;
        public int starsCounter;
    }


    private float[] LoadBeatmap()
    {
        string filename = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].filename;
        var audioClip = Resources.Load<AudioClip>($"Sound/{filename}");
        _audioSource = gameObject.GetComponent<AudioSource>();
        _audioSource.clip = audioClip;

        return JsonUtility.FromJson<BeatmapFile>(Resources.Load<TextAsset>($"Beatmaps/{filename}").text).times;
    }

    public void Play() => _audioSource.Play();
    public void Pause() => _audioSource.Pause();

    void Start()
    {
        path = Application.persistentDataPath + "/GameDesignProject/savefiles/savefile.json";
        Debug.Log("Received index: " + PlayerPrefs.GetInt("levelIndex"));
        LoadPlayerData();
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


        if (_notesList.Count == 0 && EndGameMenu.Ended == false)
        {
            int prevScore = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].score;
            int prevStars = _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].stars;
            if (prevScore < _scoreManager.TotalScore)
                _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].score = _scoreManager.TotalScore;

            // Sectiune de test pentru update-ul sprite-ului de la star rating; De modificat conform logicii dorite
            int stars = 3;
            if (prevStars < stars)
            {
                _playerData.levelsList[PlayerPrefs.GetInt("levelIndex")].stars = stars;
                _playerData.starsCounter += stars - prevStars;
            }
                

            string saveData = JsonUtility.ToJson(_playerData);
            File.WriteAllText(path, saveData);
            PlayerPrefs.DeleteKey("levelIndex");
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