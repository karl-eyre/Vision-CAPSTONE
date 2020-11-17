using System;
using InDevelopment.Mechanics.Menu;
using InDevelopment.Mechanics.VisionAbility;
using UnityEngine;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class VisionAbilityUI : MonoBehaviour
    {
        public Slider visionEnergy;
        private float cooldown;
        private bool isCooldown;

        private VisionAbilityController visionAbilityController;

        private bool paused;
        private void Start()
        {
            visionAbilityController = GetComponentInParent<VisionAbilityController>();
            if (!(visionEnergy is null)) visionEnergy.value = visionAbilityController.maxEnergyFill;
            if (!(visionEnergy is null)) visionEnergy.maxValue = visionAbilityController.maxEnergyFill;
            paused = false;
            MenuManager.instance.pauseGame += () =>  paused = !paused;
        }

        private void Update()
        {
            if (!paused)
            {
                SetSliderValue();
                visionEnergy.gameObject.SetActive(true);
            }
            else
            {
                visionEnergy.gameObject.SetActive(false);
            }
            
        }

        private void SetSliderValue()
        {
            if (!(visionEnergy is null)) visionEnergy.value = visionAbilityController.visionEnergy;
        }
    }
}