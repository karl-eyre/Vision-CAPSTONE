﻿using FMOD.Studio;
using FMODUnity;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using UnityEngine;

public class F_Music : MonoBehaviour
{
    public static EventInstance music;
    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        music.start();
    }

    private void OnDestroy()
    {
        music.release();
    }
}
