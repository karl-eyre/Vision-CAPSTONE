using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class PatrollingState : StateBase
    {
        public EnemyModel enemyModel;
        public WaitingAtPointState waitingAtPointState;
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
            if (!enemyModel.IsDetecting())
            {
                enemyModel.InvestigationTrigger();
                if (enemyModel.waypoints != null) enemyModel.targetPosition = enemyModel.waypoints[enemyModel.currentIndex].transform.position;

                enemyModel.MoveToTarget(enemyModel.targetPosition);

                if (enemyModel.CheckDistance(enemyModel.targetPosition))
                {
                    //enemyModel.previousState = EnemyModel.States.patrolling;
                    enemyModel.previousPos = transform.position;
                    stateManager.ChangeState(waitingAtPointState);
                }
            }
            
        }
    }
}
