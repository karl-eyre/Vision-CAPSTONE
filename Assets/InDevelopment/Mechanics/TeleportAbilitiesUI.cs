using System;
using InDevelopment.Mechanics.Menu;
using UnityEngine;
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

        private TeleportAbility.TeleportAbility teleportAbility;
        
        private bool paused;

        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Slider teleportSlider;

        void Start()
        {
            teleportAbility = GetComponentInParent<TeleportAbility.TeleportAbility>();
            teleportSlider.maxValue = teleportAbility.teleportStartUpDelay;
            // TeleportAbility.TeleportAbility.teleportTrigger += TeleportTrigger;
            // if (!(TeleportUI is null)) TeleportUI.fillAmount = teleportAbility.teleportDelay;
            paused = false;
            MenuManager.instance.pauseGame += () =>  paused = !paused;
        }
        

        // Update is called once per frame
        void Update()
        {
            if (!paused)
            {
                // TeleportUI.gameObject.SetActive(true);
                SetSliderValue();
                spriteRenderer.gameObject.SetActive(true);
            }
            else
            {
                // TeleportUI.gameObject.SetActive(false);
                spriteRenderer.gameObject.SetActive(false);
            }
        }
        
        private void SetSliderValue()
        {
            if (!(teleportSlider is null)) teleportSlider.value = teleportAbility.cooldownTimer;
        }
        
        public void OnValueChange(float changedValue)
        {
            animator.Play("TeleportUIAnim",0,teleportSlider.normalizedValue);
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
