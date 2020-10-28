using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class F_MusicPlayer : MonoBehaviour
{
    [EventRef]
    public string eventpath;
    EventInstance musicTrack;
    void Start()
    {
        musicTrack = RuntimeManager.CreateInstance(eventpath);
        musicTrack.start();
        musicTrack.release();
    }
    public void StopMusic()
    {
        musicTrack.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
