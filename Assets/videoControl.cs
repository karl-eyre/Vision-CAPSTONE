using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class videoControl : MonoBehaviour
{
    public GameObject fadePanel;
    public Animator fader;


    // Start is called before the first frame update
    void Start()
    {
        if (fader == null)
        {
            fader = fadePanel.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.isPressed)
        {
            fader.SetBool("ClearFade", true);
            StartCoroutine(startgame(2, "2_BuildingLevel"));
        }
    }

    IEnumerator startgame(float seconds, string targetScene)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(targetScene);
    }
}
