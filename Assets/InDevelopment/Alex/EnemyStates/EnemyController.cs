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

    
    [Header("States")]
    public PatrollingEnemyState patrollingEnemyState;
    public StationaryEnemyState stationaryEnemyState;
    public WaitingAtPointEnemyState waitingAtPointEnemyState;
    public ReturningToPosEnemyState returningToPosEnemyState;
    public InvestigatingEnemyState investigatingEnemyState;
    
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
        stateManager.ChangeState(stationaryEnemyState);
    }

    public void MoveToTarget(Vector3 tgt)
    {
        var position = transform.position;
        position = Vector3.MoveTowards(position, tgt, Time.deltaTime * speed);
        LookAtTarget(tgt);
        transform.position = position;
        direction = (tgt - position).normalized;
        rotation = transform.rotation;
    }

    public void LookAtTarget(Vector3 tgt)
    {
        transform.LookAt(tgt); 
    }
    
    public void LookLeftAndRight()
    {
        var t = transform.position;
        rotation = Quaternion.Euler(0f, maxLandRturn * Mathf.Sin(Time.time * rotSpeed), 0f);
        //TODO: Fix this so that it does the look left and right thing based on its current looking direction!
        //transform.LookAt(t.x, t.y, t.z);
        //t1.rotation.y = Quaternion.Lerp(t1.rotation.y, t1.rotation.y + 60, 0.2);
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.rotation = rotation;
    }
}
