﻿using UnityEngine;
using UnityEditor;

using System.Collections; 
using System.Xml.Serialization; 

[CustomEditor(typeof(SavingGameObject))]
public class SavingGameObjectInspector : JSONPersitentInspector
{ 

		[ExecuteInEditMode]
		public void OnEnable ()
		{
				base.OnEnable ();
		}

		public override void OnInspectorGUI ()
		{    
				base.OnInspectorGUI ();


/*				myGUIRect = GUILayoutUtility.GetRect (Screen.width, windowHeight);
			
				EditorGUILayout.BeginHorizontal ();

				if (GUILayout.Button ("Load")) { 
						myPersist.load ();
				} 

				if (GUILayout.Button ("Save")) { 
						myPersist.save ();
				} 

				EditorGUILayout.EndHorizontal ();
*/		
		} 
	

} 


