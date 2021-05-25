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
        SceneManager.LoadScene("Scene");
    }

    public static void EndGame(int score, int combosLost, int stars)
    {
        _menuUI.SetActive(true);
        _text.text = _text.text + " " + score;
        _combos.text = _combos.text + " " + combosLost + " -> " + stars + " stars";
        Ended = true;
    }
}