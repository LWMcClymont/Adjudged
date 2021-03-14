using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTriggerScript : MonoBehaviour {

	public Animator m_animator;

	public string m_triggerName = "ButtonPressed";

	[Tooltip("this will trigger the animation ONCE")]
	public bool m_debugTrigger = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (m_debugTrigger)
		{
			TriggerAnimation();
			m_debugTrigger = false;
		}
	}

	public void TriggerAnimation()
	{
		if (m_animator != null)
		{
			m_animator.SetBool(m_triggerName, true);
		}
	}
}
