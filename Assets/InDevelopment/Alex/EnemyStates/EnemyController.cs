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

    public StateManager stateManager;
    private StateBase _state; //Find a use for this or get rid of it later.

    [Header("States")]
    public PatrollingState patrollingState;
    public StationaryState stationaryState;
    public WaitingAtPointState waitingAtPointState;
    public ReturningToPosState returningToPosState;
    public InvestigatingState investigatingState;
    
#endregion
    // Start is called before the first frame update
    void Awake()
    {
        patrollingState = GetComponentInChildren<PatrollingState>();
        stationaryState = GetComponentInChildren<StationaryState>();
        waitingAtPointState = GetComponentInChildren<WaitingAtPointState>();
        returningToPosState = GetComponentInChildren<ReturningToPosState>();
        investigatingState = GetComponentInChildren<InvestigatingState>();
        stateManager.ChangeState(stationaryState);
    }

    public void MoveToTarget(Vector3 tgt)
    {
        var position = transform.position;
        position = Vector3.MoveTowards(position, tgt, Time.deltaTime * speed);
        transform.position = position;
        Vector3 direction = (tgt - position).normalized;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
