using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class StationaryEnemyState : EnemyStateBase
    {
        [Tooltip("This will determine how long it takes to go back to its previous state if it was interrupted by other functions.")]
        public float waitTime = 2f;
        public bool stayStationary;


        private void Start()
        {
            
        }

        public override void Enter()
        {
            base.Enter();
            if (stayStationary)
            {
                //StartCoroutine(waitForSec(stateManager.interruptedState));
                //DO NOTHING
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
            if (lineOfSight.isDetecting)
            {
                enemyController.LookAtTarget(lineOfSight.player.transform.position);
            }
            else
            {
                enemyController.LookLeftAndRight();
            }
        }

        IEnumerator waitForSec(EnemyStateBase state)
        {
            yield return new WaitForSeconds(waitTime);

            if (!lineOfSight.isDetecting)
            {
                stateManager.ChangeState(state);
            }
        }
        
        
    }
}
