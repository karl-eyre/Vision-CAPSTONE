using FMOD.Studio;
using FMODUnity;
using InDevelopment.Alex;
using UnityEngine;

public class F_Music : MonoBehaviour
{
    public static EventInstance music;
    string enemyState;

    void Start()
    {
        music = RuntimeManager.CreateInstance("event:/Music/AmbientMusic");
        music.start();
        music.release();

        StateManager.changeStateEvent += MusicSwitcher;
    }

    void MusicSwitcher(EnemyStateBase enemyState)
    {
        //sorry if i broke it, its just this was causing a compile error
        if (enemyState.ToString() == "InDevelopment.Alex.EnemyStates.StationaryEnemyState")
        {
            Debug.Log("Patroliing");
        }    
    }
}
