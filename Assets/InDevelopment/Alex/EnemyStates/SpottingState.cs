using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class SpottingState : EnemyStateBase
    {
        //merely stands still and looks at player.
        public override void Enter()
        {
            base.Enter();
            stateManager.interruptedState = stateManager.previousEnemyState;
            posWhenInterrupted = enemyController.transform.position;
        }

        public override void Exit()
        {
            base.Exit();
            posWhenInterrupted = enemyController.transform.position;
        }

        public override void Execute()
        {
            base.Execute();
            LOSFunc();
            if (CanSeePlayer())
            {
                AssignPlayerPos();
                LookAtPlayer();
            }
        }
    }
}