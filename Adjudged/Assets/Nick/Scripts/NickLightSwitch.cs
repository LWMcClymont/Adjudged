using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;




public class NickLightSwitch : MonoBehaviour
{
    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

    public GameObject switchModel;
    public List<GameObject> lightSources;

    public bool startingState = false;
    private bool isLightOn;

	public AudioSource m_lightOn;
	public AudioSource m_lightOff;


	// Use this for initialization
	void Start()
    {
        GameObject.FindObjectsOfType<Renderer>();
        isLightOn = startingState;

        for (int i = 0; i < lightSources.Count; i++)
        {
			if (lightSources[i] != null)
			{
				lightSources[i].SetActive(startingState);
			}
        }
    }

    private void HandHoverUpdate(Hand hand)
    {
        Debug.Log("Hand Hover Update");
        if (hand.GetStandardInteractionButtonDown() || ((hand.controller != null) && hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip)))
        {
            if (hand.currentAttachedObject != gameObject)
            {
                ToggleLight();
                FlipSwitch();
            }
        }
    }

    // Update is called once per frame
    void ToggleLight()
    {
        isLightOn = !isLightOn;

        for (int i = 0; i < lightSources.Count; i++)
        {
            lightSources[i].SetActive(isLightOn);
        }

		if (isLightOn)
		{
			if (m_lightOn != null)
			{
				m_lightOn.Play();
			}
		}
		else
		{
			if (m_lightOff != null)
			{
				m_lightOff.Play();
			}
		}

    }

    void FlipSwitch()
    {
        Vector3 scale = switchModel.transform.localScale;
        float y = scale.y * -1;
        switchModel.transform.localScale = new Vector3(scale.x, y, scale.z);
    }
}
