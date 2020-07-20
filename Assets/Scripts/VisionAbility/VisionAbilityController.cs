using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VisionAbility
{
    public class VisionAbilityController : MonoBehaviour
    {
        [SerializeField] private bool isActive;
        [SerializeField] private float visionAbilityDuration;
        public float maxVisionAbilityDuration;
        public static event Action visionActivation;

        public void FixedUpdate()
        {
            if (InputSystem.GetDevice<Keyboard>().spaceKey.isPressed)
            {
                UseVisionAbility();
            }

            if (visionAbilityDuration > 0)
            {
                visionAbilityDuration -= Time.deltaTime;
            }
            
            if(visionAbilityDuration < 0)
            {
                visionAbilityDuration = 0;
                CallEvent();
                isActive = false;
            }
        }

        public void UseVisionAbility()
        {
            if (!isActive)
            {
                isActive = true;
                visionAbilityDuration = maxVisionAbilityDuration;
                CallEvent();
            }
        }

        public void CallEvent()
        {
            visionActivation?.Invoke();
        }
    }
}
