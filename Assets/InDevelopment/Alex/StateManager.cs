using UnityEngine;

namespace InDevelopment.Alex
{
   public class StateManager : MonoBehaviour
   {
      public StateBase currentState;
      public StateBase previousState;
      
      public void ChangeState(StateBase newState)
      {
         //Check if the newState is different
         previousState = currentState;
         currentState?.Exit();
         newState?.Enter();
         currentState = newState;
      }

      public void Update()
      {
         currentState?.Execute();
      }
   }
}
