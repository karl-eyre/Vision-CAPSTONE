using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class F_MusicChange : MonoBehaviour
{
    public EnemyController enemyController;
    EventInstance searching;
    EventInstance intense;
    EventInstance intenseSnapshot;

    bool soundsPlayed;
    void Start()
    {
        intense = RuntimeManager.CreateInstance("event:/Music/Intense");
        intenseSnapshot = RuntimeManager.CreateInstance("snapshot:/EnemySearching/EnemySearching");

        StateManager.changeStateEvent += MusicSwitcher;
    
    }
    void MusicSwitcher(EnemyStateBase enemyState)
    {
        if (enemyState == enemyController.patrollingEnemyState)
        {
            F_Music.music.setParameterByName("MusicState", 0f, false);
            intense.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            intenseSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (enemyState == enemyController.playerDetectedState)
        {
            Debug.Log("detected");
            F_Music.music.setParameterByName("MusicState", 1f, false);
            intense.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            intenseSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        //Intense Music
        if (enemyState == enemyController.investigatingEnemyState)
        {
            Debug.Log("Intensestart");
            intense.start();
            intenseSnapshot.start();
        }
        if (enemyState == enemyController.stationaryEnemyState)
        {
            Debug.Log("Intensestop");
            intense.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            intenseSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
    private void OnDestroy()
    {
        StateManager.changeStateEvent -= MusicSwitcher;
    }


}
