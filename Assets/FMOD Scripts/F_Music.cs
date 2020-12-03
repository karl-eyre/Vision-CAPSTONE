using System;
using FMOD.Studio;
using FMODUnity;
using InDevelopment.Mechanics.Player;
using UnityEngine;

public class F_Music : MonoBehaviour
{
    public static EventInstance music;
    [SerializeField]
    private PlayerDetectionUI detectionMeterValue;
    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        music.start();
    }

    private void Update()
    {
        music.setParameterByName("Intencity", detectionMeterValue.detectionMeter, false);
    }

    private void OnDestroy()
    {
        music.release();
    }
}
