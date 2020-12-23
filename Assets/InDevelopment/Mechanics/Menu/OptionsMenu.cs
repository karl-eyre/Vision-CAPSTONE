using System;
using InDevelopment.Mechanics.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InDevelopment.Mechanics.Menu
{
    public class OptionsMenu : MonoBehaviour
    {
        private MouseLook mouseSettings;
        public Slider mouseSensitivitySlider;
        public Toggle mouseYInvertion;
        //public TMP_Text mouseSensitivityText;

        public void Start()
        {
            mouseSettings = FindObjectOfType<MouseLook>();
            SetUpOptions();
        }

        private void SetUpOptions()
        {
            //checking for previously saved settings
            if (PlayerPrefs.HasKey("MouseSen"))
            {
                mouseSettings.mouseSensitivity = PlayerPrefs.GetFloat("MouseSen");
                //mouseSensitivityText.text = mouseSettings.mouseSensitivity.ToString();
                mouseSensitivitySlider.value = mouseSettings.mouseSensitivity;
            }

            if (PlayerPrefs.HasKey("MouseYInvert"))
            {
                if (PlayerPrefs.GetInt("MouseYInvert") == 0)
                {
                    mouseSettings.InvertMouseY = false;
                    mouseYInvertion.isOn = mouseSettings.InvertMouseY;
                }

                if (PlayerPrefs.GetInt("MouseYInvert") == 1)
                {
                    mouseSettings.InvertMouseY = true;
                    mouseYInvertion.isOn = mouseSettings.InvertMouseY;
                }
            }
        }

        public void SetMouseSensitivity(float changedValue)
        {
            //saves the value the slider gives
            mouseSettings.mouseSensitivity = changedValue;
            //mouseSensitivityText.text = changedValue.ToString();
            PlayerPrefs.SetFloat("MouseSen", changedValue);
        }

        public void SetMouseYInversion(bool changedValue)
        {
            //saves the settings the toggle gives
            mouseSettings.InvertMouseY = changedValue;
            if (changedValue)
            {
                PlayerPrefs.SetInt("MouseYInvert", 1);
            }
            else
            {
                PlayerPrefs.SetInt("MouseYInvert", 0);
            }
        }
    }
}