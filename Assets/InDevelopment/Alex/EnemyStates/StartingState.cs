using System;
using System.Collections;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class StartingState : EnemyStateBase
    {
        [Tooltip(
            "This will determine how long it takes to go back to its previous state if it was interrupted by other functions.")]
        public float waitTime = 2f;

        
        public bool stayStationary;
        
        public override void Enter()
        {
            base.Enter();
            if (stayStationary)
            {
                StartCoroutine(waitForSec(enemyController.stationaryEnemyState));
            }
            else
            {
                StartCoroutine(waitForSec(enemyController.patrollingEnemyState));
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
        }
        
        IEnumerator waitForSec(EnemyStateBase state)
        {
            yield return new WaitForSeconds(waitTime);

            stateManager.ChangeState(state);
            
        }
    }
}
