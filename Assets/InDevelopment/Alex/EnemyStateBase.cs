using System;
using UnityEngine;

namespace InDevelopment.Alex
{
	public class EnemyStateBase : MonoBehaviour
	{
		
		/// <summary>
		/// Detect sounds here and have a universal function to switch to the investigating state.
		/// Figure out what stepts to take next when we get there.
		/// </summary>
		
		
		[HideInInspector]
		public StateManager stateManager;
		public void Awake()
		{
			if (!stateManager)
			{
				stateManager = GetComponent<StateManager>();
			}
		}

		public virtual void Enter()
		{
			if (!stateManager)
			{
				stateManager = GetComponent<StateManager>();
			}
			Debug.Log("I have entered into the " + this.GetType() + " state.");
		}
	
		public virtual void Exit()
		{
			Debug.Log("I have exited into the " + this.GetType() + " state.");
		}
	
		public virtual void Execute()
		{
			Debug.Log("I am executing the " + this.GetType() + " state.");
		}
	}
}
