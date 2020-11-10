using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class F_Ambience : MonoBehaviour
{
    private EventDescription eventDescription;
    private PARAMETER_DESCRIPTION[] pDS = new PARAMETER_DESCRIPTION[4];
    public static PARAMETER_ID[] pIDS = new PARAMETER_ID[4];
    public static EventInstance amb;
    void Start()
    {
        GetParameterIDs();
        amb = RuntimeManager.CreateInstance("event:/Ambience/AmbInterior");
        amb.start();
        amb.release();
    }
    void GetParameterIDs()
    {
        eventDescription = RuntimeManager.GetEventDescription("event:/Ambience/AmbInterior");
        eventDescription.getParameterDescriptionByName("Drone", out pDS[0]);
        eventDescription.getParameterDescriptionByName("CityOutside", out pDS[1]);
        eventDescription.getParameterDescriptionByName("RattlingFan", out pDS[2]);
        eventDescription.getParameterDescriptionByName("RoomToneHallway", out pDS[3]);
        
        for (int i = 0; i < pDS.Length; i++)
        { 
            pIDS[i] = pDS[i].id;
        }
    }
    
}
