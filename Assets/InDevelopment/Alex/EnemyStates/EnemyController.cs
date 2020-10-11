using System;
using System.Collections;
using System.Collections.Generic;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    /// <summary>
    /// What do i want this enemy to do?
    /// Patrol, Investigate, Stationary,, WaitingAtPoint, ReturnToPos, DetectPlayer
    ///
    /// Detect and handle sound.
    /// </summary>

#region Variables

    public float maxLandReturn = 60;
    public float rotSpeed = 5;
    public float moveSpeed;

    [Header("=Debug=")]
    public Quaternion rotation;
    public Vector3 direction;
    
    
    [HideInInspector]
    public StateManager stateManager;
    private EnemyStateBase _enemyState;

    [HideInInspector]
    public bool beingDistracted;

    // [HideInInspector]
    public Vector3 posWhenInterrupted;

    private NavMeshAgent agent;
    
    //[Header("States")]
    [HideInInspector]public PatrollingEnemyState patrollingEnemyState;
    [HideInInspector]public StationaryEnemyState stationaryEnemyState;
    [HideInInspector]public WaitingAtPointEnemyState waitingAtPointEnemyState;
    [HideInInspector]public ReturningToPosEnemyState returningToPosEnemyState;
    [HideInInspector]public InvestigatingEnemyState investigatingEnemyState;
    [HideInInspector]public PlayerDetectedState playerDetectedState;
    [HideInInspector]public StartingState startingState;
    [HideInInspector]public SpottingState spottingState;
    
    

    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        SetupStates();
        SetupNavmesh();
    }

    void SetupNavmesh()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = true;
        agent.speed = moveSpeed;
    }

    void SetupStates()
    {
        stateManager = GetComponentInChildren<StateManager>();
        patrollingEnemyState = GetComponentInChildren<PatrollingEnemyState>();
        stationaryEnemyState = GetComponentInChildren<StationaryEnemyState>();
        waitingAtPointEnemyState = GetComponentInChildren<WaitingAtPointEnemyState>();
        returningToPosEnemyState = GetComponentInChildren<ReturningToPosEnemyState>();
        investigatingEnemyState = GetComponentInChildren<InvestigatingEnemyState>();
        playerDetectedState = GetComponentInChildren<PlayerDetectedState>();
        startingState = GetComponentInChildren<StartingState>();
        spottingState = GetComponentInChildren<SpottingState>();
        
        stateManager.ChangeState(startingState);
    }

    public void MoveToTarget(Vector3 tgt)
    {
        if (agent.isStopped || agent.remainingDistance < 0.5f)
        {
            agent.ResetPath();
        }
        
        agent.SetDestination(tgt);
        /* old movement
        var position = transform.position;
        position = Vector3.MoveTowards(position, tgt, Time.deltaTime * moveSpeed);
        LookAtTarget(tgt);
        transform.position = position;
        direction = (tgt - position).normalized;
        //rotation = transform.rotation;
        */
       
    }

    public void LookAtTarget(Vector3 tgt)
    {
        transform.LookAt(tgt); 
    }
    
    public void LookLeftAndRight()
    {
        var t = transform.position;
        transform.rotation = Quaternion.Euler(0f, maxLandReturn * Mathf.Sin(Time.time * rotSpeed), 0f);
        //TODO: Fix this so that it does the look left and right thing based on its current looking direction!
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = rotation;
    }
}
