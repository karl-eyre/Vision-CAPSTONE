using System;
using Cinemachine;
using UnityEngine;
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

        // Start is called before the first frame update
        void Awake()
        {
            mainCam = activeCam;
            activeCam.Priority++;
        }

        private void Update()
        {
            if(Keyboard.current.escapeKey.isPressed)
            {
                
            }
        }

        public void LoadAnimaticIntro()
        {
            SceneManager.LoadScene("Animatic_start");
        }


        //return to mainMenu
        public void Escape(InputAction.CallbackContext obj)
        {
            activeCam.Priority--;

            activeCam = mainCam;

            activeCam.Priority++;
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
