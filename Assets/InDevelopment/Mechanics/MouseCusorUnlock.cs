using System;
using UnityEngine;

namespace InDevelopment.Mechanics
{
    public class MouseCusorUnlock : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
