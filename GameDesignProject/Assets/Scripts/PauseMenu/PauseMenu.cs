using UnityEngine;
using UnityEngine.SceneManagement;

namespace PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public bool IsGamePaused { get; private set; }
        private GameObject _menuUI;

        void Start()
        {
            IsGamePaused = false;
            _menuUI = GameObject.Find("PauseMenuPanel");
            _menuUI.SetActive(false);
        }


        public void Pause()
        {
            _menuUI.SetActive(true);
            Time.timeScale = 0f;
            IsGamePaused = true;
        }

        public void Resume()
        {
            _menuUI.SetActive(false);
            Time.timeScale = 1f;
            IsGamePaused = false;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
            Debug.Log("Loaded menu");
        }

        public void QuitGame()
        {
            Debug.Log("Quit game");
            Application.Quit();
        }
    }
}