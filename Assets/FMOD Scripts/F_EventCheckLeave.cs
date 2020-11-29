using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_EventCheckLeave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            F_Ext_Ambience.extAmb.setParameterByName("InVent", 0, false);
        }
    }
}
