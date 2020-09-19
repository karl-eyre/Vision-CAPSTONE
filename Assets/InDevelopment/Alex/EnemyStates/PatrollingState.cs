using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class PatrollingState : StateBase
    {
        private EnemyController _enemyController;
        
        public override void Enter()
        {
            base.Enter();
            gameObject.GetComponent<EnemyController>();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
           
            
        }
        
        public void MoveToTarget(Vector3 tgt)
        {
            var position = transform.position;
            position = Vector3.MoveTowards(position, tgt, Time.deltaTime * _enemyController.speed);
            transform.position = position;
            Vector3 direction = (tgt - position).normalized;
        }
    }
}
