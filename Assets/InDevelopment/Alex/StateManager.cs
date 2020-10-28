using System;
using System.Collections.Generic;
using UnityEngine;

namespace InDevelopment.Alex
{
    public class StateManager : MonoBehaviour
    {
        public EnemyStateBase currentEnemyState;
        public EnemyStateBase previousEnemyState;
        public EnemyStateBase initialState;
        public EnemyStateBase interruptedState;
        public List<EnemyStateBase> listOfStates = new List<EnemyStateBase>();

        private static event Action<string> changeStateEvent;

        public void ChangeState(EnemyStateBase newEnemyState)
        {
            //Check if the newState is different
            currentEnemyState?.Exit();
            previousEnemyState = currentEnemyState;
            newEnemyState?.Enter();
            currentEnemyState = newEnemyState;

            if (!(currentEnemyState is null))
            {
                changeStateEvent?.Invoke(currentEnemyState.name);
            }

            listOfStates.Add(currentEnemyState);
        }

        public void Update()
        {
            currentEnemyState?.Execute();
        }
    }
}