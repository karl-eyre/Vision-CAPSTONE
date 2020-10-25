using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class F_Music : MonoBehaviour
{
    public static EventInstance music;

    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        music.start();
        music.release();
    }
}
