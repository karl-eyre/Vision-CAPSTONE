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
        public float resetDelay = 15f;
        private float timer;

        public override void Enter()
        {
            base.Enter();
            player = FindObjectOfType<PlayerMovement>().gameObject;
            timer = resetDelay;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Execute()
        {
            base.Execute();

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (!settingTarget)
                {
                    settingTarget = true;
                    StartCoroutine(SetTargetPos());
                }

                enemyController.MoveToTarget(target);
                LookAtPlayer();
            }
            else
            {
                if (stateManager.currentEnemyState != enemyController.startingState)
                {
                    ResetAIState();
                }
            }
        }


        private IEnumerator SetTargetPos()
        {
            target = player.transform.position;
            yield return new WaitForSeconds(0.5f);
            settingTarget = false;
        }
    }
}