using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinationLockScript : MonoBehaviour {

	[Tooltip("This will display the current code (Mostly for debugging)")]
	public Text m_displayText;

	public Light m_lockedLight;
	public Light m_unlockedLight;

	public string m_code = "0000";
	public string m_currentInput = "";
	public int m_inputMaxChar = 4;

	public bool m_locked = true; // want the default state to be locked

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_displayText != null)
		{
			m_displayText.text = m_currentInput;
		}
		if (m_lockedLight != null)
		{
			m_lockedLight.enabled = IsLocked();
		}
		if (m_unlockedLight != null)
		{
			m_unlockedLight.enabled = !IsLocked();
		}
	}

	public void ButtonPressed(string buttonText)
	{
		if (m_currentInput.Length < m_inputMaxChar)
		{
			m_currentInput += buttonText;
		}
	}

	public void ClearInput()
	{
		m_currentInput = "";
	}


	/// <summary>
	/// returns if the correct code has been entered and unlocked
	/// </summary>
	/// <returns></returns>
	public bool IsLocked()
	{
		if (m_currentInput == m_code)
		{
			return false;
		}
		return true;
	}
}
