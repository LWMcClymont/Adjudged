using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestButtonCallback))]
[CanEditMultipleObjects]
public class TestButtonCallbackEditor : Editor {


	public override void OnInspectorGUI()
	{
		base.DrawDefaultInspector();
		if (GUILayout.Button("Do The Thing!"))
		{
			((TestButtonCallback)target).DoTheThing();
		}
	}
}
