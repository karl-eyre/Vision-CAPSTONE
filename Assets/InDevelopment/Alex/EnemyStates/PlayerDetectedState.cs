using System;
using System.Collections;
using InDevelopment.Mechanics.Enemy;
using InDevelopment.Mechanics.Player;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class PlayerDetectedState : EnemyStateBase
    {
        private GameObject player;
        public Vector3 target;
        private bool settingTarget;
        
        public override void Enter()
        {
            base.Enter();
            player = FindObjectOfType<PlayerMovement>().gameObject;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();

            if (!settingTarget)
            {
                settingTarget = true;
                StartCoroutine(SetTargetPos());
            }

            enemyController.MoveToTarget(target);
            // Debug.Log("player loses");
        }

        private IEnumerator SetTargetPos()
        {
            target = player.transform.position;
            yield return new WaitForSeconds(0.5f);
            settingTarget = false;
        }
    }
}