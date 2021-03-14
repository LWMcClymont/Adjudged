using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateText : MonoBehaviour {

    public HUD hud;
    public int textIndex = 0;
    public float disappearTime = 10;

    private float curTime = 0;
    private bool displayingText = false;

    private bool textAlreadyDisplayed = false;
    
    public void ShowText()
    {
        if (textAlreadyDisplayed)
        {
            return;
        }

        displayingText = true;
        hud.DisplayMessage(textIndex);

        textAlreadyDisplayed = true;
    }

    private void Update()
    {
        if (displayingText)
        {
            curTime += Time.deltaTime;

            if (curTime >= disappearTime)
            {
                hud.ClearText();
                displayingText = false;
            }
        }
    }

}
