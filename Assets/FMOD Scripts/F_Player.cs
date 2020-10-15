using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class F_Player : MonoBehaviour
{
    public static EventInstance musicTrack;
    EventInstance amb;
    EventInstance ambInterior;
    
    [SerializeField]
    private float MusicRadius = 30f;

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

    void Footsteps()
    {

    }
}
