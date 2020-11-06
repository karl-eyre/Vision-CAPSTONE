using System;
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
        
        private void Start()
        {
            visionAbilityController = GetComponentInParent<VisionAbilityController>();
            if (!(visionEnergy is null)) visionEnergy.value = visionAbilityController.maxEnergyFill;
            if (!(visionEnergy is null)) visionEnergy.maxValue = visionAbilityController.maxEnergyFill;
        }

        private void Update()
        {
            SetSliderValue();
        }

        private void SetSliderValue()
        {
            if (!(visionEnergy is null)) visionEnergy.value = visionAbilityController.visionEnergy;
        }
    }
}
