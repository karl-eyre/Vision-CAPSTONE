using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuCinemachine : MonoBehaviour
{
    public GameControls controls;
    public CinemachineVirtualCamera activeCam;
    public CinemachineVirtualCamera mainCam;

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = activeCam;
        activeCam.Priority++;
    }

    private void Start()
    {
        SetupControls();
    }

    private void SetupControls()
    {
        controls = new GameControls();
        controls.Enable();
        controls.Menu.OpenPauseMenu.performed += Escape;
    }


    //return to mainMenu
    public void Escape(InputAction.CallbackContext obj)
    {
        activeCam.Priority--;

        activeCam = mainCam;

        activeCam.Priority++;
    }

    //Change Camera
    public void ChangeCam(CinemachineVirtualCamera target)
    {
        activeCam.Priority--;

        activeCam = target;

        activeCam.Priority++;
    }

}
