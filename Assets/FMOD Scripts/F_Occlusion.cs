﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

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

    [SerializeField]
    private LayerMask lm;
    private RaycastHit hit;
    Transform player;

    //Testing
    bool patroling;
    //Animator animator;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        //animator = GetComponent<Animator>();
        music = RuntimeManager.CreateInstance(eventPath);
        RuntimeManager.AttachInstanceToGameObject(music, transform, GetComponent<Rigidbody>());
        music.start();
        F_Player.musicTrack.setParameterByName("Intencity", 100f, false);
        searching = RuntimeManager.CreateInstance("event:/Enemies/Searching");
        RuntimeManager.AttachInstanceToGameObject(searching, transform, GetComponent<Rigidbody>());
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
            Animation();
        }
        else
        {
            music.setParameterByName("LowPass", 0, false);
            //searching.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            patroling = false;
        }
    }
    private void Update()
    {
        Music();
        
    }
    void Music() //Fade In More Intense Music depending on how close player is. 
    {
       float musicDist = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(musicDist);

        if (player != null)
        {
            if (musicDist <= MusicRadius)
            {
                F_Player.musicTrack.setParameterByName("Intencity", musicDist, false);
            }
        }
    }

    void Animation()
    {
        //For Testing
        if (patroling == false)
        {
            //searching.start();
            //animator.SetBool("Start", true);
            patroling = true;
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
        F_Player.musicTrack.setParameterByName("Intencity", 100f, false);
    }
}
