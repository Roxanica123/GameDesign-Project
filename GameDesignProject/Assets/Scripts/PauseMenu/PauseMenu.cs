using UnityEngine;
using UnityEngine.SceneManagement;

namespace PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public bool IsGamePaused { get; private set; }
        private FiguresManager _figuremanager;
        private GameObject _menuUI;

        void Start()
        {
            IsGamePaused = false;
            _menuUI = GameObject.Find("PauseMenuPanel");
            _menuUI.SetActive(false);
            _figuremanager = GameObject.Find("GameManager").GetComponent<FiguresManager>();
        }


        public void Pause()
        {
            _menuUI.SetActive(true);
            _figuremanager.Pause();
            IsGamePaused = true;
        }

        public void Resume()
        {
            _menuUI.SetActive(false);
            _figuremanager.Play();
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