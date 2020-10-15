using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class PlayerDetectedState : EnemyStateBase
    {
        private GameObject player;

        public override void Enter()
        {
            base.Enter();
            player = GetComponentInParent<EnemyController>().gameObject;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
            //TODO:chase player until the player is caught
            Debug.Log("player loses");
        }
    }
}