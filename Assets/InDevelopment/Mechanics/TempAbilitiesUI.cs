using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.InputSystem;

public class TempAbilitiesUI : MonoBehaviour
{

    [Header("Teleport")]
    public Image TeleportUI;
    public float cooldown1 = 5;
    bool isCooldown = false;
    public KeyCode teleport;
    

    // Start is called before the first frame update
    void Start()
    {
        TeleportUI.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Teleport();
    }

    void Teleport()
    {

        if(Keyboard.current.eKey.isPressed && isCooldown == false)
        {
            isCooldown = true;
            TeleportUI.fillAmount = 1;
        }

        if (isCooldown)
        {
            TeleportUI.fillAmount -= 1 / cooldown1 * Time.deltaTime;

            if(TeleportUI.fillAmount <= 0)
            {
                TeleportUI.fillAmount = 0;
                isCooldown = false;
            }
        }

    }
}
