using System;
using System.Collections;
using InDevelopment.Mechanics.TeleportAbility;
using InDevelopment.Mechanics.VisionAbility;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace InDevelopment.PostProcessing
{
    public class PostProcessing : MonoBehaviour
    {
        [SerializeField]
        public Volume volume;
        [SerializeField]
        public Animator animator;
        
        public Vignette vignette;
        public LensDistortion lensDistortion;
        public float vignetteIntensity = 0.6f;
        public float lensDistortionIntensity = 1;
        public bool applyTeleportPostProcessing;


        public ChromaticAberration chromaticAberration;
        public bool applyVisionPostProcessing;
        public float chromaticAberrationIntensity;
        // private static readonly int VisionPostProcessingOn = Animator.StringToHash("VisionPostProcessingOn");
        // private static readonly int TeleportPostProcessingOn = Animator.StringToHash("TeleportPostProcessingOn");

        private void Awake()
        {
            animator = null;
        }

        void Start()
        {
            SetUpPostProcessing();
            animator = GetComponent<Animator>();
            applyTeleportPostProcessing = false;
            applyVisionPostProcessing = false;
            TeleportAbility.teleportStarted += TurnOnPostProcessing;
            TeleportAbility.teleportTriggered += TurnOffPostProcessing;
            VisionAbilityController.visionActivation += TurnVisionPostProcessingOn;
        }

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            animator = GetComponent<Animator>();
        }

        private void TurnVisionPostProcessingOn()
        {
            applyVisionPostProcessing = !applyVisionPostProcessing;
            animator?.SetBool("VisionPostProcessingOn", applyVisionPostProcessing);
        }

        private void Update()
        {
            if (applyTeleportPostProcessing)
            {
                if (vignette != null)
                {
                    vignette.intensity.SetValue(new NoInterpFloatParameter(vignetteIntensity, true));
                }
                if (lensDistortion != null)
                {
                    lensDistortion.intensity.SetValue(new NoInterpFloatParameter(lensDistortionIntensity, true));
                }
            }
            // if (applyVisionPostProcessing)
            // {
                if (vignette != null)
                {
                    vignette.intensity.SetValue(new NoInterpFloatParameter(vignetteIntensity, true));
                }
                if (chromaticAberration != null)
                {
                    chromaticAberration.intensity.SetValue(new NoInterpFloatParameter(chromaticAberrationIntensity, true));
                }
            // }
        }

        private void SetUpPostProcessing()
        {
            volume = GetComponent<Volume>();
            if (!(volume is null))
            {
                volume.profile.TryGet(out vignette);
                volume.profile.TryGet(out lensDistortion);
                volume.profile.TryGet(out chromaticAberration);
            }
        }

        private void TurnOnPostProcessing()
        {
            animator?.SetBool("TeleportPostProcessingOn", true);
            applyTeleportPostProcessing = true;
        }

        private void TurnOffPostProcessing()
        {
            animator?.SetBool("TeleportPostProcessingOn", false);
            StartCoroutine(StopPostProcessing());
        }

        public IEnumerator StopPostProcessing()
        {
           yield return new WaitForSeconds(1);
           applyTeleportPostProcessing = false;
        }
    }
}