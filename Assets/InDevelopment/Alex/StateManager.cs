using UnityEngine;

namespace InDevelopment.Alex
{
   public class StateManager : MonoBehaviour
   {
      public StateBase currentState;

      public void ChangeState(StateBase newState)
      {
         //Check if the newState is different
         currentState.Exit();
         newState.Enter();
         currentState = newState;
      }

      public void Update()
      {
         currentState?.Execute();
      }
   }
}
