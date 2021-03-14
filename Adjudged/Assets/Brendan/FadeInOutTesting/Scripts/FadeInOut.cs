using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour {

	public float m_fadeTime;
	[Tooltip("Between 0 and 1")]
	public float m_fadePercentage = 0;

	public bool m_fadeDirection = false;

	private Renderer rend;

	public delegate void FadeInCompleteDelegate();
	public delegate void FadeOutCompleteDelegate();

	public FadeInCompleteDelegate m_delegatefadeInComplete;
	public FadeInCompleteDelegate m_delegatefadeOutComplete;

	// Use this for initialization
	void Start ()
	{
		rend = GetComponent<Renderer>();
		UpdateTexture();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_fadePercentage > 1)
		{
			m_fadePercentage = 1;
		}
		else if (m_fadePercentage < 0)
		{
			m_fadePercentage = 0;
		}
		if (m_fadeDirection && m_fadePercentage < 1)
		{
			m_fadePercentage += Time.deltaTime;
			UpdateTexture();
			if (m_fadePercentage >= 1)
			{
				m_delegatefadeOutComplete.Invoke();
			}
		}
		else if (!m_fadeDirection && m_fadePercentage > 0)
		{
			m_fadePercentage -= Time.deltaTime;
			UpdateTexture();
			if (m_fadePercentage <= 0)
			{
				m_delegatefadeInComplete.Invoke();
			}
		}
	}

	protected void UpdateTexture()
	{
		Color c = rend.material.color;
		c.a = Mathf.Lerp(0, 1, m_fadePercentage);
		rend.material.color = c;
	}
}
