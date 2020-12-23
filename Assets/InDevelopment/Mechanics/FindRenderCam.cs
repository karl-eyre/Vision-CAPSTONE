using System;
using UnityEngine;
using UnityEngine.UI;

namespace InDevelopment.Mechanics
{
    public class FindRenderCam : MonoBehaviour
    {
        public Canvas menuUI;
        public Canvas playerUI;

        public Camera cam;

        public void Awake()
        {
            //finds the render camera for the menu manager and the ui elements
            cam = FindObjectOfType<Camera>();
            menuUI.worldCamera = cam;
            playerUI.worldCamera = cam;
        }
    }
}
