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
    void Start()
    {
        snapshot = RuntimeManager.CreateInstance(eventpath);
        RuntimeManager.AttachInstanceToGameObject(snapshot, transform, GetComponent<Rigidbody>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("snapshot");
            snapshot.start();         
        }    
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            snapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
