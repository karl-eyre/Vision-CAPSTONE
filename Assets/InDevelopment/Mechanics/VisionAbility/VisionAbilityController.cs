using System;
using Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VisionAbility
{
    public class VisionAbilityController : AbilityBase
    {
        [SerializeField]
        private bool isActive;

        private float visionAbilityDuration;
        public float maxVisionAbilityDuration = 3f;
        public static event Action visionActivation;

        [SerializeField]
        private GameControls controls;

        private void Start()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.VisionAbilityActivation.performed += UseVisionAbility;
        }

        public void FixedUpdate()
        {
            CheckAbility();
        }

        private void CheckAbility()
        {
            if (visionAbilityDuration > 0)
            {
                visionAbilityDuration -= Time.deltaTime;
            }

            if (visionAbilityDuration < 0)
            {
                visionAbilityDuration = 0;
                CallEvent();
                isActive = false;
            }
        }

        private void UseVisionAbility(InputAction.CallbackContext context)
        {
            if (!isActive)
            {
                isActive = true;
                visionAbilityDuration = maxVisionAbilityDuration;
                CallEvent();
            }
        }

        private void CallEvent()
        {
            visionActivation?.Invoke();
        }

        private void OnEnable()
        {
            if (controls != null) controls.Enable();
        }

        private void OnDisable()
        {
            if (controls != null) controls.Disable();
        }
    }
}