using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Runtime.Serialization;


namespace JSONPersistency
{

		[Serializable]
		public abstract class JSONPersistent : MonoBehaviour
		{

				/// <summary>
				/// This is a copy of the "m_localIndentiferInFile"
				/// </summary>
				[SerializeField]	
				private int
						persistentID = -1;

				/// <summary>
				/// Is required to save the json
				/// </summary>
				protected string fileName;
	
				public bool loadOnAwake = true;
				public bool saveOnDestroy = false;
				private bool isInit = false;
				public bool IsInit {
						get{ return isInit;}
						private set{ isInit = value;}
				}

				protected void Awake ()
				{
						init ();
				}

		#region workaround
		
		
				public void loadPersistentID ()
				{
#if UNITY_EDITOR
						// only do during the Editor because it uses UnityEditor which isn't in a build
						// when a object isn't saved yet saved in a scene, the Id is == 0
						//if (!persistentIDisSet ()) {
						persistentID = JSONPersistor.GetLocalIdentfier (this);
						//Debug.LogWarning ("set id " + persistentID);
						//}
#endif
				}

		#endregion

				public bool persistentIDisSet ()
				{
						return (persistentID != -1 && persistentID != 0);
				}

				/// <summary>
				/// Init this instance, it's public so it can be called from a InspectorScript
				/// </summary>
				public void init ()
				{
						loadPersistentID ();

						fileName = getFileName ();

						//Debug.LogWarning ("file exists: " + fileName + " " + JSONPersistor.Instance.fileExists (fileName));

						if (loadOnAwake) {
								load ();
						}

						isInit = true;
				}

				private void OnDestroy ()
				{
						if (saveOnDestroy) {
								save ();
						}

				}

				public int getPersistentID ()
				{
						return this.persistentID;
				}

				private string getFileName ()
				{
						return this.gameObject.name + "_" + this.persistentID;
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
						//Debug.Log ("going to save " + jClass.ToString ());

						jClass ["persistentID"].AsInt = this.persistentID;
						JSONPersistor.Instance.saveToFile (fileName, jClass);
						//JSONPersistor.Instance.savePersitencies (fileName, jClass);

						//Debug.Log ("saved " + fileName);
				}

				public virtual void load ()
				{
/*				JSONClass jClass = JSONPersistor.Instance.loadPersistencies (fileName);
				setClassData (jClass);
*/

						if (JSONPersistor.Instance.fileExists (fileName)) {
								JSONClass jClass = JSONPersistor.Instance.loadJSONClassFromFile (fileName);

								if (!string.IsNullOrEmpty (jClass ["persistentID"].Value)) {
										this.persistentID = jClass ["persistentID"].AsInt;
								}

								setClassData (jClass);
						} else {
								//Debug.LogError ("File with fileName '" + fileName + "' does not exists!");
						}
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
}