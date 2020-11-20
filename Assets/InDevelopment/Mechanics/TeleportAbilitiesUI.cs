using System;
using InDevelopment.Mechanics.Menu;
using UnityEngine;
using InDevelopment.Mechanics.TeleportAbility;
using UnityEngine.InputSystem;
using InDevelopment.Mechanics.TeleportAbility;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class TeleportAbilitiesUI : MonoBehaviour
    {
        [Header("Teleport")]
        // public Image TeleportUI;
        private float cooldown1 = 5;

        private bool isCooldown = false;

        public TeleportAbility.TeleportAbility teleportAbility;

        private bool paused;
        public GameObject teleportUI;

        public Animator animator;
        public Slider teleportSlider;

        void Start()
        {
            teleportAbility = FindObjectOfType<TeleportAbility.TeleportAbility>();
            if (!(teleportAbility is null)) teleportSlider.maxValue = teleportAbility.teleportStartUpDelay;
            // TeleportAbility.TeleportAbility.teleportTrigger += TeleportTrigger;
            // if (!(TeleportUI is null)) TeleportUI.fillAmount = teleportAbility.teleportDelay;
            paused = false;
            if (!(MenuManager.instance is null)) MenuManager.pauseGame += () => paused = !paused;
        }

        private void Pause()
        {
            paused = !paused;
        }

        void FixedUpdate()
        {
            if (!paused)
            {
                SetSliderValue();
            }
        }

        private void SetSliderValue()
        {
            if (!(teleportSlider is null)) teleportSlider.value = teleportAbility.cooldownTimer;
        }

        public void OnValueChange(float changedValue)
        {
            animator.Play("TeleportUIAnim", 0, teleportSlider.normalizedValue);
        }

        // private void TeleportTrigger()
        // {
        //     // animator.Play("VisionUIAnim",0,teleportAbility.cooldownTimer);
        // }
        //
        //         
        // private void OnEnable()
        // {
        //     TeleportAbility.TeleportAbility.teleportTrigger += TeleportTrigger;
        // }
        //
        // private void OnDisable()
        // {
        //     TeleportAbility.TeleportAbility.teleportTrigger -= TeleportTrigger;
        // }
        //
        // private void OnDestroy()
        // {
        //     TeleportAbility.TeleportAbility.teleportTrigger -= TeleportTrigger;
        // }
    }
}