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

		[HideInInspector]
		public Transform lastKnownPlayerPosition;

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
				//ALWAYS HAPPENS
				//enemyController.LookAtTarget(lineOfSight.player.transform.position);
				enemyController.LookAtTarget(lineOfSight.player.transform.position);
				stateManager.interruptedState = stateManager.currentEnemyState;
				lastKnownPlayerPosition = lineOfSight.player.transform;
				if (lineOfSight.detectionMeter > lineOfSight.investigationThreshold)
				{
					if (stateManager.currentEnemyState != enemyController.investigatingEnemyState)
					{
						posWhenInterrupted = enemyController.transform.position;
						stateManager.ChangeState(enemyController.investigatingEnemyState);
					}
					//NOT DOING SHIT
				}
				else
				{
					stateManager.ChangeState(enemyController.stationaryEnemyState);	
				}
				
				
			}
			else
			{
				//WHAT DO I DO WHEN I STOP DETECTING
				//STOP CHASING!
				//stateManager.ChangeState(stateManager.interruptedState);
			}
		}
		
		
	}
}
