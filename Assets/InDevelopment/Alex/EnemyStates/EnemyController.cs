using System.Collections;
using System.Collections.Generic;
using InDevelopment.Alex;
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
    
#endregion
    // Start is called before the first frame update
    void Start()
    {
        
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
