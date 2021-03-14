using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestButtonCallback : MonoBehaviour {

	[Tooltip("These are the functions that will be called when the button is pressed")]
	public UnityEvent m_buttonEvent;

	public void DoTheThing()
	{
		m_buttonEvent.Invoke();
	}
}
