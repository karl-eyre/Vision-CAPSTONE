using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSmoke : MonoBehaviour
{
    public Material smoke;
    //public float darkness;
    public bool fadeIn = false;
    //float fadeSpeed = 0.015f;

    
    float fadeDuration = 2;
    float startTrans = 0f;
    float endTrans = 4.7f;
    public float ValueOfLerp;


    // Start is called before the first frame update
    void Start()
    {
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
        fadeIn = !fadeIn;

        if(fadeIn == true)
        {
            startTrans = endTrans;
            endTrans = 0;
        }
        else
        {
            endTrans = startTrans;
            startTrans = 0;
        }

        StartCoroutine(fadeInSmoke());
    }

    IEnumerator fadeInSmoke()
    {
        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            ValueOfLerp = Mathf.Lerp(startTrans, endTrans, currentTime / fadeDuration);
            smoke.SetFloat("_TransparencyScale", ValueOfLerp);
            currentTime += Time.unscaledDeltaTime;
            yield return null;           
        }
        ValueOfLerp = endTrans;
        smoke.SetFloat("_TransparencyScale", ValueOfLerp);
    }
}
