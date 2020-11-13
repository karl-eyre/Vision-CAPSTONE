﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace InDevelopment.Mechanics
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance = null;

        public event Action pauseGame;
        
        public GameControls controls;

        public GameObject pauseMenu;
        private bool paused;
        public GameObject optionsMenu;
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
                Cursor.lockState = CursorLockMode.Confined;
                pauseGame?.Invoke();
                //pause time here
            }
            
        }

        public void UnPauseGame()
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            pauseGame?.Invoke();
            paused = false;
            //unpause time here
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
