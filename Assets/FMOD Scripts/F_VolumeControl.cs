using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class F_VolumeControl : MonoBehaviour
{
    Bus bus;

    float busVolume;
    float busFinalVolume;

    Slider slider;

    [SerializeField]
    private string busBath;
    void Start()
    {
        slider = GetComponent<Slider>();

        bus = RuntimeManager.GetBus("bus:/" + busBath);
        bus.getVolume(out busVolume, out busFinalVolume);

        slider.value = busVolume;
    }
    public void SliderVolume(float sliderValue)
    {
        bus.setVolume(sliderValue);
    }
}
