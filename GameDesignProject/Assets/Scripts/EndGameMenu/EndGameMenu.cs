using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    private static GameObject _menuUI;
    private static Text _text;
    private static Text _combos;
    private static int _levelIndex;
    public static LevelManager LevelManager { get; set; }
    public static bool Ended { get; private set; }

    void Start()
    {
        _menuUI = GameObject.Find("EndGameMenuPanel");
        _text = GameObject.Find("EndGameScore").GetComponent<Text>();
        _combos = GameObject.Find("CombosLost").GetComponent<Text>();
        _menuUI.SetActive(false);

        Ended = false;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Loaded menu");
    }

    public void PlayAgain()
    {
        Debug.Log(_levelIndex);
        LevelManager.SelectLevel(_levelIndex);
    }

    public static void EndGame(int score, int combosLost, int stars, int levelIndex)
    {
        _menuUI.SetActive(true);
        _text.text = _text.text + " " + score;
        _combos.text = _combos.text + " " + combosLost + " -> " + stars + " stars";
        _levelIndex = levelIndex;
        Ended = true;
    }
}