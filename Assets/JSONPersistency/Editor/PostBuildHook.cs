using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;

namespace JSONPersistency
{

		public static class PostBuildHook
		{

				private static int filecount;
				private static int dircount;

				#region static_build_methods

				[PostProcessBuild]
				// add the JSONPersistor.filePath to the build!
				public static void OnPostProcessBuild (BuildTarget target, string path)
				{
						Debug.Log ("OnPostProcessBuild target " + target + " path " + path);

						// Get Required Paths
						//DirectoryInfo projectParent = Directory.GetParent (Application.dataPath);
						DirectoryInfo projectParent = new DirectoryInfo (Application.dataPath);
						string buildname = Path.GetFileNameWithoutExtension (path);
						DirectoryInfo targetdir = Directory.GetParent (path);
						char divider = Path.DirectorySeparatorChar;

						string dataMarker = "";
						#if UNITY_STANDALONE_WIN
						dataMarker = "_Data";
						#endif

						string buildDataDir = targetdir.FullName + divider + buildname + dataMarker + JSONPersistor.FilePathInUnity;
						string unitySourceFolder = projectParent.ToString () + JSONPersistor.FilePathInUnity;

						Debug.Log ("copy all from " + unitySourceFolder + " to " + buildDataDir.ToString ());

						filecount = 0;
						dircount = 0;

						CopyAll (new DirectoryInfo (unitySourceFolder), new DirectoryInfo (buildDataDir));

						Debug.Log ("Copied: " + filecount + " file" + ((filecount != 1) ? "s" : "") + ", " + dircount + " folder" + ((dircount != 1) ? "s" : ""));
				}


				//Your options: say you want to add another method that runs before your previous one then you can add a priority to the attribute like this:
				[PostProcessBuild(0)]
				// <- this is where the magic happens
				public static void OnPostProcessBuildFirst (BuildTarget target, string path)
				{
						//Debug.Log ("I get Executed First");
				}

				/// <summary>
				/// Recursive Copy Directory Method
				/// </summary>
				public static void CopyAll (DirectoryInfo source, DirectoryInfo target)
				{
						// Check if the target directory exists, if not, create it.
						if (Directory.Exists (target.FullName) == false) {
								dircount++;
								Directory.CreateDirectory (target.FullName);
						}
						// Copy each file into it’s new directory.
						foreach (FileInfo fi in source.GetFiles()) {
								filecount++;
								fi.CopyTo (Path.Combine (target.ToString (), fi.Name), true);
						}
						// Copy each subdirectory using recursion.
						foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
								dircount++;
								DirectoryInfo nextTargetSubDir = target.CreateSubdirectory (diSourceSubDir.Name);
								CopyAll (diSourceSubDir, nextTargetSubDir);
						}
				}
				#endregion


		}
}