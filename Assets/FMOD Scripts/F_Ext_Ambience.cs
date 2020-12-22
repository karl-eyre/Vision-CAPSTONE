using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.SceneManagement;

public class F_Ext_Ambience : MonoBehaviour
{
    public static EventInstance extAmb;
    [SerializeField]
    private bool noSiren;
    void Start()
    {
        extAmb = RuntimeManager.CreateInstance("event:/Ambience/Amb_Ext_City_Night");
        extAmb.start();

        if (noSiren == true)
        {
            extAmb.setParameterByName("NoSiren", 1, true);
        }
    }
    private void OnDestroy()
    {
        extAmb.release();
        extAmb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
