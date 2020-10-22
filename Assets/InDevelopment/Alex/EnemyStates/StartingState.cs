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

        public Vector3 intialPos;
        private bool isWaiting;

        private void Start()
        {
            intialPos = transform.position;
        }

        public override void Enter()
        {
            base.Enter();
            isWaiting = false;
        }

        public override void Exit()
        {
            base.Exit();
            waitTime = 0.1f;
        }

        public override void Execute()
        {
            base.Execute();
            enemyController.MoveToTarget(intialPos);
            if (Vector3.Distance(transform.position, intialPos) < 0.5f)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    if (stayStationary)
                    {
                        StartCoroutine(waitForSec(enemyController.stationaryEnemyState));
                    }
                    else
                    {
                        StartCoroutine(waitForSec(enemyController.patrollingEnemyState));
                    }
                }
                
            }
        }

        IEnumerator waitForSec(EnemyStateBase state)
        {
            yield return new WaitForSeconds(waitTime);

            stateManager.ChangeState(state);
            isWaiting = false;
        }
    }
}