using UnityEngine;
using UnityEditor;
using System.Collections; 
using System.Xml.Serialization; 

namespace JSONPersistency
{

	[CustomEditor(typeof(JSONPersistent))]
	public class JSONPersitentInspector : Editor
	{ 

		public float windowHeight = 16;
		public Rect myGUIRect;

		private JSONPersistent myPersist;

		[ExecuteInEditMode]
		public void OnEnable ()
		{
			myPersist = target as JSONPersistent;		
			myPersist.init ();
		}

		public void checkPersist ()
		{
			Debug.Log ("checkPersist");
		}

		public override void OnInspectorGUI ()
		{    
			base.DrawDefaultInspector ();

			myPersist = target as JSONPersistent;

			if (myPersist.persistentIDisSet ()) {
				EditorGUILayout.LabelField ("PersistentID: " + myPersist.getPersistentID ());
			} else {
				EditorGUILayout.LabelField ("PersistentID: not loaded yet (save the scene first)");
			}

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


}