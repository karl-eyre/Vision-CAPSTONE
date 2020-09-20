using System;
using System.Collections;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class StationaryEnemyState : EnemyStateBase
    {
        private EnemyController _enemyController;


        private void Start()
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
            Debug.Log("StationaryState: Active.");
            
        }

        IEnumerator waitForSec()
        {
            yield return new WaitForSeconds(3f);
            stateManager.ChangeState(_enemyController.patrollingEnemyState);
        }
        
        
    }
}
