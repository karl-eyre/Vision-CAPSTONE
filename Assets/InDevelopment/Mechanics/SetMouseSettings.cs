using System;
using UnityEngine;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class SetMouseSettings : MonoBehaviour
    {
        public Toggle mouseInvertToggle;
        public Slider mouseSensitivySlider;

        private void Start()
        {
            SetUpOptions();
        }

        private void SetUpOptions()
        {
            if (PlayerPrefs.HasKey("MouseSen"))
            {
                mouseSensitivySlider.value = PlayerPrefs.GetFloat("MouseSen");
            }

            if (PlayerPrefs.HasKey("MouseYInvert"))
            {
                if (PlayerPrefs.GetInt("MouseYInvert") == 0)
                {
                    mouseInvertToggle.isOn = false;
                }

                if (PlayerPrefs.GetInt("MouseYInvert") == 1)
                {
                    mouseInvertToggle.isOn = true;
                }
            }
        }

        
        public void SetMouseSensitivity(float changedValue)
        {
            mouseSensitivySlider.value = changedValue;
            PlayerPrefs.SetFloat("MouseSen", changedValue);
        }

        public void SetMouseYInversion(bool changedValue)
        {
            mouseInvertToggle.isOn = changedValue;
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
