//using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using JSONPersistency;
using UnityEditor.Callbacks;
using System.Runtime.Serialization;

public class SaveSceneHook : AssetModificationProcessor
{


/*		[PostProcessScene]
		// is called after a scene start running!
		public static void OnPostprocessScene ()
		{ 
				Debug.Log ("OnPostprocessScene ");
		}*/

	
	static string[] OnWillSaveAssets (string[] paths)
	{

		Object[] objs = Component.FindObjectsOfType (typeof(JSONPersistent));

		//Debug.Log ("OnWillSaveAssets " + objs.Length);

		foreach (Object obj in objs) {
			JSONPersistent persist = (JSONPersistent)obj;
			if (!persist.persistentIDisSet ()) {
				persist.loadPersistentID ();
				//Debug.Log (persist.name + " loaded!  persistID: " + persist.getPersistentID ());

			} else {
				//Debug.Log (persist.name + " persistID: " + persist.getPersistentID ());
			}
		}

		return paths;
	}
	
} 
