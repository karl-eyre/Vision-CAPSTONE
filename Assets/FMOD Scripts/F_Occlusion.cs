using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class F_Occlusion : MonoBehaviour
{
    public float lookRadius = 30f;

    [FMODUnity.EventRef]
    public string eventPath;

    public EventInstance music;

    [SerializeField]
    private LayerMask lm;

    private RaycastHit hit;
    public Transform player;

    void Start()
    {
        music = RuntimeManager.CreateInstance(eventPath);
        music.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        music.start();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void Update()
    {
        Lowpass();
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= lookRadius)
        {
            Occlusion();
        }
    }

    void Occlusion()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        Vector3 directionToFace = player.position - transform.position;
        Physics.Raycast(transform.position, directionToFace, out hit, dist, lm);
        Debug.DrawRay(transform.position, directionToFace, Color.red);
    }

    void Lowpass()
    {
        if (hit.collider)
        {
            if (hit.collider.gameObject.tag == "Wall")
            {
                Debug.Log("wall");
                music.setParameterByName("LowPass", 1, false);
            }             
        }
        else
        {
            Debug.Log("No wall");
            music.setParameterByName("LowPass", 0, false);
        }
    }
}
