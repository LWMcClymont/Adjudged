using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class DialCombinationLock : MonoBehaviour {

    public enum DialDir
    {
        DD_AntiClockwise,
        DD_Clockwise,
        DD_Default
    }

	public int m_numberCount = 4;

	public HingeJoint m_dialHinge; // hinge angle ranges from -180 to 180 with 0 being rest angle

	public FreezeableCircularDrive m_circularDrive;

	public VRAxisRotationScript m_CRAxisRotationScript;

	public Hand m_grabbedHand = null;

	public bool m_useHapticFeedback = true;
	public ushort m_hapticFeedbackDurationMicroseconds = 1000;
    public ushort m_hapticFeedbackCorrectDurationMicroseconds = 2000;

    public int m_lastNumber;
	public int m_currentNumber;
	[Tooltip("<0 is anti-clockwise, 0 is none, >0 is clockwise")]
	public DialDir m_dialDirection = DialDir.DD_Default;
    bool m_dialMoved = false; // if the dial has been rotated to a new number

	public List<int> m_combination;
	public int m_comboIndex;
	[Tooltip("The amount of time that the dial must stay on the number before it's accepted")]
	public float m_comboHoldTime = 0.1f;
	public float m_comboHoldTimeCurrent = 0;
	public bool m_resetOnWrongNumber = false;
    public bool m_needDirection = true;

	public bool m_locked = true;

	public UnityEvent m_onLock;
	public UnityEvent m_onUnlock;

	List<string> m_keywords = new List<string> {
			"{lockedBool}", "{LockedString}", "{currentNumber}", "{dialAngle}", "{dialDir}", "{DDialDir}", "{index}", "{neededNumber}"
		};

	[HideInInspector]
	[Tooltip("\"{lockedBool}\", \"{LockedString}\", \"{currentNumber}\", \"{dialAngle}\", \"{dialDir}\", \"{DDialDir}\", \"{index}\", \"{neededNumber}\"")]
	public string m_displayTextString =
					"Locked: {LockedString}" +
					"\nNumber: {currentNumber}" +
					"\nNeeded number: {neededNumber}" +
					"\nNeeded Direction: {DDialDir}"
		;

	[HideInInspector]
	[Tooltip("\"{lockedBool}\", \"{LockedString}\", \"{currentNumber}\", \"{dialAngle}\", \"{dialDir}\", \"{DDialDir}\", \"{index}\", \"{neededNumber}\"")]
	public string m_displayTextStringVerbose =
					"Locked: {LockedString}" +
					"\nNumber: {currentNumber}" +
					"\nAngle: {dialAngle}" +
					"\nDir: {dialDir}" +
					"\nDDir: {DDialDir}" +
					"\nIndex: {index}" +
					"\nNeeded number: {neededNumber}"
		;

	public string m_clockwiseSymbol = "->";
	public string m_antiClockwiseSymbol = "<-";

	// debug

	public bool m_verboseDisplayText = false;
	public Text m_numberDisplayText;


	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		int newNumber = GetNumberAtAngle(GetDialAngle());
		if (newNumber != m_currentNumber)
		{
			m_lastNumber = m_currentNumber;
			m_currentNumber = newNumber;
			m_dialDirection = CalculateDialDirection(m_lastNumber, m_currentNumber);
            m_dialMoved = true;
			if (m_useHapticFeedback && m_grabbedHand != null)
			{
                if (newNumber == m_combination[m_comboIndex])
                {
					m_grabbedHand.controller.TriggerHapticPulse(m_hapticFeedbackCorrectDurationMicroseconds);
				}
                else
                {
					m_grabbedHand.controller.TriggerHapticPulse(m_hapticFeedbackDurationMicroseconds);
				}
			}
		}

		PollLock();

		// debug
		if (m_numberDisplayText != null)
		{
			if (m_verboseDisplayText)
			{
				m_numberDisplayText.text = BetterReplaceKeywords(m_displayTextStringVerbose);
				//m_numberDisplayText.text =
				//	"Locked: " + m_locked +
				//	"\nNumber: " + m_currentNumber +
				//	"\nAngle: " + GetDialAngle() +
				//	"\nDir: " + getDialDirection() +
				//	"\nDDir: " + GetDesiredDir(m_comboIndex) +
				//	"\nIndex: " + m_comboIndex +
				//	"\nCurrent number: " + m_combination[m_comboIndex]
				//	;
			}
			else
			{
				m_numberDisplayText.text = BetterReplaceKeywords(m_displayTextString);
				//m_numberDisplayText.text =
				//	"Locked: " + m_locked +
				//	"\nNumber: " + m_currentNumber +
				//	"\nCurrent number: " + m_combination[m_comboIndex] +
				//	"\nNeeded Direction: " + (GetDesiredDir(m_comboIndex) == DialDir.DD_AntiClockwise ? "<-" : "->")
				//	;
			}
		}
	}

	public DialDir CalculateDialDirection(int oldN, int newN)
	{
		int dir = newN - oldN;
		if (Math.Abs(dir) > 1)
		{
			dir = -dir;
		}
        return NumberToDialDir(dir);
        //return (dir < -1) ? -1 : (dir > 1) ? 1 : dir; // clamps value to between -1 and 1
	}

    public DialDir NumberToDialDir(int dir)
    {
        if (dir > 0)
        {
            return DialDir.DD_Clockwise;
        }
        else if (dir < 0)
        {
            return DialDir.DD_AntiClockwise;
        }
        else
        {
            return DialDir.DD_Default;
        }
    }

    public DialDir GetDesiredDir(int index)
    {
        return (index % 2 == 0) ? DialDir.DD_Clockwise : DialDir.DD_AntiClockwise; 
        //return NumberToDialDir(((index % 2) * 2) - 1);
    }

    public bool IsCorrectDialDirection(DialDir dir, DialDir desiredDir)
    {
        if (!m_needDirection || dir == desiredDir || dir == DialDir.DD_Default)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// is called when a number has been inputed as a combination
    /// </summary>
    public void NumberTicked()
    {
        m_dialMoved = false;
    }

    public void PollLock()
	{
		if (m_combination.Count > 0 && m_locked)
		{
            DialDir desiredDir = GetDesiredDir(m_comboIndex);
            if (!IsCorrectDialDirection(m_dialDirection, desiredDir) && m_dialMoved)
            {
				if (m_resetOnWrongNumber)
				{
					m_comboIndex = 0;
				}
            }
            else
            {
                if (m_currentNumber == m_combination[m_comboIndex])
                {
                    m_comboHoldTimeCurrent += Time.deltaTime;
                    if (m_comboHoldTimeCurrent >= m_comboHoldTime)
                    {
                        m_comboIndex++;
						if (m_comboIndex >= m_combination.Count)
						{
							m_comboIndex = m_combination.Count-1;
							Unlock();
						}
						NumberTicked();
                    }
                }
                else
                {
                    m_comboHoldTimeCurrent = 0;
                }
            }
		}

	}

	public void Lock()
	{
		m_locked = true;
		m_comboIndex = 0;
		m_onLock.Invoke();
	}

	public void Unlock()
	{
		m_locked = false;
		m_onUnlock.Invoke();
	}

	public DialDir getDialDirection()
	{
		return m_dialDirection;
	}

	/// <summary>
	/// returns the dial angle as a range of 0 to 360
	/// </summary>
	/// <returns></returns>
	public float GetDialAngle()
	{
		float angle = 0;
		if (m_dialHinge != null)
		{
			angle = m_dialHinge.angle;
		}
		else if (m_circularDrive != null)
		{
			angle = m_circularDrive.outAngle;
		}
		else if (m_CRAxisRotationScript != null)
		{
            angle = m_CRAxisRotationScript.ExternalGetAngleOfObject();
		}
		if (angle < 0)
		{
			int c = Mathf.CeilToInt((Mathf.Abs(angle) / 360));
			angle = (360*c) + angle;
		}
		return angle;
	}

	/// <summary>
	/// returns the angle of that number;
	/// ie. on a dial with 1 to 4, 2 would be at 90 degrees
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public float GetAngleAtNumber(int index)
	{
		return (m_numberCount > 0 ? (360 / m_numberCount) * index : 0);
		//float angle = 0;
		//if (m_numberCount > 0)
		//{
		//	angle = (360 / m_numberCount) * index;
		//}
		//return angle;
	}

	public int GetNumberAtAngle(float angle)
	{
		int number = 0;

		number = Mathf.RoundToInt(angle / (360 / m_numberCount));

		if (number < 0)
		{
			Debug.LogError("DialCombinationLock: GetNumberAtAngle got a negative number");
		}

		while (number > m_numberCount)
		{
			number -= m_numberCount;
		}

		if (number == m_numberCount)
		{
			number = 0;
		}

		return number;
	}



	public string ConvertKeywordToValue(string keyword)
	{
		for (int i = 0; i < m_keywords.Count; i++)
		{
			if (keyword == m_keywords[i])
			{
				return GetValueFromKeywordIndex(i);
			}
		}
		return "";
	}

	public string ReplaceKeywords(string input)
	{
		for (int i = 0; i < m_keywords.Count; i++)
		{
			input = input.Replace(m_keywords[i], ConvertKeywordToValue(m_keywords[i]));
		}
		return input;
	}

	public string GetValueFromKeywordIndex(int index)
	{
		switch (index) // "{lockedBool}", "{LockedString}", "{currentNumber}", "{dialAngle}", "{dialDir}", "{DDialDir}", "{index}", "{neededNumber}"
		{
			case 0:
				return "" + m_locked;
			case 1:
				return (m_locked ? "locked" : "unlocked");
			case 2:
				return "" + m_currentNumber;
			case 3:
				return "" + GetDialAngle();
			case 4:
				return DialDirEnumToString(getDialDirection());
			case 5:
				return DialDirEnumToString(GetDesiredDir(m_comboIndex));
			case 6:
				return "" + m_comboIndex;
			case 7:
				return "" + m_combination[m_comboIndex];
		}
		return "";
	}

	public string BetterReplaceKeywords(string input)
	{
		for (int i = 0; i < m_keywords.Count; i++)
		{
			input = input.Replace(m_keywords[i], GetValueFromKeywordIndex(i));
		}
		return input;
	}

	public string DialDirEnumToString(DialDir dde)
	{
		switch (dde)
		{
			case DialDir.DD_AntiClockwise:
				return m_antiClockwiseSymbol;
			case DialDir.DD_Clockwise:
				return m_clockwiseSymbol;
			case DialDir.DD_Default:
				return "";
		}
		return "";
	}

	/// <summary>
	/// should be called by Hand script when attached to hand
	/// </summary>
	public void OnAttachedToHand(Hand hand)
	{
		m_grabbedHand = hand;
	}

	/// <summary>
	/// should be called by Hand script when detached to hand
	/// </summary>
	public void OnDetachedFromHand(Hand hand)
	{
		m_grabbedHand = null;
	}

}
