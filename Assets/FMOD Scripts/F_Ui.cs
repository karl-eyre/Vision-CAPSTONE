using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class F_Ui : MonoBehaviour
{
    public void UiPress()
    {
        RuntimeManager.PlayOneShot("event:/Ui/UiPress", default);
    }

    public void UiHover()
    {
        
    }
}
