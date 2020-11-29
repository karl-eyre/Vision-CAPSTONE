using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class F_Ext_Ambience : MonoBehaviour
{
    public static EventInstance extAmb;
    void Start()
    {
        extAmb = RuntimeManager.CreateInstance("event:/Ambience/Amb_Ext_City_Night");
        extAmb.start();
    }

    private void OnDestroy()
    {
        extAmb.release();
    }
}
