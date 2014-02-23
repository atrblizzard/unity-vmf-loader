using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityVMFLoader.Tasks;

namespace UnityVMFLoader
{
	public class VMFPostprocessor : AssetPostprocessor
	{
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
		{
			importedAssets = importedAssets.Where(x => Path.GetExtension(x) == ".vmf").ToArray();
			deletedAssets = deletedAssets.Where(x => Path.GetExtension(x) == ".vmf").ToArray();
			movedAssets = movedAssets.Where(x => Path.GetExtension(x) == ".vmf").ToArray();
			movedFromPath = movedFromPath.Where(x => Path.GetExtension(x) == ".vmf").ToArray();

			foreach (var asset in importedAssets)
			{
				try
				{
					Importer.AddTask<ParseNodesTask>();
					Importer.AddTask<GroupNodesTask>();

					if(Settings.ImportBrushes)
					{
						Importer.AddTask<ImportBrushesTask>();
					}

					if(Settings.ImportWorldBrushes)
					{
						Importer.AddTask<ImportWorldBrushesTask>();
					}

					if(Settings.ImportDetailBrushes)
					{
						Importer.AddTask<ImportDetailBrushesTask>();
					}

					if(Settings.ImportWorldBrushes || Settings.ImportDetailBrushes)
					{
						Importer.AddTask<CreateBrushObjectsTask>();
						Importer.AddTask<DestorySingleItemGroupsTask>();
					}

					if (Settings.ImportLights)
					{
						Importer.AddTask<ImportLightsTask>();
					}

					Importer.Import(Path.Combine(Directory.GetParent(Application.dataPath).FullName, asset));
				}
				finally
				{
					// We can get rid of the asset once we have imported its contents.

					AssetDatabase.DeleteAsset(asset);
				}
			}
		}
	}
}
