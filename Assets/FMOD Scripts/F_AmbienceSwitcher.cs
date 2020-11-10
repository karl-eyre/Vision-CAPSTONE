using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
public class F_AmbienceSwitcher : MonoBehaviour
{
    [SerializeField]
    private float drone;
    [SerializeField]
    private float cityOutside;
    [SerializeField]
    private float rattlingFan;
    [SerializeField]
    private float roomToneHallWay;
    private float[] values = new float[4];
    [SerializeField]
    bool enableGameplayMusic;

    private void Start()
    {
        values[0] = drone;
        values[1] = cityOutside;
        values[2] = rattlingFan;
        values[3] = roomToneHallWay;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            F_Ambience.amb.setParametersByIDs(F_Ambience.pIDS, values, 4, false);
            if (enableGameplayMusic == true)
                F_Music.music.setParameterByName("GameplayMusic", 1, false);
        }
    }
}
