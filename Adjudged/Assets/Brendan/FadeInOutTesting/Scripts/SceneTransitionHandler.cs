using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour {
	
	public FadeInOut m_fadeInOut;

	protected string m_currentTargetScene;
	protected bool m_isExiting = false;

	public bool m_loadAsync = false;

	// LoadAsync mutex vairables
	protected Mutex m_loadAsyncMutex = new Mutex();
	protected bool m_loadAsyncComplete = false;
	protected bool m_waitingOnFade = false;
	// LoadAsync mutex vairables


	// Use this for initialization
	void Start ()
	{
		if (m_fadeInOut != null)
		{
			m_fadeInOut.m_delegatefadeInComplete += FadeInCompleteCallback;
			m_fadeInOut.m_delegatefadeOutComplete += FadeOutCompleteCallback;
		}
		else
		{
			Debug.LogError("SceneTransitionHandler - requires FadeInOut script");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public IEnumerator LoadAsync(string sceneName)
	{
		m_loadAsyncMutex.WaitOne();
		m_loadAsyncComplete = false;
		m_loadAsyncMutex.ReleaseMutex();
		
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		asyncLoad.allowSceneActivation = false;

		//Wait until the last operation fully loads to return anything
		while (asyncLoad.progress < 0.9f)
		{
			Debug.Log("LoadAsync[" + asyncLoad.isDone + "]: " + asyncLoad.progress);
			yield return null;
		}
		m_loadAsyncMutex.WaitOne();
		m_loadAsyncComplete = true;
		bool fadeCheck = m_waitingOnFade;
		m_loadAsyncMutex.ReleaseMutex();

		while (fadeCheck)
		{
			m_loadAsyncMutex.WaitOne();
			fadeCheck = m_waitingOnFade;
			m_loadAsyncMutex.ReleaseMutex();
			yield return null;
		}

		asyncLoad.allowSceneActivation = true;
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(m_currentTargetScene));
		Debug.Log("LoadAsync done");
	}

	/// <summary>
	/// this will fade out the scene and eventually switch scenes
	/// </summary>
	/// <param name="sceneName"></param>
	public void TransitionToScene(string sceneName)
	{
		m_currentTargetScene = sceneName;
		m_isExiting = true;
		if (m_fadeInOut != null)
		{
			m_fadeInOut.m_fadeDirection = true;
		}
		else
		{
			Debug.LogError("SceneTransitionHandler - requires FadeInOut script");
		}
		if (m_loadAsync)
		{
			m_loadAsyncMutex.WaitOne();
			m_loadAsyncComplete = false;
			m_waitingOnFade = true;
			m_loadAsyncMutex.ReleaseMutex();
			StartCoroutine(LoadAsync(sceneName));
		}
	}



	/// <summary>
	/// this will actually change scenes
	/// </summary>
	public void ChangeScene(string sceneName = "")
	{
		if (sceneName.Length > 0)
		{
			m_currentTargetScene = sceneName;
		}
		m_isExiting = false;
		if (!m_loadAsync)
		{
			SceneManager.LoadScene(m_currentTargetScene);
		}
	}

	public void FadeInCompleteCallback()
	{
		print("SceneTransitionHandler - finished fading in");
	}

	public void FadeOutCompleteCallback()
	{
		print("SceneTransitionHandler - finished fading out");
		if (m_loadAsync)
		{
			m_loadAsyncMutex.WaitOne();
			m_waitingOnFade = false;
			m_loadAsyncMutex.ReleaseMutex();
		}
		else
		{
			ChangeScene();
		}
	}

	public bool IsInExiting()
	{
		return m_isExiting;
	}
}
