using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK.Examples
{
	public class torch : VRTK_InteractableObject
	{

		private bool isTorchOn = false;
		public GameObject lightSource;

        void Start()
        {
            lightSource.SetActive(false);
        }

        public override void StartUsing(VRTK_InteractUse usingObject)
		{
			base.StartUsing(usingObject);
			ToggleTorch();
		}

		void ToggleTorch()
		{
			isTorchOn = !isTorchOn;
			lightSource.SetActive(isTorchOn);
		}
	}
}