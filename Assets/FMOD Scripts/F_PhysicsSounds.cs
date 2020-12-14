using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class F_PhysicsSounds : MonoBehaviour
{
    private bool impact;

    [FMODUnity.EventRef]
    public string eventPath;

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.relativeVelocity.magnitude > 3.5)
        {
            RuntimeManager.PlayOneShotAttached(eventPath, this.gameObject);
        }
    }

    IEnumerator ImpactReset()
    {
        yield return new WaitForSeconds(1);
        impact = false;

    }
}
