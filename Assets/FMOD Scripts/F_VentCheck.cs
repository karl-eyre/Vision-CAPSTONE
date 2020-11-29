using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_VentCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            F_Ext_Ambience.extAmb.setParameterByName("InVent", 1, false);
        }
    }
}
