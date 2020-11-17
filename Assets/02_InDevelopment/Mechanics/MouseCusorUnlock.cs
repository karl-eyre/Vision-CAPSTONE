using System;
using UnityEngine;

namespace InDevelopment.Mechanics
{
    public class MouseCusorUnlock : MonoBehaviour
    {
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
