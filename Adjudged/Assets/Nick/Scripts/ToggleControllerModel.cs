using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VRTK.Examples
{
	public class controller : MonoBehaviour {



		public GameObject controllerModel;

		// Use this for initialization
		void Start()
		{
			GetComponent< VRTK_InteractGrab > ().ControllerStartGrabInteractableObject += DoGrabOn;
			GetComponent< VRTK_InteractGrab > ().ControllerUngrabInteractableObject += DoGrabOff;

		}

		private void DoGrabOn(object sender, ObjectInteractEventArgs e)
		{
			controllerModel.SetActive(false);
		}

		private void DoGrabOff(object sender, ObjectInteractEventArgs e)
		{
			controllerModel.SetActive(true);
		}
	}
}