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

            Debug.Log("player loses");
            // Scene scene = SceneManager.GetActiveScene();
            // SceneManager.LoadScene(scene.buildIndex);
        }
    }
}