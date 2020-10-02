﻿using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingEnemyState : EnemyStateBase
    {
        private Transform target;
        
        public override void Enter()
        {
            base.Enter();
            target = lastKnownPlayerPosition;
            Debug.Log("I am in the " + this.GetType() + " state.");
        }

        public override void Exit()
        {
            base.Exit();
            if (!lineOfSight.isDetecting)
            {
                lineOfSight.detectionMeter = lineOfSight.investigationThreshold - 5;    
            }
        }

        public override void Execute()
        {
            //Move to the investigation point?
            //LookLeftAndRight + waitAtPoint
            if (target)
            {

                if (lineOfSight.isDetecting)
                {
                    enemyController.MoveToTarget(target.position);
                }
                else
                {
                    stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }
                
                
                if (enemyController.patrollingEnemyState.IsReached())
                {
                    enemyController.LookLeftAndRight();
                    
                    //stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }
            }
            base.Execute();
        }
    }
}
