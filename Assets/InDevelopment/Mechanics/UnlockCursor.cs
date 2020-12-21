using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


}
