using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FPS : MonoBehaviour {

    public Text text;

    public bool display = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            display = !display;
        }

        
        text.text = display ? ("FPS: " + Time.frameCount / Time.time) : "";	
	}

    private void OnGUI()
    {
        GUI.Label(new Rect(500, 10, 100, 20), "FPS: " + Time.frameCount / Time.time);
    }
}
