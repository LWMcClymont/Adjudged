using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Reference to the HUD script
    public HUD manager;

    // Each message
    bool first = false;

    void Update()
    {
        // Display messages accordingly

        if (Time.time > 1 && !first)
        {
            first = true;

            manager.DisplayMessage(0);
        }
    }
    
}
