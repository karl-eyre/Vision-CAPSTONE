using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace InDevelopment.Mechanics
{
    public class MenuManager : MonoBehaviour
    {
        public GameControls controls;

        private void Start()
        {
            SetupControls();
        }

        private void SetupControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.Menu.OpenPauseMenu.performed += Escape;
        }
        
        public void Escape(InputAction.CallbackContext obj)
        {
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                ExitGame();
            }
            else
            {
                MainMenu();
            }
        }

        public void ExitGame()
        {        
            Application.Quit();
            Debug.Log("We Finished");
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Credits()
        {
            SceneManager.LoadScene("Credits");
        }

        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        public void StartGame()
        {
            SceneManager.LoadScene("2_BuildingLevel");
        }
    }
}
