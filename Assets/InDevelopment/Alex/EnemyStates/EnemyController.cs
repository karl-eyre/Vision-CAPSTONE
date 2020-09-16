using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
#region Variables

    [Header("Enemy Settings")]
    public List<GameObject> waypoints;

    private int currentIndex = 0;

    public float speed;
    private float WaypointRadius = 1;


    public float WaitTime;

    public float rotSpeed = 2f;
    public float maxRotation = 45f;

    [HideInInspector]
    public bool investigating;

    private bool waiting;

    [SerializeField]
    private bool stationary;

    [HideInInspector]
    public Vector3 targetLastKnownPos;

    private Vector3 targetPosition;
    private Vector3 previousPos;
    private Quaternion previousRot;
    
    [Header("Debug"), Tooltip("Leave ALONE")]
    public float investigationThreshold = 50f;
    public float maxDetection = 100f;


    
    
#endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
