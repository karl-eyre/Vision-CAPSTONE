using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class TempAbilitiesUI : MonoBehaviour
    {

        [Header("Teleport")]
        public Image TeleportUI;
        private float cooldown1 = 5;
        bool isCooldown = false;

        private TeleportAbility.TeleportAbility teleportAbility;
        
        // Start is called before the first frame update
        void Start()
        {
            teleportAbility = GetComponentInParent<TeleportAbility.TeleportAbility>();
            TeleportAbility.TeleportAbility.teleportTrigger += TeleportTrigger;
            TeleportUI.fillAmount = teleportAbility.teleportDelay;
        }

        // Update is called once per frame
        void Update()
        {
            RefillUI();
        }

        public void TeleportTrigger()
        {
            isCooldown = true;
            TeleportUI.fillAmount = 0;
        }

        void RefillUI()
        {
            if (isCooldown)
            {
                TeleportUI.fillAmount += 1 / cooldown1 * Time.deltaTime;

                if(TeleportUI.fillAmount <= 0)
                {
                    TeleportUI.fillAmount = 0;
                    isCooldown = false;
                }
            }

        }
                
        private void OnEnable()
        {
            TeleportAbility.TeleportAbility.teleportTrigger += TeleportTrigger;
        }

        private void OnDisable()
        {
            TeleportAbility.TeleportAbility.teleportTrigger -= TeleportTrigger;
        }
    }
}
