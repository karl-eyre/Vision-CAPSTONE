using System;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class PlayerDetectedState : EnemyStateBase
    {
        private GameObject target;
        
        public override void Enter()
        {
            base.Enter();
            target = lineOfSight.player;

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();
            //TODO set off event to aleat all AI of player detection
            enemyController.MoveToTarget(target.transform.position);
            Debug.Log("player loses");
        }
    }
}