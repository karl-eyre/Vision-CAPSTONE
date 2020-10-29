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
    EventInstance music;
    EventInstance searching;

    EnemyController enemyController;

    [SerializeField]
    private LayerMask lm;

    private RaycastHit hit;
    Transform player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        music = RuntimeManager.CreateInstance(eventPath);
        RuntimeManager.AttachInstanceToGameObject(music, transform, GetComponent<Rigidbody>());
        music.start();
        F_Music.music.setParameterByName("Intencity", 100f, false);
        searching = RuntimeManager.CreateInstance("event:/Enemies/Searching");
        RuntimeManager.AttachInstanceToGameObject(searching, transform, GetComponent<Rigidbody>());
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

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position); //Distance Between player and sound source

        if (distance <= OcclusionRadius)
        {
            Occlusion();
            Lowpass();
        }
        else
        {
            music.setParameterByName("LowPass", 0, false);
        }
    }
    void MusicAndSounds(EnemyStateBase enemyState) //Fade In More Intense Music depending on how close player is. 
    {
       float musicDist = Vector3.Distance(transform.position, player.transform.position);
        //Debug.Log(musicDist);

        if (player != null)
        {
            if (musicDist <= MusicRadius)
            {
                F_Music.music.setParameterByName("Intencity", musicDist, false);          
            }          
        }
        if (enemyState == enemyController.investigatingEnemyState)
        {
            searching.start();
        }
        else if (enemyState == enemyController.patrollingEnemyState)
        {
            Debug.Log("patrolling");
            searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
                music.setParameterByName("LowPass", 1, false);
            }             
        }
        if (hit.collider == null)
        {
            //Debug.Log("No wall");
            music.setParameterByName("LowPass", 0, false);
        }
    }

    private void OnDestroy()
    {
        music.release();
        F_Music.music.setParameterByName("Intencity", 100f, false);
    }
}
