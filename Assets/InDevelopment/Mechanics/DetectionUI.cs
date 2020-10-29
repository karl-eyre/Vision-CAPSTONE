using System;
using InDevelopment.Alex;
using UnityEngine;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class DetectionUI : MonoBehaviour
    {
        public Slider detectionMeter;

        private EnemyStateBase stateBase;
        private LineOfSight.LineOfSight lineOfSight;

        private void Start()
        {
            lineOfSight = GetComponent<LineOfSight.LineOfSight>();
            stateBase = GetComponentInChildren<EnemyStateBase>();
        }

        private void Update()
        {
            SetSliderValue();
            SetSliderVisiblity();
        }

        private void SetSliderVisiblity()
        {
            if (stateBase.isAlerted)
            {
                detectionMeter.gameObject.SetActive(true);
            }
            else
            {
                detectionMeter.gameObject.SetActive(false);
            }
        }


        private void SetSliderValue()
        {
            detectionMeter.value = lineOfSight.detectionMeter;
        }
    }
}
