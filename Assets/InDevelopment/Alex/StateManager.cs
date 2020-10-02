using System;
using UnityEngine;

namespace InDevelopment.Alex
{
   public class StateManager : MonoBehaviour
   {
      public EnemyStateBase currentEnemyState;
      public EnemyStateBase previousEnemyState;
      public EnemyStateBase initialState;
      public EnemyStateBase interruptedState;

      private void Start()
      {
         initialState = currentEnemyState;
      }

      public void ChangeState(EnemyStateBase newEnemyState)
      {
         //Check if the newState is different
         currentEnemyState?.Exit();
         previousEnemyState = currentEnemyState;
         newEnemyState?.Enter();
         currentEnemyState = newEnemyState;
      }

      public void Update()
      {
         currentEnemyState?.Execute();
      }
   }
}
