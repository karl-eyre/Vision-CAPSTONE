using System;
using UnityEngine;

namespace InDevelopment.Alex
{
	public class StateBase : MonoBehaviour
	{
		public StateManager stateManager;
		
		public virtual void Enter()
		{
			stateManager = GetComponent<StateManager>();
		}
	
		public virtual void Exit()
		{
		
		}
	
		public virtual void Execute()
		{
		
		}
	}
}
