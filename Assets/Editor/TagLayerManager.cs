using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class TagLayerManager {

	[MenuItem("Asset Store Tools/Export package with tags and physics layers")]
	public static void ExportPackage()
	{
		string[] projectContent = new string[]{"Assets/Tycoon Terrain","ProjectSettings/TagManager.asset"};
		AssetDatabase.ExportPackage (projectContent,"UltimateTemplate_BeatEmUp.unitypackage",ExportPackageOptions.Interactive | ExportPackageOptions.Recurse |ExportPackageOptions.IncludeDependencies);
		Debug.Log ("Project Exported");
	}
}
