using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class F_Music : MonoBehaviour
{
    [EventRef]
    public string eventpath;
    EventInstance music;

    void Start()
    {
        music = RuntimeManager.CreateInstance(eventpath);
        music.start();
        music.release();
    }
}
