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

    bool soundsPlayed;
    bool detected;

    EnemyController enemyController;

    [SerializeField]
    private LayerMask lm;

    private RaycastHit hit;
    Transform player;
    void Start()
    {
        searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        player = GameObject.Find("Player").GetComponent<Transform>();
        footsteps = RuntimeManager.CreateInstance("event:/Enemies/E_Footsteps");
        footsteps.start();

        //F_Music.music.setParameterByName("Intencity", 100f, false);
        searching = RuntimeManager.CreateInstance("event:/Enemies/Searching");


        lm = LayerMask.GetMask("Obstacle");
        enemyController = GetComponent<EnemyController>();


        StateManager.changeStateEvent += MusicAndSounds;
    }

    private void OnDrawGizmosSelected() //Visual Radius For Occlusion & Music.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OcclusionRadius);
        Gizmos.DrawWireSphere(transform.position, MusicRadius);
    }


    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(searching, transform, GetComponent<Rigidbody>());
        RuntimeManager.AttachInstanceToGameObject(footsteps, transform, GetComponent<Rigidbody>());
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


            //float musicDist = Vector3.Distance(transform.position, player.transform.position);
            //if (distance <= OcclusionRadius)
           // {
                //F_Music.music.setParameterByName("Intencity", musicDist, false);
           // }
        }
    }

    void MusicAndSounds(EnemyStateBase enemyState) //Fade In More Intense Music depending on how close player is. 
    {      
        //Debug.Log(musicDist);
        if (enemyState == enemyController.investigatingEnemyState) 
        {
                searching.start();
                soundsPlayed = true;
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
            detected = false;
        }
    }
    void Occlusion() //Raycast From sound Source to Player
    {
        float dist = Vector3.Distance(transform.position, player.position);
        Vector3 directionToFace = player.position - transform.position;
        Physics.Raycast(transform.position, directionToFace, out hit, dist, lm);
        Debug.DrawRay(transform.position, directionToFace, Color.red);
    }

    void Lowpass() //Occludes Sound Source
    {
        if (hit.collider)
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                //Debug.Log("wall");
                footsteps.setParameterByName("LowPass", 1, false);
            }             
        }
        if (hit.collider == null)
        {
            //Debug.Log("No wall");
            footsteps.setParameterByName("LowPass", 0, false);
        }
    }

    private void OnDestroy()
    {
        footsteps.release();
        searching.release();
        StateManager.changeStateEvent -= MusicAndSounds;
    }
}
