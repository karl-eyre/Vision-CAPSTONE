using System;
using InDevelopment.Mechanics.Menu;
using InDevelopment.Mechanics.VisionAbility;
using UnityEngine;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class VisionAbilityUI : MonoBehaviour
    {
        public GameObject visionUI;
        public Slider visionEnergy;
        private float cooldown;
        private bool isCooldown;

        public VisionAbilityController visionAbilityController;

        private bool paused;

        public Animator animator;
        public SpriteRenderer SpriteRenderer;
        public Sprite sprite;

        public void OnValueChange(float changedValue)
        {
            //controls the slider for the vision animation
            animator.Play("VisionUIAnim", 0, visionEnergy.normalizedValue);
        }

        private void Start()
        {
            visionAbilityController = FindObjectOfType<VisionAbilityController>();
            if (!(visionEnergy is null))
                if (!(visionAbilityController is null))
                    visionEnergy.value = visionAbilityController.maxEnergyFill;
            if (!(visionEnergy is null))
                if (!(visionAbilityController is null))
                    visionEnergy.maxValue = visionAbilityController.maxEnergyFill;
            paused = false;
            if (!(MenuManager.instance is null)) MenuManager.pauseGame += Pause;
            if (!(MenuManager.instance is null)) MenuManager.unpauseGame += SetEnergy;
        }

        private void SetEnergy()
        {
            if (visionAbilityController.visionEnergy >= visionAbilityController.maxEnergyFill)
            {
                //due to some weird animation glitch this needs to be set to just before the max because
                //the max for some reason ends up showing the first animation frame again for some reason
                visionAbilityController.visionEnergy = 99f;
            }
        }

        private void Pause()
        {
            paused = !paused;
        }

        private void FixedUpdate()
        {
            if (!paused)
            {
                SetSliderValue();
            }
        }

        private void SetSliderValue()
        {
            if (!(visionEnergy is null))
            {
                if (!(visionAbilityController is null))
                {
                    visionEnergy.value = visionAbilityController.visionEnergy;
                }
            }
        }
    }
}