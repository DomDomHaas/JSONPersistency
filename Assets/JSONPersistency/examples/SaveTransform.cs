using UnityEngine;
using System.Collections;
using SimpleJSON;
using JSONPersistency;

public class SaveTransform : JSONPersistent
{
		public bool saveLocalPosition = false;

		public override JSONClass getDataClass ()
		{
				JSONClass jClass = new JSONClass ();
				jClass ["world_x"].AsFloat = transform.position.x;
				jClass ["world_y"].AsFloat = transform.position.y;
				jClass ["world_z"].AsFloat = transform.position.z;
				jClass ["local_x"].AsFloat = transform.localPosition.x;
				jClass ["local_y"].AsFloat = transform.localPosition.y;
				jClass ["local_z"].AsFloat = transform.localPosition.z;

				jClass ["quaternion_x"].AsFloat = transform.rotation.x;
				jClass ["quaternion_y"].AsFloat = transform.rotation.y;
				jClass ["quaternion_z"].AsFloat = transform.rotation.z;
				jClass ["quaternion_w"].AsFloat = transform.rotation.w;

				jClass ["scale_x"].AsFloat = transform.localScale.x;
				jClass ["scale_y"].AsFloat = transform.localScale.y;
				jClass ["scale_z"].AsFloat = transform.localScale.z;

		
				return jClass;
		}

		public override void setClassData (JSONClass jClass)
		{
				transform.position = new Vector3 (jClass ["world_x"].AsFloat,
		                                  jClass ["world_y"].AsFloat,
		                                  jClass ["world_z"].AsFloat);
				if (saveLocalPosition) {
						transform.localPosition = new Vector3 (jClass ["local_x"].AsFloat,
			                                       jClass ["local_y"].AsFloat,
			                                       jClass ["local_z"].AsFloat);
				}

				transform.rotation = new Quaternion (jClass ["quaternion_x"].AsFloat,
		                                     jClass ["quaternion_y"].AsFloat,
		                                     jClass ["quaternion_z"].AsFloat,
		                                     jClass ["quaternion_w"].AsFloat);

				transform.localScale = new Vector3 (jClass ["scale_x"].AsFloat,
		                                    jClass ["scale_y"].AsFloat,
		                                    jClass ["scale_z"].AsFloat);
		}

}
