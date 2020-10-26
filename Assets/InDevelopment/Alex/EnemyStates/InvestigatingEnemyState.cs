﻿using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingEnemyState : EnemyStateBase
    {
        // [HideInInspector]
        public Vector3 target;

        private float distanceThreshold = 0.8f;

        public override void Enter()
        {
            base.Enter();
            target = lastKnownPlayerPosition;
            // Debug.Log("I am in the " + this.GetType() + " state.");
        }

        public override void Exit()
        {
            base.Exit();
            lineOfSight.SoftResetLos();
            
        }

        public override void Execute()
        {
            base.Execute();
            //Move to last know player position if you lose player otherwise slowing move towards while continuing to detect player
            //once player is lost wait at last know pos
            //TODO only do if check in enemy controller doesn't work, put check in to see if pos is on nav mesh otherwise assign target as closest point

            if (CanSeePlayer())
            {
                target = lastKnownPlayerPosition;
                enemyController.MoveToTarget(target);
            }
            else
            {
                enemyController.MoveToTarget(target);
                if (Vector2.Distance(new Vector2(transform.position.x,transform.position.z), new Vector2(target.x,target.z)) < distanceThreshold && stateManager.currentEnemyState != enemyController.waitingAtPointEnemyState)
                {
                    stateManager.ChangeState(enemyController.waitingAtPointEnemyState);
                }
            }
        }
    }
}