using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class SnapShotTrigger : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string eventpath;
    public EventInstance snapshot; 
    GameObject Player;
    void Start()
    {
        snapshot = RuntimeManager.CreateInstance(eventpath);
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        RuntimeManager.AttachInstanceToGameObject(snapshot, transform, GetComponent<Rigidbody>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
        {
            snapshot.start();         
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
