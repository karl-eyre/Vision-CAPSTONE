using System.Collections;
using System.Collections.Generic;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    /// <summary>
    /// What do i want this enemy to do?
    /// Patrol, Investigate, Stationary,, WaitingAtPoint, ReturnToPos, DetectPlayer
    ///
    /// Detect and handle sound.
    /// </summary>

#region Variables

    public float speed = 5;
    public float maxLandRturn = 60;
    public float rotSpeed = 5;

    [Header("=Debug=")]
    public Quaternion rotation;
    public Vector3 direction;
    
    
    [HideInInspector]
    public StateManager stateManager;
    private EnemyStateBase _enemyState; 

    
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
    void Awake()
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
        var position = transform.position;
        position = Vector3.MoveTowards(position, tgt, Time.deltaTime * speed);
        LookAtTarget(tgt);
        transform.position = position;
        direction = (tgt - position).normalized;
        //rotation = transform.rotation;
    }

    public void LookAtTarget(Vector3 tgt)
    {
        transform.LookAt(tgt); 
    }
    
    public void LookLeftAndRight()
    {
        var t = transform.position;
        transform.rotation = Quaternion.Euler(0f, maxLandRturn * Mathf.Sin(Time.time * rotSpeed), 0f);
        //TODO: Fix this so that it does the look left and right thing based on its current looking direction!
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = rotation;
    }
}
