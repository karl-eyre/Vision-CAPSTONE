using System;
using UnityEngine;

namespace InDevelopment.Alex
{
	public class StateBase : MonoBehaviour
	{
		public StateManager stateManager;


		public void Awake()
		{
			stateManager = GetComponent<StateManager>();
		}

		public virtual void Enter()
		{
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
