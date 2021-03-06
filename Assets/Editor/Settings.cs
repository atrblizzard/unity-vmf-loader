using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace UnityVMFLoader
{
	class Settings : EditorWindow
	{
		public static bool ImportDisplacements = false;

		public static bool ImportBrushes = true;
		public static bool ImportWorldBrushes = true;
		public static bool ImportDetailBrushes = true;
		public static bool GenerateLightmapUVs = true;

		public static bool ImportPointEntities = true;
		public static bool ImportLights = true;
		public static float LightBrightnessScalar = 0.005f;

		public static bool ImportAssets = false;
		public static bool ImportMaterials = false;
		public static bool ImportModels = false;
		public static bool ImportSounds = false;

		public static string AssetPath;
		public static string AssetLibraryPath;

		public static string MaterialsFolder = "materials";
		public static string DestinationMaterialsFolder = "Materials";

		public static string ModelsFolder = "models";
		public static string DestinationModelsFolder = "Models";

		public static string SoundsFolder = "sound";
		public static string DestinationSoundsFolder = "Sounds";

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

			ImportPointEntities = EditorGUILayout.BeginToggleGroup("Import point entities", ImportPointEntities);

			ImportLights = EditorGUILayout.Toggle("Import lights", ImportLights);

			LightBrightnessScalar = EditorGUILayout.Slider("Light brightness scalar", LightBrightnessScalar, 0, 0.02f);

			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();

			// Assets.

			var old = GUI.enabled;
			GUI.enabled = false;

			ImportAssets = EditorGUILayout.BeginToggleGroup("Import assets", ImportAssets);

			GUILayout.Label("Importing assets will import any missing assets that the map uses.", EditorStyles.wordWrappedLabel);

			EditorGUILayout.Space();

			ImportMaterials = EditorGUILayout.Toggle("Import materials", ImportMaterials);
			ImportModels = EditorGUILayout.Toggle("Import models", ImportModels);
			ImportSounds = EditorGUILayout.Toggle("Import sounds", ImportSounds);

			EditorGUILayout.Space();

			GUILayout.Label("The asset path is the path to the root directory of the game. It will be used to look for the other paths.", EditorStyles.wordWrappedLabel);

			EditorGUILayout.Space();

			AssetPath = EditorGUILayout.TextField("Asset path", AssetPath);

			MaterialsFolder = EditorGUILayout.TextField("Materials folder", MaterialsFolder);
			ModelsFolder = EditorGUILayout.TextField("Models folder", ModelsFolder);
			SoundsFolder = EditorGUILayout.TextField("Sounds folder", SoundsFolder);

			EditorGUILayout.Space();

			GUILayout.Label("The destination asset path is relative to the Assets folder.", EditorStyles.wordWrappedLabel);

			EditorGUILayout.Space();

			AssetLibraryPath = EditorGUILayout.TextField("Destination asset path", AssetLibraryPath);

			DestinationMaterialsFolder = EditorGUILayout.TextField("Destination materials folder", DestinationMaterialsFolder);
			DestinationModelsFolder = EditorGUILayout.TextField("Destination models folder", DestinationModelsFolder);
			DestinationSoundsFolder = EditorGUILayout.TextField("Destination mounds folder", DestinationSoundsFolder);

			GUI.enabled = old;

			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.Space();
		}

		[MenuItem("Unity VMF Loader/Settings")]

		static public void ShowSettings()
		{
			EditorWindow.GetWindow<Settings>();
		}
	}
}
