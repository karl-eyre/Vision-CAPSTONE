using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class F_AmbientSound : MonoBehaviour
{
    EventInstance ship; 

    public Animator animator;
    public GameObject soundsource;
    void Start()
    {
        RuntimeManager.PlayOneShotAttached("event:/Ambience/ShipFlyOver", soundsource);
        animator.SetBool("Play", true);
    }
}
