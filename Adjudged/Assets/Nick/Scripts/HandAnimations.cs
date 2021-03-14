using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimations : MonoBehaviour {


    private Animator animator;
    private SteamVR_Controller.Device controller;

    private bool hovering;

    public float fadeTime;
    private float timer;
    private Material handMaterial;

    private bool triggerIsDown = false;
    private float alpha;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        controller = GetComponentInParent<Valve.VR.InteractionSystem.Hand>().controller;
        handMaterial = GetComponentInChildren<Renderer>().material;

    }
	
	// Update is called once per frame
	void Update ()
    {

        if (controller.GetHairTriggerDown() && hovering)
        {
            triggerIsDown = true;
            animator.SetBool("Grab", triggerIsDown);
            //Debug.Log("grabing");
        }
        else if(controller.GetHairTriggerDown() && !hovering)
        {
            triggerIsDown = true;
            animator.SetBool("Grab", triggerIsDown);
            //Debug.Log("pointing");

        }
        else if (controller.GetHairTriggerUp())
        {
            triggerIsDown = false;
            animator.SetBool("Grab", triggerIsDown);
            animator.SetBool("Point", triggerIsDown);
            //Debug.Log("idling");

        }

        if (triggerIsDown)
            timer += Time.deltaTime;
        else
            timer -= Time.deltaTime;

        if (timer > fadeTime)
            timer = fadeTime;
        if (timer < 0)
            timer = 0;

        alpha = 1 - timer/fadeTime;

        Color tempColour = handMaterial.color;
        tempColour.a = alpha;
        handMaterial.color = tempColour;

       // Debug.Log("alpha: " + alpha);

    }

    void OnParentHandHoverBegin()
    {
        hovering = true;
        //Debug.Log("Start hoving");

    }

    void OnParentHandHoverEnd()
    {
        hovering = false;
       // Debug.Log("End hoving");

    }


}

