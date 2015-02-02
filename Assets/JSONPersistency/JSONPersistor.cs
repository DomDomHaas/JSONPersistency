using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Text; 
using SimpleJSON;
using System.Reflection;


namespace JSONPersistency
{

		/// <summary>
		/// JSON persistor capsuls the connection the SimpleJSON.
		/// </summary>
		public class JSONPersistor
		{
				public static readonly string FilePathInUnity = "/JSONPersistency/SaveLoadObjects";
				public static readonly string PersistorFileName = "JSONSaves";
				private static readonly string persistentClasses = "persist";


				private static JSONPersistor instance = null;
				private Dictionary<string, JSONClass> persistencies = null;

/*		private int instanceCount = 0;

		private IDictionary<int, string> fileInstances = new Dictionary<int, string> ();
		private IDictionary<int, int> gameObjectInstances = new Dictionary<int, int> ();
*/
				private JSONPersistor ()
				{
						loadPersistenciesFromFile ();
				}
	
				public static JSONPersistor Instance {
						get {
								if (instance == null) {
										instance = new JSONPersistor ();
								}
								return instance; 
						}
				}

/*		private void loadUpExistingInstances (IList<JSONPersistent> persistents)
		{

				foreach (JSONPersistent persist in persistents) {
						registerNewInstance (persist);
				}
		}
*/
				public delegate void EventHandler (object listener,EventArgs e);

				public event EventHandler gameSaved;
				public event EventHandler gameLoaded;

				public bool fileExists (string fileName)
				{
						return File.Exists (getFullFilePath (fileName));
				}

				public bool directoryExists (string dirName)
				{
						return Directory.Exists (Application.dataPath + FilePathInUnity);
				}

				public string getFullFilePath (string fileName)
				{
						//Debug.Log ("getFullFilePath: " + Application.dataPath + FilePathInUnity + "/" + fileName + ".txt");
						return Application.dataPath + FilePathInUnity + "/" + fileName + ".txt";
				}


				public void savePersitencies (string fileName, JSONClass data)
				{
						if (persistencies.ContainsKey (fileName)) {
								persistencies.Remove (fileName);
						}
						persistencies.Add (fileName, data);

						JSONClass savePersistency = new JSONClass ();

						//Debug.Log ("save " + persistencies.Count);

						foreach (KeyValuePair<string, JSONClass> kvp in persistencies) {
								savePersistency [persistentClasses].Add (kvp.Key, kvp.Value);
						}

						//Debug.Log ("savePersitencies: should save " + savePersistency.AsObject.ToString ());

						savePersistency.SaveToFile (getFullFilePath (PersistorFileName));
				}

				public void saveToFile (string fileName, JSONClass data)
				{
						if (data.Count <= 0) {
								throw new UnityException ("saveToFile: JSONClass data is empty!");
						} else {

								//Debug.Log ("saveToFile: should save " + fileName + " data: " + data.ToString ());

								// SaveToFile already creates directories and the file!
								data.SaveToFile (getFullFilePath (fileName));
						}

				}

				public void saveToFile (string fileName, JSONArray jArray)
				{
						/*		if (!directoryExists (Application.dataPath + filePath)) {
			Directory.CreateDirectory (Application.dataPath + filePath);
		}
		
		if (!fileExists (fileName)) {
			File.CreateText (getFullFilePath (fileName));
		}*/

						// save to file already creates directories and the file!
						jArray.SaveToFile (getFullFilePath (fileName));
				}

				public JSONClass loadPersistencies (string objname)
				{
						//Debug.Log ("loadPersistencies " + objname + " from " + persistencies.Count);

						if (persistencies.Count > 0 && persistencies.ContainsKey (objname)) {
								return persistencies [objname];
						}

						loadPersistenciesFromFile ();

						if (!persistencies.ContainsKey (objname)) {
								throw new UnityException ("trying to load '" + objname + "' but it's not in the " + getFullFilePath (PersistorFileName) + " file!");
						}

						return persistencies [objname];
				}

				private void loadPersistenciesFromFile ()
				{
						persistencies = new Dictionary<string, JSONClass> ();

						//Debug.Log ("loading fileExists " + fileExists (PersistorFileName) + " " + getFullFilePath (PersistorFileName));

						if (fileExists (PersistorFileName)) {

								JSONNode node = JSONNode.LoadFromFile (getFullFilePath (PersistorFileName));
								JSONClass savePersistency = node.AsObject;

								//Debug.Log ("loading " + savePersistency.Count);

								foreach (string key in savePersistency [persistentClasses].Keys) {
										persistencies.Add (key, savePersistency [persistentClasses] [key].AsObject);
										//Debug.Log ("added " + key + " to persistencies");
								}
						}

				}

				public JSONClass loadJSONClassFromFile (string fileName)
				{
						JSONNode node = JSONNode.LoadFromFile (getFullFilePath (fileName));
						return node.AsObject;
				}

				public JSONArray loadJSONArrayFromFile (string fileName)
				{
						JSONNode node = JSONNode.LoadFromFile (getFullFilePath (fileName));
						return node.AsArray;
				}


	#region Events
	
				private void OnGameSaved (EventArgs e)
				{
						if (this.gameSaved == null) {
								//Debug.Log ("OnStart but no one to notify");
						} else {
								this.gameSaved (this, e);
						}
				}
	
				private void OnGameLoaded (EventArgs e)
				{
						if (this.gameLoaded == null) {
								//Debug.Log ("OnSuccess but no one to notify");
						} else {
								this.gameLoaded (this, e);
						}
				}

	#endregion

	#region conversion

				public static JSONBinaryTag GetJSONBinaryTag<T> (T t)
				{
						Type type = t.GetType ();
						//Debug.Log ("got type: " + type);

						if (type == typeof(Array))
								return JSONBinaryTag.Array;
						if (type == typeof(bool))
								return JSONBinaryTag.BoolValue;
						if (type == typeof(JSONClass))
								return JSONBinaryTag.Class;
						if (type == typeof(double))
								return JSONBinaryTag.DoubleValue;
						if (type == typeof(float))
								return JSONBinaryTag.FloatValue;
						if (type == typeof(int))
								return JSONBinaryTag.IntValue;

						/*
		if (type == typeof())
			return JSONBinaryTag.Value;
*/
						return JSONBinaryTag.Value;
				}
	#endregion

	#region static_color_methdos

				// Note that Color32 and Color implictly convert to each other.
				// You may pass a Color object to this method without first casting it.
				public static string ColorToHex (Color32 color)
				{
						string hex = color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
						return hex;
				}
	
				public static Color HexToColor (string hex)
				{
						if (hex.Length < 6) {
								throw new UnityException ("Hexadecimal Color Value is too short!");
						} else {
								byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
								byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
								byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
								return new Color32 (r, g, b, 255);
						}
				}

				public static string[] getHexArrayFromColors (Color[] colors)
				{
						string[] hexArray = new string[colors.Length];
						for (int i = 0; i < colors.Length; i++) {
								hexArray [i] = JSONPersistor.ColorToHex (colors [i]);
						}
						return hexArray;
				}
	
				public static Color[] getColorsArrayFromHex (string[] hexArray)
				{
						Color[] colors = new Color[hexArray.Length];
						for (int i = 0; i < hexArray.Length; i++) {
								colors [i] = JSONPersistor.HexToColor (hexArray [i]);
						}
						return colors;
				}

				public static Texture2D getTextureFromWWW (string fullPath)
				{
						fullPath = "file:///" + fullPath;
						WWW wwwLoader = new WWW (fullPath);
						//Debug.Log ("loading texture " + fullPath + " via www, loaded: " + www);
						return wwwLoader.texture;
				}

				public static Sprite getSpriteFromWWW (string fullPath, bool packTight = false)
				{
						Texture2D tex = getTextureFromWWW (fullPath);

						Rect rect = new Rect (0, 0, tex.width, tex.height);
						SpriteMeshType meshType = SpriteMeshType.FullRect;
						if (packTight) {
								meshType = SpriteMeshType.Tight;
						}
		
						// use 100f to scale down
						Sprite spr = Sprite.Create (tex, rect, new Vector2 (0.5f, 0.5f), 100f, 0, meshType);
						spr.name = fullPath.Substring (fullPath.LastIndexOf ("/") + 1);
						return spr;
				}
	
	#endregion

	#region instance_methods

				public static int GetLocalIdentfier (Component comp)
				{
#if UNITY_EDITOR
						PropertyInfo inspectorModeInfo = typeof(UnityEditor.SerializedObject).GetProperty ("inspectorMode", BindingFlags.NonPublic 
								| BindingFlags.Instance);
		
						UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject (comp);
						inspectorModeInfo.SetValue (serializedObject, UnityEditor.InspectorMode.Debug, null);
		
						UnityEditor.SerializedProperty localIdProp = serializedObject.FindProperty ("m_LocalIdentfierInFile");

						return localIdProp.intValue;
#else
				return -1;
#endif
				}

				public static int GetLocalIdentfier (GameObject go)
				{
#if UNITY_EDITOR
						PropertyInfo inspectorModeInfo = typeof(UnityEditor.SerializedObject).GetProperty ("inspectorMode", BindingFlags.NonPublic 
								| BindingFlags.Instance);
		
						UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject (go);
						inspectorModeInfo.SetValue (serializedObject, UnityEditor.InspectorMode.Debug, null);
		
						UnityEditor.SerializedProperty localIdProp = serializedObject.FindProperty ("m_LocalIdentfierInFile");

						//Debug.Log ("found property: " + localIdProp.intValue);

						return localIdProp.intValue;
#else
				return -1;
#endif
				}


	#endregion

		}

}