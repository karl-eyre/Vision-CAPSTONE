using System;
using Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace InDevelopment.Mechanics.VisionAbility
{
    public class VisionAbilityController : AbilityBase
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

        [SerializeField]
        private Volume postProcessing;

        private static float t1;
        private static float t2;

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


            // t1 = Mathf.Lerp(1, 0, 0.1f);
            // t2 = Mathf.Lerp(0, 1, 0.1f);
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

                //turning off post processing
                // cb.intensity.value = t2;
                // v.intensity.value = t2;
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
                    //turning post processing on
                    // cb.intensity.value = t1;
                    // v.intensity.value = t1;
                }
            }
            else
            {
                visionAbilityDuration = 0f;
            }
        }

        private void CallActivationEvent()
        {
            visionActivation?.Invoke();
        }

        private void CallEndEvent()
        {
            visionEnded?.Invoke();
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
