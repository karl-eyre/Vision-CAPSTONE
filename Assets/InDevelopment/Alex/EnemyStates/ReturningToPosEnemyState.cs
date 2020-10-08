using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class ReturningToPosEnemyState : EnemyStateBase
    {
        public Vector3 target;


        public override void Enter()
        {
            base.Enter();
            
            
            target = posWhenInterrupted;
        }

        public override void Exit()
        {
            base.Exit();
            lineOfSight.SoftResetLos();
        }

        public override void Execute()
        {
            //should return to previous duty, so if they were patrolling return to patroll route, is stationary
            //return to stationary position
            base.Execute();
            // LOSFunc();
            enemyController.MoveToTarget(target);
            if (Vector3.Distance(transform.position, target) < 0.5f)
            {
                stateManager.ChangeState(stateManager.initialState);
            }
        }
    }
}