using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class F_Player : MonoBehaviour
{
    EventInstance musicTrack;
    EventInstance amb;
    EventInstance ambInterior;
    
    [SerializeField]
    private float radius = 30f;

    public Transform enemy;


    void Start()
    {
        backgroundMusic();
        amb = RuntimeManager.CreateInstance("event:/Ambience/AmbOutside");
        amb.start();

       
    }

    void backgroundMusic()
    {
        musicTrack = RuntimeManager.CreateInstance("event:/Music/MusicTest");
        musicTrack.start();
        musicTrack.release();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
  
   
    void Update()
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        //Debug.Log(distance);

        
        if (distance <= radius)
        {
            musicTrack.setParameterByName("Intencity", distance, false);
        }
    }
}
