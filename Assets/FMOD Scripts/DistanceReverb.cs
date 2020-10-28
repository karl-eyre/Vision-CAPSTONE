using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class DistanceReverb : MonoBehaviour
{
    EventInstance reverb;
    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");

        reverb = RuntimeManager.CreateInstance("snapshot:/MainRoom/WholeRoom");
        RuntimeManager.AttachInstanceToGameObject(reverb, transform, GetComponent<Rigidbody>());
        reverb.start();
        reverb.release();
    }
    private void OnDestroy()
    {
        reverb.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void GetDistance()
    {      
        float dist = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(dist);
    }
}
