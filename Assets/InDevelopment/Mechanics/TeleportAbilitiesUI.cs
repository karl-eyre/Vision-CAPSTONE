﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using InDevelopment.Mechanics.TeleportAbility;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class TeleportAbilitiesUI : MonoBehaviour
    {

        [Header("Teleport")]
        public Image TeleportUI;
        private float cooldown1 = 5;
        private bool isCooldown = false;

        private TeleportAbility.TeleportAbility teleportAbility;

        //TODO figure out why teleport references break on scene load
        void Awake()
        {
            teleportAbility = GetComponentInParent<TeleportAbility.TeleportAbility>();
            TeleportAbility.TeleportAbility.teleportTrigger += TeleportTrigger;
            if (!(TeleportUI is null)) TeleportUI.fillAmount = teleportAbility.teleportDelay;
        }
        

        // Update is called once per frame
        void Update()
        {
            RefillUI();
        }

        private void TeleportTrigger()
        {
            isCooldown = true;
            if (!(TeleportUI is null)) TeleportUI.fillAmount = 0;
        }

        private void RefillUI()
        {
            if (isCooldown)
            {
                if (!(TeleportUI is null))
                {
                    TeleportUI.fillAmount += 1 / cooldown1 * Time.deltaTime;

                    if (TeleportUI.fillAmount <= 0)
                    {
                        TeleportUI.fillAmount = 0;
                        isCooldown = false;
                    }
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
        
        private void OnDestroy()
        {
            TeleportAbility.TeleportAbility.teleportTrigger -= TeleportTrigger;
        }
    }
}
