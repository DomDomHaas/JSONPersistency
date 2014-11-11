using UnityEngine;
using System.Collections;
using SimpleJSON;
using JSONPersistency;

public class SavingGameObject : JSONPersistent
{

/*		public class SaveTransform :JSONPersistentData
		{
				public float world_x;
				public float world_y;
				public float world_z;
		}

		public SaveTransform mySaveData;*/

		// Use this for initialization
		new void Awake ()
		{
				//mySaveData = new SaveTransform ();
				init ();
		}

		new public void init ()
		{
				base.init ();
		}

		void Start ()
		{
				//save ();
		}
		// Update is called once per frame
		void Update ()
		{
	
		}

		public override JSONClass getDataClass ()
		{
				JSONClass jClass = new JSONClass ();
				jClass ["world_x"].AsFloat = transform.position.x;
				jClass ["world_y"].AsFloat = transform.position.y;
				jClass ["world_z"].AsFloat = transform.position.z;
				return jClass;
		}

		public override void setClassData (JSONClass jClass)
		{
				transform.position = new Vector3 (jClass ["world_x"].AsFloat, jClass ["world_y"].AsFloat, jClass ["world_z"].AsFloat);
		}

		public override void save ()
		{
				base.save ();
				//Debug.Log ("name " + name + " persistentID " + this.getPersistentID () + " saved");
		}

		public override void load ()
		{
				base.load ();
				//Debug.Log ("name " + name + " persistentID " + this.getPersistentID () + " loaded");
		}

}
