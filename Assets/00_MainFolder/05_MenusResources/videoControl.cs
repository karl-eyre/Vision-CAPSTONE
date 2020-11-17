using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class videoControl : MonoBehaviour
{
    public GameObject fadePanel;
    public Animator fader;
    public AnimationClip clip;
    public VideoPlayer videoPlayer;


    // Start is called before the first frame update
    void Start()
    {
        if (fader == null)
        {
            fader = fadePanel.GetComponent<Animator>();
        }
        StartCoroutine(endOfVideo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            EndScene();
        }
    }

    IEnumerator startgame(float seconds, string targetScene)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(targetScene);
    }

    IEnumerator endOfVideo()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length - clip.length);
        EndScene();
    }

    public void EndScene()
    {
        fader.SetBool("ClearFade", true);
        StartCoroutine(startgame(clip.length, "2_BuildingLevel"));
    }
}
