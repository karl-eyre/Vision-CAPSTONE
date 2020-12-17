using System;
using FMOD.Studio;
using FMODUnity;
using InDevelopment.Mechanics.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class F_Music : MonoBehaviour
{
    public static EventInstance music;
    [SerializeField]
    private PlayerDetectionUI detectionMeterValue;
    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        if (SceneManager.GetActiveScene().name == "3_Backstreet 1")
        {
            music.setParameterByName("Outside", 1, false);
            music.start();
        }
        else
        {
            music.start();
        }
 
    }

    private void Update()
    {
        music.setParameterByName("Intencity", detectionMeterValue.detectionMeter, false);
    }

    private void OnDestroy()
    {
        music.release();
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
