using InDevelopment.Mechanics.Enemy;

namespace InDevelopment.Alex.EnemyStates
{
    public class StationaryState : StateBase
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
                transform.position = enemyModel.previousPos;
                transform.rotation = enemyModel.previousRot;
                enemyModel.LookLeftAndRight();
            }

            //put in if statement that checks are you in your original spot if not move there
            //enemyModel.previousState = EnemyModel.States.stationary;

        }
    }
}
