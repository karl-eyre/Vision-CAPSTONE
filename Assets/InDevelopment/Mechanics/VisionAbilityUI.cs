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
            visionEnergy.value = visionAbilityController.maxEnergyFill;
        }

        private void Update()
        {
            SetSliderValue();
        }

        private void SetSliderValue()
        {
            visionEnergy.value = visionAbilityController.visionEnergy;
        }
    }
}
