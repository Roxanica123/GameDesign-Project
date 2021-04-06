using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    private static GameObject _menuUI;
    private static Text _text;
    public static bool Ended { get; private set; }

    void Start()
    {
        _menuUI = GameObject.Find("EndGameMenuPanel");
        _text = GameObject.Find("EndGameScore").GetComponent<Text>();
        _menuUI.SetActive(false);
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
        SceneManager.LoadScene("Scene");
    }

    public static void EndGame(int score)
    {
        _menuUI.SetActive(true);
        _text.text = _text.text + " " + score;
        Ended = true;
    }
}