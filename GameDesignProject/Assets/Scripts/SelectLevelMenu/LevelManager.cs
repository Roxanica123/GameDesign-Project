using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    //public static LevelManager instance;
    //public GameObject[] levelSelectionPanels;
    //public Text[] starMilestonesText;
    public Text[] lockedStarsText;
    public LevelSelection[] levelSelections;

    public Text starCount;
    //private List<Level> _levels;

    public int stars;
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
        public int difficulty;
    }

    [Serializable]
    private class PlayerData
    {
        public List<Level> levelsList;
        public int starsCounter;
        public int difficulty;
    }

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    //DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    if (instance != this)
        //    {
        //        Destroy(gameObject);
        //    }
        //}
        //DontDestroyOnLoad(gameObject);
        //stars = JsonUtility.FromJson<StarsCounter>(Resources.Load<TextAsset>($"Levels/levels").text).starsCounter;
        //_levels = JsonUtility.FromJson<LevelsContainer>(Resources.Load<TextAsset>($"Levels/levels").text).levelsList;
        path = Application.persistentDataPath + "/GameDesignProject/savefiles/savefile.json";
        CreateUpdateSaveFile();
        _playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(path));
        stars = _playerData.starsCounter;
    }

    private void Start()
    {
        
        for (int i = 0; i < _playerData.levelsList.Count; ++i)
        {
            levelSelections[i].levelIndex = _playerData.levelsList[i].index;
            levelSelections[i].lockLevelName.text = _playerData.levelsList[i].trackName;
            levelSelections[i].unlockLevelName.text = _playerData.levelsList[i].trackName;
            levelSelections[i].CurrBestScore = _playerData.levelsList[i].score;
            levelSelections[i].CurrStarRating = _playerData.levelsList[i].stars;
        }
    }

    private void Update()
    {
        UpdateStarCountUI();
        UpdateLockedStarsUI();
    }

    private void CreateUpdateSaveFile()
    {
        PlayerData templateData = JsonUtility.FromJson<PlayerData>(Resources.Load<TextAsset>($"Levels/levels").text);
        if (File.Exists(path))
        {
            PlayerData currentData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(path));
            for (int i = 0; i < currentData.levelsList.Count; ++i)
            {
                currentData.levelsList[i].trackName = templateData.levelsList[i].trackName;
                currentData.levelsList[i].filename = templateData.levelsList[i].filename;
                currentData.levelsList[i].difficulty = templateData.levelsList[i].difficulty;
            }

            if (currentData.levelsList.Count < templateData.levelsList.Count)
            {
                int diff = templateData.levelsList.Count - currentData.levelsList.Count;
                for (int i = currentData.levelsList.Count; i < currentData.levelsList.Count + diff; ++i)
                {
                    currentData.levelsList.Add(templateData.levelsList[i]);
                }
            }

            string saveData = JsonUtility.ToJson(currentData);
            File.WriteAllText(path, saveData);
        }
        else
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/GameDesignProject/savefiles");
            String saveData = JsonUtility.ToJson(templateData);
            Debug.Log(saveData);
            File.AppendAllText(path, saveData);
        }
    }

    private void UpdateLockedStarsUI()
    {
        for (int i = 0; i < levelSelections.Length; ++i)
        {
            if (levelSelections[i].isUnlocked == false)
            {
                lockedStarsText[i].text = (levelSelections[i].minimumStars - _playerData.starsCounter).ToString();
            }
        }
    }

    private void UpdateStarCountUI()
    {
        starCount.text = _playerData.starsCounter.ToString();
    }

    public void SelectLevel(int _levelIndex) //MARKER this method is triggered when pressing one of the level buttons
    {
        if (levelSelections[_levelIndex].isUnlocked == true)
        {
            PlayerPrefs.SetInt("levelIndex", _levelIndex);
            //PlayerPrefs.SetInt("currLevelScore", levelSelections[_levelIndex].CurrBestScore);
            //PlayerPrefs.SetInt("currStars", levelSelections[_levelIndex].CurrStarRating);
            Debug.Log(PlayerPrefs.GetInt("levelIndex"));
            SceneManager.LoadScene("Scene");
        }
        else
        {
            Debug.Log("You cannot play this level yet! Please work hard to collect more stars! :)");
        }
    }
}