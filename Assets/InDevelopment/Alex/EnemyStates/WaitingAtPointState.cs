using System.Collections;
using InDevelopment.Mechanics.Enemy;
using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class WaitingAtPointState : StateBase
    {
        public EnemyModel enemyModel;
        public InvestigatingState InvestigatingState;
        public ReturningToPosState returningToPosState;
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
            }

            if (!enemyModel.waiting)
            {
                StartCoroutine(WaitAtWaypoint());
            }

            
        }
        
        public IEnumerator WaitAtWaypoint()
        {
            enemyModel.waiting = true;
            yield return new WaitForSeconds(enemyModel.WaitTime);
            enemyModel.waiting = false;
            enemyModel.lineOfSight.stopDecrease = false;

            if (enemyModel.investigating)
            {
                enemyModel.targetLastKnownPos = enemyModel.previousPos;
                enemyModel.investigating = false;
                enemyModel.lineOfSight.detectionMeter = enemyModel.investigationThreshold - 5;
                stateManager.ChangeState(returningToPosState);
                yield break;
            }

            if (enemyModel.IsDetecting() && enemyModel.lineOfSight.DistToTarget() < enemyModel.lineOfSight.ViewDistance())
            {
                stateManager.ChangeState(InvestigatingState);
                yield break;
            }

            enemyModel.currentIndex++;
            stateManager.ChangeState(returningToPosState);
        }
    }
}
