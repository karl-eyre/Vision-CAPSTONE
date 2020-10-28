﻿using FMOD.Studio;
using FMODUnity;
using InDevelopment.Alex;
using UnityEngine;

public class F_Music : MonoBehaviour
{
    public static EventInstance music;
    string enemyState;

    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        music.start();
        music.release();

        StateManager.changeStateEvent += enemyState;
    }

    void MusicSwitcher()
    {      
        if (enemyState == "")
        {

        }
    }
}
