using System;
using InDevelopment.Mechanics.LineOfSight;
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
		public EnemyController enemyController;
		
		[HideInInspector]
		public StateManager stateManager;

		[HideInInspector]
		public LineOfSight lineOfSight;

		[HideInInspector]
		public Vector3 posWhenInterrupted;

		private void Init()
		{
			if (!stateManager)
			{
				stateManager = GetComponent<StateManager>();
			}

			if (!lineOfSight)
			{
				lineOfSight = GetComponentInParent<LineOfSight>();
			}

			if (!enemyController)
			{
				enemyController = GetComponentInParent<EnemyController>();
			}
		}
		public void Awake()
		{
			Init();
		}

		public virtual void Enter()
		{
			Init();
			Debug.Log("I have entered into the " + this.GetType() + " state.");
		}
	
		public virtual void Exit()
		{
			Debug.Log("I have exited into the " + this.GetType() + " state.");
		}
	
		public virtual void Execute()
		{
			LOSFunc();
			Debug.Log("I am executing the " + this.GetType() + " state.");
		}

		public void LOSFunc()
		{
			if (lineOfSight.isDetecting)
			{
				if (lineOfSight.detectionMeter > lineOfSight.detectionThreshold)
				{
					stateManager.interruptedState = stateManager.currentEnemyState;
					posWhenInterrupted = enemyController.transform.position;
					stateManager.ChangeState(enemyController.investigatingEnemyState);
				}
				else
				{
					stateManager.ChangeState(enemyController.stationaryEnemyState);	
				}
				//enemyController.LookAtTarget(lineOfSight.player.transform.position);
				
			}
		}
	}
}
