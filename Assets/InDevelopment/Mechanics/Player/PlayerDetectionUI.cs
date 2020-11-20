using System;
using System.Collections.Generic;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using UnityEngine;
using UnityEngine.UI;
using InDevelopment.Mechanics.Menu;

namespace InDevelopment.Mechanics.Player
{
    public class PlayerDetectionUI : MonoBehaviour
    {
        public float detectionMeter;
        public GameObject detectionMeterUI;

        public Slider detectionSlider;

        public Animator animator;

        private bool paused;
        public LineOfSight.LineOfSight lastEnemy;

        public List<LineOfSight.LineOfSight> enemies = new List<LineOfSight.LineOfSight>();

        private void Start()
        {
            paused = false;
            if (!(MenuManager.instance is null)) MenuManager.pauseGame += Pause;
        }

        private void Pause()
        {
            paused = !paused;
        }
        
        private void FixedUpdate()
        {
            if (enemies != null)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].detectionMeter > detectionMeter)
                    {
                        detectionMeter = enemies[i].detectionMeter;
                        lastEnemy = enemies[i];
                    }
                }
            }

            if (!(lastEnemy is null)) detectionMeter = lastEnemy.detectionMeter;

            if (!paused)
            {
                SetSliderValue();
            }
        }

        private void SetSliderValue()
        {
            if (!(detectionSlider is null)) detectionSlider.value = detectionMeter;
        }

        public void OnValueChange(float changedValue)
        {
            animator.Play("DetectionAnimation", 0, detectionSlider.normalizedValue);
        }
    }
}