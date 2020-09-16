using UnityEngine;

namespace InDevelopment.Alex.EnemyStates
{
    public class InvestigatingState : StateBase
    {
        public StateBase stateToChangeTo;
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("I am in the " + this.GetType() + " state.");
            
        }

        public override void Exit()
        {
            base.Exit();
            
            
        }

        public override void Execute()
        {
            base.Execute();
            
            
        }
    }
}
