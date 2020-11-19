using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InDevelopment.Mechanics.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance = null;

        public event Action pauseGame;

        public GameControls controls;

        public GameObject pauseMenu;
        private bool paused;
        public GameObject optionsMenu;
        public GameObject pauseBackground;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            pauseMenu.SetActive(false);
            pauseBackground.SetActive(false);
        }

        private void Start()
        {
            SetupControls();
        }

        private void SetupControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.Menu.OpenPauseMenu.performed += PauseGame;
        }

        private void PauseGame(InputAction.CallbackContext obj)
        {
            if (!paused)
            {
                paused = true;
                pauseMenu.SetActive(true);
                pauseBackground.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                pauseGame?.Invoke();
                Time.timeScale = 0;
            }
        }

        public void UnPauseGame()
        {
            pauseMenu.SetActive(false);
            pauseBackground.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            pauseGame?.Invoke();
            paused = false;
            Time.timeScale = 1;
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("We Finished");
        }

        public void MainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu2");            
        }

        public void OpenOptionsMenu()
        {
            optionsMenu.SetActive(true);
        }

        public void CloseOptionsMenu()
        {
            optionsMenu.SetActive(false);
        }
    }
}