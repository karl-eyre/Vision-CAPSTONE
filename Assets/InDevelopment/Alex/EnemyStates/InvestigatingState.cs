using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingState : StateBase
    {
        public EnemyModel enemyModel;
        public WaitingAtPointState waitingAtPointState;
        public override void Enter()
        {
            base.Enter();
            Debug.Log("I am in the " + this.GetType() + " state.");
            
        }

        public override void Exit()
        {
            base.Exit();
            
            
        }

        public override void Execute()
        {
            base.Execute();
            //figure out how to get investigation threshold to work
            enemyModel.investigating = true;

            if (!enemyModel.IsDetecting())
            {
                enemyModel.MoveToTarget(enemyModel.targetLastKnownPos);
                if (enemyModel.CheckDistance(enemyModel.targetLastKnownPos))
                {
                    stateManager.ChangeState(waitingAtPointState);
                }
            }
            
        }
    }
}
