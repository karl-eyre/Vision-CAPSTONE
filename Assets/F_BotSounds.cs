using System;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class F_BotSounds : MonoBehaviour
{
    private F_Occlusion occlusionScript;


    private void Start()
    {
        occlusionScript = GetComponentInParent<F_Occlusion>();
    }

    public void BotTurn()
    {
        RuntimeManager.PlayOneShotAttached("event:/Enemies/BotTurn",this.gameObject);
    }

    public void BotTurnBack()
    {
        RuntimeManager.PlayOneShotAttached("event:/Enemies/BotTurnBack",this.gameObject);
    }
    
    public void BotLook()
    {
        RuntimeManager.PlayOneShotAttached("event:/Enemies/BotLook",this.gameObject);
    }
    public void BotStep()
    {
        EventInstance footsteps = RuntimeManager.CreateInstance("event:/Enemies/E_Footsteps");
        footsteps.set3DAttributes(RuntimeUtils.To3DAttributes(this.gameObject, GetComponent<Rigidbody>()));
        footsteps.setParameterByName("LowPass", occlusionScript.occlusion, true);
        footsteps.start();
        footsteps.release();
    }
}
