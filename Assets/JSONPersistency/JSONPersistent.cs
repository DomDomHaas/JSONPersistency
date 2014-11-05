using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public abstract class JSONPersistent : MonoBehaviour
{
		/// <summary>
		/// Is required to save the json
		/// </summary>
		protected string fileName;

		/// <summary>
		/// The identifier don't change
		/// </summary>
		private int id = -1;

		public bool loadOnAwake = true;
		public bool saveOnDestroy = false;

		public bool isInit = false;

		protected void Awake ()
		{
				init ();
		}

		/// <summary>
		/// Init this instance, it's public so it can be called from a InspectorScript
		/// </summary>
		public void init ()
		{
				if (id == -1) {
						id = JSONPersistor.GetLocalIdentfier (this);
				}

				fileName = getFileName ();
		
				if (loadOnAwake
		    //&& FileExists ()
		    ) {
						//Debug.Log ("file exists: " + fileName);
						load ();
				}

				isInit = true;
		}

		private void OnDestroy ()
		{
				if (saveOnDestroy) {
						save ();
				}
				//JSONPersistor.Instance.killInstanceID (this.id);
		}

		public int getPersistentID ()
		{
				return this.id;
		}

		public bool FileExists ()
		{
				return JSONPersistor.Instance.fileExists (fileName);
		}

		private string getFileName ()
		{
				return this.gameObject.name + "_" + this.id;
		}
	
		public abstract JSONClass getDataClass ();

/* example:
 * protected JSONClass getDataClass ()
	{
		JSONClass jClass = new JSONClass ();

		jClass ["startRange"].AsFloat = myData.startRange;
		jClass ["endRange"].AsFloat = myData.endRange;
		jClass ["minimumValue"].AsFloat = myData.minimumValue;

		return jClass;
	}
*/

		public abstract void setClassData (JSONClass jClass);

/* example:
 * 
 * protected void setClassData (JSONClass jClass)
	{
		myData.startRange = jClass ["startRange"].AsFloat;
		myData.endRange = jClass ["endRange"].AsFloat;
		myData.minimumValue = jClass ["minimumValue"].AsFloat;
	}
*/


/*	protected JSONClass Deserialize (string json)
	{
		JSONNode node = JSONNode.LoadFromBase64 (json);
		return node.AsObject;
	}*/

		public virtual void save ()
		{
				JSONClass jClass = getDataClass ();
				jClass ["id"].AsInt = this.id;
				//JSONPersistor.Instance.saveToFile (fileName, jClass);
				JSONPersistor.Instance.savePersitencies (fileName, jClass);

				//Debug.Log ("saved " + fileName);
		}

		public virtual void load ()
		{
				JSONClass jClass = JSONPersistor.Instance.loadPersistencies (fileName);
				setClassData (jClass);


/*				if (JSONPersistor.Instance.fileExists (fileName)) {
						JSONClass jClass = JSONPersistor.Instance.loadJSONClassFromFile (fileName);

						if (!string.IsNullOrEmpty (jClass ["id"].Value)) {
								this.id = jClass ["id"].AsInt;
						}

						setClassData (jClass);
				} else {
						//Debug.LogError ("File with fileName '" + fileName + "' does not exists!");
				}
*/
		}

		static string[] OnWillSaveAssets (string[] paths)
		{
				Debug.Log ("OnWillSaveAssets");
				foreach (string path in paths)
						Debug.Log (path);
				return paths;
		}

	#region instance_handling

		private static List<WeakReference> instances = new List<WeakReference> ();
	
		public JSONPersistent ()
		{
				instances.Add (new WeakReference (this));
		}

		public static IList<JSONPersistent> GetInstances ()
		{
				List<JSONPersistent> realInstances = new List<JSONPersistent> ();
				List<WeakReference> toDelete = new List<WeakReference> ();
		
				foreach (WeakReference reference in instances) {
						if (reference.IsAlive) {
								realInstances.Add ((JSONPersistent)reference.Target);
						} else {
								toDelete.Add (reference);
						}
				}
		
				foreach (WeakReference reference in toDelete)
						instances.Remove (reference);
		
				return realInstances;
		}

	#endregion

}