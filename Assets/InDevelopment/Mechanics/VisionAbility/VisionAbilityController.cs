using System;
using Abilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


namespace InDevelopment.Mechanics.VisionAbility
{
    public class VisionAbilityController : AbilityBase
    {
        [SerializeField]
        private bool isActive;

        [SerializeField]
        private float visionAbilityDuration;

        public float visionEnergy = 100f;
        public float abilityUseCost = 25f;
        public float visionEnergyFillRate = 5f;
        public float maxEnergyFill = 100f;
        public float maxVisionAbilityDuration = 3f;
        public static event Action visionActivation;

        [SerializeField]
        private Volume postProcessing;

        private ChromaticAberration cb;
        private Vignette v;
        
        private GameControls controls;

        private static float t1;
        private static float t2;

        private void Awake()
        {
            GetReferences();
        }

        private void Start()
        {
            controls = new GameControls();
            controls.Enable();
            controls.InGame.VisionAbilityActivation.performed += UseVisionAbilityInput;
        }

        private void GetReferences()
        {
            postProcessing.profile.TryGet(out cb);
            postProcessing.profile.TryGet(out v);
            cb.intensity.value = 0f;
            v.intensity.value = 0f;    
        }

        public void FixedUpdate()
        {
            ReduceTime();
            if (!isActive)
            {
                RefillEnergy();
            }
            
            // t1 = Mathf.Lerp(1, 0, 0.1f);
            // t2 = Mathf.Lerp(0, 1, 0.1f);
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
                CallEvent();
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

        private void UseVisionAbilityInput(InputAction.CallbackContext context)
        {
            UseVisionAbility();
        }

        private void UseVisionAbility()
        {
            if (visionEnergy > abilityUseCost)
            {
                visionAbilityDuration = 0f;
                if (!isActive)
                {
                    isActive = true;
                    visionAbilityDuration = maxVisionAbilityDuration;
                    visionEnergy -= abilityUseCost;
                    CallEvent();
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