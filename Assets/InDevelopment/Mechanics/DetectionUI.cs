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
            if (!(stateBase is null) && stateBase.isAlerted)
            {
                if (!(detectionMeter is null)) detectionMeter.gameObject.SetActive(true);
            }
            else
            {
                if (!(detectionMeter is null)) detectionMeter.gameObject.SetActive(false);
            }
        }


        private void SetSliderValue()
        {
            if (!(detectionMeter is null)) detectionMeter.value = lineOfSight.detectionMeter;
        }
    }
}
