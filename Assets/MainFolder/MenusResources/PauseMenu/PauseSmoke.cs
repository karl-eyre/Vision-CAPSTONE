using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSmoke : MonoBehaviour
{
    public Material smoke;
    //public float darkness;
    public bool fadeIn = false;
    //float fadeSpeed = 0.015f;

    public GameObject fadePanel;
    public Animator fadePanelController;
    
    float fadeDuration = 2;
    float startTrans = 0f;
    float endTrans = 4.7f;
    public float ValueOfLerp;

    // Start is called before the first frame update
    void Start()
    {
        fadePanelController = fadePanel.GetComponent<Animator>();
        smoke.SetFloat("_TransparencyScale", 0);
    }

    // Update is called once per frame
    void Update()
    {
        smoke.SetFloat("_UnscaledTime", Time.unscaledTime);


        /*if (fadeIn == true)
        {
            float t = smoke.GetFloat("_TransparencyScale");
            if(t < darkness)
            {
                smoke.SetFloat("_TransparencyScale", t += fadeSpeed);
            }
        }
        else
        {
            float t = smoke.GetFloat("_TransparencyScale");
            if (t > 0)
            {
                smoke.SetFloat("_TransparencyScale", t -= fadeSpeed);
            }
        }*/
    }

    /*public void startFade()
    {
        fadeIn = true;
    }*/

    public void fadeSmoke()
    {
        StartCoroutine(fadeInSmoke());
    }

    IEnumerator fadeInSmoke()
    {
        fadeIn = !fadeIn;

        float currentTime = 0;
        float s;
        float e;

        fadePanelController.SetBool("Fade?", fadeIn);

        if (fadeIn == true)
        {
            s = startTrans;
            e = endTrans;
        }
        else
        {
            e = startTrans;
            s = endTrans;
        }

        while (currentTime < fadeDuration)
        {
            ValueOfLerp = Mathf.Lerp(s,e, currentTime / fadeDuration);
            smoke.SetFloat("_TransparencyScale", ValueOfLerp);
            currentTime += Time.unscaledDeltaTime;
            yield return null;           
        }
        ValueOfLerp = e;
        smoke.SetFloat("_TransparencyScale", ValueOfLerp);

        /*if (fadeIn == true)
        {
            startTrans = endTrans;
            endTrans = 0;
        }
        else
        {
            endTrans = startTrans;
            startTrans = 0;
        }*/
    }
}
