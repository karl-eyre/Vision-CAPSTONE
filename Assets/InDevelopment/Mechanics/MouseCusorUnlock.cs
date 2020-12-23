using System;
using UnityEngine;

namespace InDevelopment.Mechanics
{
    public class MouseCusorUnlock : MonoBehaviour
    {
        private void Start()
        {
            //used in the main menu scene to unlock the cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
