using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class StationaryEnemyState : EnemyStateBase
    {
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
            //look around until you see the player then trigger investigation state
            base.Execute();
            enemyController.LookLeftAndRight();
        }
    }
}