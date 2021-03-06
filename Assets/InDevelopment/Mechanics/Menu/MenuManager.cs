﻿using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InDevelopment.Mechanics.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager instance = null;

        public static event Action pauseGame;
        public static event Action unpauseGame;

        public GameControls controls;

        public GameObject pauseMenu;
        private bool paused;
        public GameObject optionsMenu;
        public GameObject pauseBackground;
        public PauseSmoke smokeScript;
        private EventInstance pausedSnapshot;

        public GameObject playerUI;

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
            //pauseBackground.SetActive(false);
            smokeScript = pauseBackground.GetComponent<PauseSmoke>();
        }

        private void Start()
        {
            SetupControls();
            pausedSnapshot = RuntimeManager.CreateInstance("snapshot:/Paused");
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
                pausedSnapshot.start();
                Debug.Log("paused");
                paused = true;
                pauseMenu.SetActive(true);
                Cursor.visible = true;
                playerUI.SetActive(false);
                //pauseBackground.SetActive(false);
                Cursor.lockState = CursorLockMode.Confined;
                pauseGame?.Invoke();
                Time.timeScale = 0;
                smokeScript.fadeSmoke();
            }
        }

        public void UnPauseGame()
        {
            pausedSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            pauseMenu.SetActive(false);
            Cursor.visible = false;
            playerUI.SetActive(true);
            Debug.Log("unpaused");
            //pauseBackground.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            pauseGame?.Invoke();
            unpauseGame?.Invoke();
            paused = false;
            Time.timeScale = 1;
            smokeScript.fadeSmoke();
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

        private void OnDestroy()
        {
            pausedSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}