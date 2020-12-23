using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace InDevelopment.Mechanics.VisionAbility
{
    //this reacts to the vision activation script, this sits on the enemys and throwable objects
    public class VisionAbilityController : MonoBehaviour
    {
        private GameControls controls;

        [HideInInspector]
        public bool isActive;

        private float visionAbilityDuration;

        public float visionEnergy = 100f;
        public float abilityUseCost = 25f;
        public float visionEnergyFillRate = 5f;
        public float maxEnergyFill = 100f;
        public float maxVisionAbilityDuration = 3f;
        public float abiltiyUseMultiplier = 10f;

        public static event Action visionActivation;
        public static event Action visionEnded;

        public bool toggleAbility;

        private void Awake()
        {
            SetupControls();
        }

        private void SetupControls()
        {
            controls = new GameControls();
            controls.Enable();
            if (toggleAbility)
            {
                controls.InGame.VisionAbilityActivation.performed += UseVisionAbilityToggleInput;
            }
            else
            {
                controls.InGame.VisionAbilityActivation.started += StartAbility;
                controls.InGame.VisionAbilityActivation.canceled += EndAbility;
            }
        }

        public void FixedUpdate()
        {
            if (toggleAbility)
            {
                ReduceTime();
                if (!isActive)
                {
                    RefillEnergy();
                }
            }
            else
            {
                if (isActive)
                {
                    ReduceEnergy();
                }
                else
                {
                    RefillEnergy();
                }
            }
        }

        private void ReduceEnergy()
        {
            if (visionEnergy > 0)
            {
                visionEnergy -= Time.deltaTime * abiltiyUseMultiplier;
            }
            if(visionEnergy <= 0)
            {
                visionEnergy = 0;
                isActive = false;
                CallActivationEvent();
            }
            // else
            // {
            //     CallActivationEvent();
            // }
        }

        private void ReduceTime()
        {
            if (visionAbilityDuration > 0)
            {
                visionAbilityDuration -= Time.deltaTime;
            }

            if (visionAbilityDuration <= 0 && isActive)
            {
                visionAbilityDuration = 0f;
                CallActivationEvent();
                // CallEndEvent();
                isActive = false;

            }
        }

        private void RefillEnergy()
        {
            if (visionEnergy < maxEnergyFill)
            {
                visionEnergy += Time.deltaTime * visionEnergyFillRate;
            }
            else
            {
                visionEnergy = maxEnergyFill;
            }
        }

        private void UseVisionAbilityToggleInput(InputAction.CallbackContext context)
        {
            UseVisionAbility();
            // CallActivationEvent();
        }

        private void StartAbility(InputAction.CallbackContext context)
        {
            if (!isActive)
            {
                isActive = true;
                CallActivationEvent();
            }
        }

        private void EndAbility(InputAction.CallbackContext context)
        {
            if (isActive)
            {
                CallActivationEvent();
                CallEndEvent();
                isActive = false;
            }
        }

        private void UseVisionAbility()
        {
            if (visionEnergy >= abilityUseCost)
            {
                visionAbilityDuration = 0f;
                if (!isActive)
                {
                    isActive = true;
                    visionAbilityDuration = maxVisionAbilityDuration;
                    visionEnergy -= abilityUseCost;
                    CallActivationEvent();
                }
            }
            else
            {
                visionAbilityDuration = 0f;
            }
        }

        private void CallActivationEvent()
        {
            if (visionActivation != null) visionActivation?.Invoke();
        }

        private void CallEndEvent()
        {
            if (visionEnded != null) visionEnded?.Invoke();
        }

        private void OnEnable()
        {
            if (controls != null) controls.Enable();
        }

        private void OnDestroy()
        {
            if (controls != null) controls.Disable();
        }
    }
}
