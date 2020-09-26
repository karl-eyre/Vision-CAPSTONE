using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingEnemyState : EnemyStateBase
    {
        private Transform target;
        
        public override void Enter()
        {
            base.Enter();
            target = lineOfSight.player.transform;
            Debug.Log("I am in the " + this.GetType() + " state.");
        }

        public override void Exit()
        {
            base.Exit();
            if (!lineOfSight.isDetecting)
            {
                lineOfSight.detectionMeter = lineOfSight.detectionThreshold - 5;    
            }
        }

        public override void Execute()
        {
            //Move to the investigation point?
            //LookLeftAndRight + waitAtPoint
            if (target)
            {
                enemyController.MoveToTarget(target.position);
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
