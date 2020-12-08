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
            cam = FindObjectOfType<Camera>();
            menuUI.worldCamera = cam;
            playerUI.worldCamera = cam;
        }
    }
}
