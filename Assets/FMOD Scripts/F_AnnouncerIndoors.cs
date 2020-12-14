using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using InDevelopment.Alex;
using InDevelopment.Alex.EnemyStates;
using InDevelopment.Mechanics.Player;

public class F_AnnouncerIndoors : MonoBehaviour
{
    [EventRef]
    public string announcerEvent;
    public static EventInstance announcer;
    [SerializeField]
    private bool overideAttenuation;
    [SerializeField]
    float OverrideMinDistance = -1.0f;
    [SerializeField]
    float OverrideMaxDistance = -1.0f;

    public PlayerDetectionUI detectionM;
    void Start()
    {
        announcer = RuntimeManager.CreateInstance(announcerEvent);
        RuntimeManager.AttachInstanceToGameObject(announcer, transform, GetComponent<Rigidbody>());
        announcer.start();
        announcer.release();
    }

    private void Update()
    {
        OverrideAttenuation();

        if (detectionM.detectionMeter <= 0)
        {
            announcer.setParameterByName("PlayerDetected", 0, false);
            F_Music.music.setParameterByName("MusicState", 0, true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, OverrideMinDistance);
        Gizmos.DrawWireSphere(transform.position, OverrideMaxDistance);
    }

    void OverrideAttenuation()
    {
        if (overideAttenuation == true)
        {
            announcer.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, OverrideMinDistance);
            announcer.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, OverrideMaxDistance);
        }
    }

    private void OnDestroy()
    {
        announcer.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        announcer.setParameterByName("PlayerDetected", 0, false);
    }
}
