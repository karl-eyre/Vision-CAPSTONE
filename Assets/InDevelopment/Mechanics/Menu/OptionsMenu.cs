using System;
using InDevelopment.Mechanics.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InDevelopment.Mechanics.Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        private MouseLook mouseSettings;
        public bool mouseYInvert;
        
        public void Start()
        {
            mouseSettings = FindObjectOfType<MouseLook>();
            SetUpOptions();
        }

        private void SetUpOptions()
        {
            if (PlayerPrefs.HasKey("MouseSen"))
            {
                mouseSettings.mouseSensitivity = PlayerPrefs.GetFloat("MouseSen");
            }
            if (PlayerPrefs.HasKey("MouseYInvert"))
            {
                mouseSettings.mouseSensitivity = PlayerPrefs.GetInt("MouseYInvert");
            }
        }

        public void SetMouseSensitivity(float changedValue)
        {
            mouseSettings.mouseSensitivity = changedValue;
        }
    }
}
