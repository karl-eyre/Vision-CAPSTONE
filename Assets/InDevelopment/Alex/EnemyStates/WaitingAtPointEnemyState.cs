using System;
using System.Collections;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class WaitingAtPointEnemyState : EnemyStateBase
    {
        public float waitTimer = 1.0f;
        public float maxLandRturn = 60;
        public float rotSpeed = 5;
        private EnemyController _enemyController;

        public void Start()
        {
            _enemyController = GetComponentInParent<EnemyController>();
        }

        public override void Enter()
        {
            base.Enter();
            StartCoroutine(waitForSec());
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
            _enemyController.LookLeftAndRight();
        }

        // private void LookLeftAndRight()
        // {
        //     //Quaternion parentRotation = _enemyController.transform.rotation;
        //     _enemyController.transform.rotation = Quaternion.Euler(0f, maxLandRturn * Mathf.Sin(Time.time * rotSpeed), 0f);
        //    //parentRotation = Quaternion.Euler(parentRotation.x, parentRotation.y * Mathf.Sin(Time.time * rotSpeed), parentRotation.z);
        // }
        
        IEnumerator waitForSec()
        {
            yield return new WaitForSeconds(waitTimer);
            stateManager.ChangeState(stateManager.previousEnemyState);
        }
        
    }
}
