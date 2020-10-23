using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class TriggerMovingSound : MonoBehaviour
{
    [EventRef]
    public string eventpath;

    public Animator animator;
    GameObject player;
    bool played;
    [SerializeField]
    GameObject ambientSound;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && played == false)
        {
            animator.SetBool("Play", true);
            RuntimeManager.PlayOneShotAttached(eventpath, ambientSound);
            played = true;
        }
    }
}
