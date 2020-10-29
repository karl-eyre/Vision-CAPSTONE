﻿using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_MusicChange : MonoBehaviour
{
    EnemyController enemyController;
    void Start()
    {
        enemyController = GameObject.Find("Enemy").GetComponent<EnemyController>();

        StateManager.changeStateEvent += MusicSwitcher;
    
    }
    void MusicSwitcher(EnemyStateBase enemyState)
    {
        if (enemyState == enemyController.patrollingEnemyState)
        {
            F_Music.music.setParameterByName("MusicState", 0f, false);
        }
        if (enemyState == enemyController.playerDetectedState)
        {
            Debug.Log("detected");
            F_Music.music.setParameterByName("MusicState", 1f, false);
        }
    }
    private void OnDestroy()
    {
        StateManager.changeStateEvent -= MusicSwitcher;
    }


}
