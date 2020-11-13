using System;
using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;

namespace InDevelopment.Mechanics.Menu
{
    public class MenuCinemachine : MonoBehaviour
    {
        public GameControls controls;
        public CinemachineVirtualCamera activeCam;
        public CinemachineVirtualCamera mainCam;
        public float gameStartDelay = 2;

        // Start is called before the first frame update
        void Awake()
        {
            mainCam = activeCam;
            activeCam.Priority++;
            SetupControls();
        }

        private void SetupControls()
        {
            controls = new GameControls();
            controls.Enable();
            controls.Menu.OpenPauseMenu.performed += Escape;
        }

        public void LoadScene(string targetScene)
        {
            StartCoroutine(startgame(gameStartDelay, targetScene));
        }

        IEnumerator startgame(float seconds, string targetScene)
        {
            yield return new WaitForSeconds(seconds);
            SceneManager.LoadScene(targetScene);
        }

        //return to mainMenu
        public void Escape(InputAction.CallbackContext obj)
        {
            {
                activeCam.Priority--;

                activeCam = mainCam;

                activeCam.Priority++;
            }
        }

        //Change Camera
        public void ChangeCam(CinemachineVirtualCamera target)
        {
            activeCam.Priority--;

            activeCam = target;

            activeCam.Priority++;
        }
    }
}
