using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public PictureManagerScript m_pictureManager;

	public Camera m_cam;

	public Light m_cameraFlash;
	public AnimationCurve m_flashIntensityCurve;
	public float m_cameraIntensityMultiplier = 1;
	bool m_flashAnimationPlaying = false;
	float m_flashAnimationTime = 0;

	[Tooltip("This will keep the camera rendering for any active render textures")]
	public bool m_isRendering = false;

	[Tooltip("This will manually render a frame ONCE")]
	public bool m_debugManualRender = false;

	// Use this for initialization
	void Start () {
		if (m_cam == null)
		{
			m_cam = GetComponentInChildren<Camera>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_cam != null)
		{
			m_cam.enabled = m_isRendering;
			if (m_debugManualRender)
			{
				Render();
				m_debugManualRender = false;
			}
			UpdateCameraFlash();
		}
	}

	public void UpdateCameraFlash()
	{
		if (m_cameraFlash != null && m_flashIntensityCurve.length > 0)
		{
			if (m_flashAnimationPlaying)
			{
				m_cameraFlash.intensity = m_flashIntensityCurve.Evaluate(m_flashAnimationTime) * m_cameraIntensityMultiplier;
				m_flashAnimationTime += Time.deltaTime;
				if (m_flashAnimationTime >= m_flashIntensityCurve[m_flashIntensityCurve.length - 1].time)
				{
					m_flashAnimationPlaying = false;
					m_flashAnimationTime = 0;
				}
			}
			else
			{
				m_cameraFlash.intensity = m_flashIntensityCurve[m_flashIntensityCurve.length - 1].value * m_cameraIntensityMultiplier;
				m_flashAnimationTime = 0;
			}
		}
	}

	/// <summary>
	/// This will manually render a frame
	/// </summary>
	public void Render()
	{
		if (m_cam != null && !m_isRendering)
		{
			m_cam.Render();
		}
	}

	public void TakePhoto()
	{
		if (m_pictureManager != null)
		{
			m_flashAnimationPlaying = true;
			UpdateCameraFlash();
			Render();
			m_pictureManager.CreatePicture();
		}
	}
}
