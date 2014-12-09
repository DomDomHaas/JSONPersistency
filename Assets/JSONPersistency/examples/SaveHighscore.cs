using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using JSONPersistency;

public class SaveHighscore: JSONPersistent
{
		public List<string> names;
		public List<int> scores;
	

		public override JSONClass getDataClass ()
		{
				JSONClass jClass = new JSONClass ();

				for (int i = 0; i < names.Count; i++) {
						string name = names [i];
						jClass ["names"] [i] = name;
				}

				for (int i = 0; i < scores.Count; i++) {
						int score = scores [i];
						jClass ["scores"] [i].AsInt = score;
				}
		
				return jClass;
		}

		public override void setClassData (JSONClass jClass)
		{
				names = new List<string> ();
				scores = new List<int> ();

				for (int i = 0; i < jClass["names"].Count; i++) {
						//Debug.Log ("add " + jClass ["names"] [i] + " to names");
						names.Add (jClass ["names"] [i]);
				}

				for (int i = 0; i < jClass["scores"].Count; i++) {
						//Debug.Log ("add " + jClass ["scores"] [i] + " to scores");
						scores.Add (jClass ["scores"] [i].AsInt);
				}


		}


}
