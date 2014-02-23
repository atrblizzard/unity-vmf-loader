using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace UnityVMFLoader
{
	class Settings : EditorWindow
	{
		public static bool ImportDisplacements = false;

		public static bool ImportEntities = false;
		public static bool ImportLights = false;

		public static bool ImportBrushes = true;
		public static bool ImportWorldBrushes = true;
		public static bool ImportDetailBrushes = true;
		public static bool GenerateLightmapUVs = true;

		public static bool ImportAssets = false;
		public static bool ImportMaterials = false;
		public static bool ImportModels = false;
		public static bool ImportSounds = false;

		public static string AssetPath;
		public static string AssetLibraryPath;

		public static string MaterialsPath = "materials";
		public static string MaterialsLibraryPath = "materials";

		public static string ModelsPath = "models";
		public static string ModelsLibraryPath = "models";

		public static string SoundsPath = "sound";
		public static string SoundsLibraryPath = "sound";

		public void OnGUI()
		{
			title = "Unity VMF Loader";

			// General.

			GUILayout.Label("General", EditorStyles.boldLabel);

			GUI.enabled = false;

			ImportDisplacements = EditorGUILayout.Toggle("Import displacements", ImportDisplacements);

			GUI.enabled = true;

			EditorGUILayout.Space();

			// Brushes.

			ImportBrushes = EditorGUILayout.BeginToggleGroup("Import brushes", ImportBrushes);

			ImportWorldBrushes = EditorGUILayout.Toggle("Import world brushes", ImportWorldBrushes);
			ImportDetailBrushes = EditorGUILayout.Toggle("Import detail brushes", ImportDetailBrushes);
			GenerateLightmapUVs = EditorGUILayout.Toggle("Generate lightmap UVs", GenerateLightmapUVs);

			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();

			// Entities.

			GUI.enabled = false;

			ImportEntities = EditorGUILayout.BeginToggleGroup("Import entities", ImportEntities);

			ImportLights = EditorGUILayout.Toggle("Import lights", ImportLights);

			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();

			// Assets.

			ImportAssets = EditorGUILayout.BeginToggleGroup("Import assets", ImportAssets);

			GUILayout.Label("Importing assets will import any missing assets that the map uses.");

			EditorGUILayout.Space();

			ImportMaterials = EditorGUILayout.Toggle("Import materials", ImportMaterials);
			ImportModels = EditorGUILayout.Toggle("Import models", ImportModels);
			ImportSounds = EditorGUILayout.Toggle("Import sounds", ImportSounds);

			EditorGUILayout.Space();

			GUILayout.Label("The asset path is the path to the root directory of the game. It will be used to look for the other paths.");

			EditorGUILayout.Space();

			AssetPath = EditorGUILayout.TextField("Asset path", AssetPath);
			MaterialsPath = EditorGUILayout.TextField("Materials path", MaterialsPath);
			ModelsPath = EditorGUILayout.TextField("Models path", ModelsPath);
			SoundsPath = EditorGUILayout.TextField("Sounds path", SoundsPath);

			EditorGUILayout.Space();

			GUILayout.Label("The destination paths in the asset library. These will also be used to look for the existing assets before deciding if they need to be imported.");

			EditorGUILayout.Space();

			AssetLibraryPath = EditorGUILayout.TextField("Asset path", AssetLibraryPath);
			MaterialsLibraryPath = EditorGUILayout.TextField("Materials path", MaterialsLibraryPath);
			ModelsLibraryPath = EditorGUILayout.TextField("Models path", ModelsLibraryPath);
			SoundsLibraryPath = EditorGUILayout.TextField("Sounds path", SoundsLibraryPath);

			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();

			GUI.enabled = true;
		}

		[MenuItem("Unity VMF Loader/Settings")]

		static public void ShowSettings()
		{
			EditorWindow.GetWindow<Settings>();
		}
	}
}
