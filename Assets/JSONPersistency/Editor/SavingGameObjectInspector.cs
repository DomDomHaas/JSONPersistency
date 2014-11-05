using UnityEngine;
using UnityEditor;

using System.Collections; 
using System.Xml.Serialization; 

[CustomEditor(typeof(SavingGameObject))]
public class SavingGameObjectInspector : Editor
{ 

		public float windowHeight = 16;
		public Rect myGUIRect;

		private SavingGameObject myPersist;
	
		[ExecuteInEditMode]
		public void OnEnable ()
		{
				myPersist = target as SavingGameObject;

				if (!myPersist.isInit) {
						myPersist.init ();
				}

		}

		public override void OnInspectorGUI ()
		{    
				base.DrawDefaultInspector ();


				myGUIRect = GUILayoutUtility.GetRect (Screen.width, windowHeight);
			
				EditorGUILayout.BeginHorizontal ();

				if (GUILayout.Button ("Load")) { 
						myPersist.load ();
				} 

				if (GUILayout.Button ("Save")) { 
						myPersist.save ();
				} 

				EditorGUILayout.EndHorizontal ();
		
		} 
	

} 


