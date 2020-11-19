using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSmoke : MonoBehaviour
{
    public Material smoke;

    // Start is called before the first frame update
    void Start()
    {
       // smoke = GetComponent<>();
    }

    // Update is called once per frame
    void Update()
    {
        smoke.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
