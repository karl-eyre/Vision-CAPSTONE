using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class SnapShotTrigger : MonoBehaviour
{
    EventInstance snapshot;
    EventInstance snapshot2;
    EventInstance ambInterior;

    public GameObject Player;
    void Start()
    {
        snapshot = RuntimeManager.CreateInstance("snapshot:/Area1 Tutorial");
        snapshot2 = RuntimeManager.CreateInstance("snapshot:/Area2 Tutorial");
        ambInterior = RuntimeManager.CreateInstance("event:/Ambience/RoomTone");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            snapshot.start();         
            ambInterior.start();
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Debug.Log("exit");
            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
