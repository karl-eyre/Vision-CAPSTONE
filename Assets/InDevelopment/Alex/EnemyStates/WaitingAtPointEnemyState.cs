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

        IEnumerator waitForSec()
        {
            yield return new WaitForSeconds(waitTimer);
            stateManager.ChangeState(stateManager.previousEnemyState);
        }
        
    }
}
