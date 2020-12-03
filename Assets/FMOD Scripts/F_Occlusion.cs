using System;
using FMOD.Studio;
using FMODUnity;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using InDevelopment.Mechanics.Player;
using UnityEngine;

public class F_Occlusion : MonoBehaviour
{
    [SerializeField]
    private float OcclusionRadius = 30f;
    private EventInstance footsteps;
    public EventInstance searching;
    private Vector3 objPosition;
    [SerializeField]
    private LayerMask lm;
    EnemyController enemyController;
    private RaycastHit hit;
    Transform player;
    void Start()
    {
        player = GameObject.Find("Player 1").GetComponent<Transform>();
        enemyController = GetComponent<EnemyController>();
        lm = LayerMask.GetMask("Obstacle");
        objPosition = transform.position;
        StateManager.changeStateEvent += MusicAndSounds;
        RuntimeManager.PlayOneShotAttached("event:/Enemies/BotTurn",default);
   
        footsteps = RuntimeManager.CreateInstance("event:/Enemies/E_Footsteps");
        RuntimeManager.AttachInstanceToGameObject(footsteps, transform, GetComponent<Rigidbody>());
    }

    #region Footsteps

    public void BotTurn()
    {
        RuntimeManager.PlayOneShot("event:/Enemies/BotTurn",default);
    }

    public void BotTurnBack()
    {
        RuntimeManager.PlayOneShot("event:/Enemies/BotTurnBack",default);
    }
    
    public void BotLook()
    {
        RuntimeManager.PlayOneShot("event:/Enemies/BotLook",default);
    }
    public void BotStep()
    {
        RuntimeManager.PlayOneShot("event:/Enemies/E_Footsteps");
    }

    #endregion

    private void OnDrawGizmosSelected() //Visual Radius For Occlusion & Music.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(objPosition, OcclusionRadius);

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
                searching = RuntimeManager.CreateInstance("event:/Enemies/Searching");
                RuntimeManager.AttachInstanceToGameObject(searching, transform, GetComponent<Rigidbody>());
                searching.start();
                searching.release();
        }
        else if (enemyState == enemyController.stationaryEnemyState)
        {
            searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); 
        }
        else if (enemyState == enemyController.patrollingEnemyState)
        {
            searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (enemyState == enemyController.playerDetectedState)
        {
            F_Music.music.setParameterByName("MusicState", 1, true);
            F_AnnouncerIndoors.announcer.setParameterByName("PlayerDetected", 1, false);
        }
    }

    #region Occlusion
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
    #endregion

    private void OnDestroy()
    {
        footsteps.release();
        StateManager.changeStateEvent -= MusicAndSounds;
    }
}
