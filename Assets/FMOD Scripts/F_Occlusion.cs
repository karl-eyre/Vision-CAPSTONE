using FMOD.Studio;
using FMODUnity;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using UnityEngine;

public class F_Occlusion : MonoBehaviour
{
    [SerializeField]
    private float OcclusionRadius = 30f;
    [SerializeField]
    private float MusicRadius = 20f;
    [EventRef]
    public string eventPath;
    EventInstance footsteps;
    public EventInstance searching;
    private Vector3 objPosition;
    [SerializeField]
    private LayerMask lm;
    EnemyController enemyController;
    private RaycastHit hit;
    Transform player;
    void Start()
    {
        FmodEventInstances();
        player = GameObject.Find("Player").GetComponent<Transform>();
        enemyController = GetComponent<EnemyController>();
        lm = LayerMask.GetMask("Obstacle");
        objPosition = transform.position;
        searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        StateManager.changeStateEvent += MusicAndSounds;
    }
    void FmodEventInstances()
    {
        footsteps = RuntimeManager.CreateInstance("event:/Enemies/E_Footsteps");
        RuntimeManager.AttachInstanceToGameObject(footsteps, transform, GetComponent<Rigidbody>());
        footsteps.start();
        
        searching = RuntimeManager.CreateInstance("event:/Enemies/Searching");
        RuntimeManager.AttachInstanceToGameObject(searching, transform, GetComponent<Rigidbody>());
    }
    private void OnDrawGizmosSelected() //Visual Radius For Occlusion & Music.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(objPosition, OcclusionRadius);
        Gizmos.DrawWireSphere(objPosition, MusicRadius);
    }
    private void FixedUpdate()
    {
        if (!(player is null))
        {
            float distance = Vector3.Distance(transform.position, player.position); //Distance Between player and sound source


            if (distance <= OcclusionRadius)
            {
                Occlusion();
                Lowpass();
            }
            else
            {
                footsteps.setParameterByName("LowPass", 0, false);
            }
        }
    }
    void MusicAndSounds(EnemyStateBase enemyState) //Fade In More Intense Music depending on how close player is. 
    {      
        //Debug.Log(musicDist);
        if (enemyState == enemyController.investigatingEnemyState) 
        {
                searching.start();
        }
        if (enemyState == enemyController.stationaryEnemyState)
        {
                searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (enemyState == enemyController.patrollingEnemyState)
        {
                searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (enemyState == enemyController.playerDetectedState)
        {
            F_Music.music.setParameterByName("MusicState", 1, true);
        }
    }
    void Occlusion() //Raycast From sound Source to Player
    {
        Vector3 playerPos = player.position;
        float dist = Vector3.Distance(objPosition, playerPos);
        Vector3 directionToFace = playerPos - objPosition;
        Physics.Raycast(objPosition, directionToFace, out hit, dist, lm);
        Debug.DrawRay(objPosition, directionToFace, Color.red);
    }
    void Lowpass() //Occludes Sound Source
    {
        if (hit.collider)
        {
            if (hit.collider.gameObject.tag == "Obstacles")
            {
                //Debug.Log("wall");
                footsteps.setParameterByName("LowPass", 1, true);
                searching.setParameterByName("LowPass", 1, true);
            }
        }
        else
        {
            //Debug.Log("No wall");
            footsteps.setParameterByName("LowPass", 0, true);
            searching.setParameterByName("LowPass", 0, true);
        }
    }
    private void OnDestroy()
    {
        footsteps.release();
        searching.release();
        StateManager.changeStateEvent -= MusicAndSounds;
    }
}
