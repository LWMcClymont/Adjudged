using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Valve.VR;
using Valve.VR.InteractionSystem;

public class ViveObjectFixedGrab : MonoBehaviour {

	public enum VOFGFallback
	{
		VOFG_Left,
		VOFG_Right,
		VOFG_DefaultAttach
	}

	public GameObject m_leftHandAttachPoint;
	public GameObject m_rightHandAttachPoint;
	[Tooltip("If there are no attachment points or the hands don't know what they are, it will use this")]
	public VOFGFallback m_fallbackAttachPoint = VOFGFallback.VOFG_DefaultAttach;

	[Tooltip("If active fix, the script will move the object to the attach point EVERY frame, not just when attached")]
	public bool m_activeFix = false;

	bool m_beingHeld = false;
	Hand m_hand = null;

    //debug variables

    //display variables

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	// LateUpdate is called once per frame after Update
	void LateUpdate()
	{
		if (m_beingHeld && m_activeFix)
		{
			if (m_hand != null)
			{
                UpdateObject();
			}
			else
			{
				Debug.Log("ViveObjectFixedGrab doesn't have a hand");
			}
		}
	}

	void MoveToAttachPoint(GameObject point)
	{
        Vector3 pos = point.transform.localPosition;
        Vector3 s = point.transform.lossyScale;
        pos.Scale(s);
        transform.localPosition = -pos;
        transform.localRotation = point.transform.localRotation;
	}

    void UpdateObject()
    {
        switch (m_hand.GuessCurrentHandType())
        {
            case Hand.HandType.Left:
                MoveToAttachPoint(m_leftHandAttachPoint);
                break;
            case Hand.HandType.Right:
                MoveToAttachPoint(m_rightHandAttachPoint);
                break;
            default:
                switch (m_fallbackAttachPoint)
                {
                    case VOFGFallback.VOFG_Left:
                        MoveToAttachPoint(m_leftHandAttachPoint);
                        break;
                    case VOFGFallback.VOFG_Right:
                        MoveToAttachPoint(m_rightHandAttachPoint);
                        break;
                }
                break;
        }
    }

	// function called by the SteamVR hand script when the object is attached
	public void OnAttachedToHand(Hand hand)
	{
		m_beingHeld = true;
		m_hand = hand;
		if (m_hand != null && !m_activeFix)
		{
            UpdateObject();
        }
		else
		{
			Debug.Log("ViveObjectFixedGrab doesn't have a hand");
		}
	}

	// function called by the SteamVR hand script when the object is detached
	public void OnDetachedFromHand(Hand hand)
	{
		m_beingHeld = false;
		m_hand = null;
	}
}
