using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialCombinationLock))]
public class DialCombinationLockEditor : Editor
{
	void OnEnable()
	{
		
	}

	public override void OnInspectorGUI()
	{
		DialCombinationLock script = (DialCombinationLock)target;
		DrawDefaultInspector();
		EditorGUILayout.LabelField("Editor Script Values");
		{
			EditorGUILayout.PrefixLabel(new GUIContent("Display Text", "\"{lockedBool}\", \"{LockedString}\", \"{currentNumber}\", \"{dialAngle}\", \"{dialDir}\", \"{DDialDir}\", \"{index}\", \"{neededNumber}\""));
			string newDisT = EditorGUILayout.TextArea(script.m_displayTextString);
			if (newDisT != script.m_displayTextString)
			{
				Undo.RecordObject(target, "Changed Display Text");
				script.m_displayTextString = newDisT;
				EditorUtility.SetDirty(target);
				Debug.Log("text changed");
			}
		}
		{
			EditorGUILayout.PrefixLabel(new GUIContent("Verbose Display Text", "\"{lockedBool}\", \"{LockedString}\", \"{currentNumber}\", \"{dialAngle}\", \"{dialDir}\", \"{DDialDir}\", \"{index}\", \"{neededNumber}\""));
			string newDisT = EditorGUILayout.TextArea(script.m_displayTextStringVerbose);
			if (newDisT != script.m_displayTextStringVerbose)
			{
				Undo.RecordObject(target, "Changed Verbose Display Text");
				script.m_displayTextStringVerbose = newDisT;
				EditorUtility.SetDirty(target);
			}
		}
	}

}
