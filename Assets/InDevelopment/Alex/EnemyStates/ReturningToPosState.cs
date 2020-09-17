using InDevelopment.Mechanics.Enemy;

namespace InDevelopment.Alex.EnemyStates
{
    public class ReturningToPosState : StateBase
    {
        public EnemyModel enemyModel;

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
                enemyModel.MoveToTarget(enemyModel.previousPos);
                if (enemyModel.CheckDistance(enemyModel.previousPos))
                {
                    //enemyModel.states = enemyModel.previousState;
                    stateManager.ChangeState(stateManager.previousState);
                }
            }
        }
    }
}
