using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class ReturningToPosEnemyState : EnemyStateBase
    {
        public Transform target;

        
        public override void Enter()
        {
            base.Enter();
            target.position = posWhenInterrupted;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
            enemyController.MoveToTarget(target.position);
            
        }
    }
}
