using UnityEngine;

namespace InDevelopment.Alex
{
   public class StateManager : MonoBehaviour
   {
      public EnemyStateBase currentEnemyState;
      public EnemyStateBase previousEnemyState;
      
      public void ChangeState(EnemyStateBase newEnemyState)
      {
         //Check if the newState is different
         previousEnemyState = currentEnemyState;
         currentEnemyState?.Exit();
         newEnemyState?.Enter();
         currentEnemyState = newEnemyState;
      }

      public void Update()
      {
         currentEnemyState?.Execute();
      }
   }
}
