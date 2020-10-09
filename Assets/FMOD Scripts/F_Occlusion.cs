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

    Animator animator;

    public EventInstance music;

    [SerializeField]
    private LayerMask lm;

    private RaycastHit hit;
    public Transform player;

    bool patroling;

    void Start()
    {
        animator = GetComponent<Animator>();
        music = RuntimeManager.CreateInstance(eventPath);
        RuntimeManager.AttachInstanceToGameObject(music, transform, GetComponent<Rigidbody>());
        music.start();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= lookRadius)
        {
            Occlusion();
            Lowpass();
            animation();
        }
        else
        {
            music.setParameterByName("LowPass", 0, false);
        }
    }

    void animation()
    {
        if (patroling == false)
        {
            RuntimeManager.PlayOneShotAttached("event:/Enemies/Searching", this.gameObject);
            animator.SetBool("Start", true);
            patroling = true;
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
        if (hit.collider == null)
        {
            Debug.Log("No wall");
            music.setParameterByName("LowPass", 0, false);
        }
    }
}
