using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using UnityVMFLoader.Nodes;

using Debug = UnityEngine.Debug;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(CreateBrushObjectsTask))]
	public class ImportMaterialsTask : Task
	{
		private string SourcePath
		{
			get
			{
				return Path.Combine(Path.Combine("Assets", Settings.AssetPath), Settings.MaterialsFolder);
			}
		}

		private string DestinationPath
		{
			get
			{
				return Path.Combine(Path.Combine(Application.dataPath, Settings.DestinationAssetPath), Settings.DestinationMaterialsFolder);
			}
		}

		public override void Run()
		{
			var solids = Importer.GetTask<ImportBrushesTask>().Solids;

			// Get all unique materials used in the solids.

			var materials = solids.Select(solid => solid.Children.OfType<Side>().Select(side => side.Material)).SelectMany(x => x).Distinct().ToList();

			// Narrow it down to those that don't already exist in the assets.

			materials = materials.Where(material => AssetDatabase.LoadAssetAtPath(Path.Combine(DestinationPath, material + ".tga"), typeof(Texture)) == null).ToList();

			// Use vtf2tga to make them into assets.

			if (Settings.AssetPath == "")
			{
				Debug.LogWarning("No asset path specified in settings - skipping asset import.");

				base.Run();

				return;
			}

			foreach (var material in materials)
			{
				var materialFullPath = Path.Combine(SourcePath, material + ".vmt");

				if (!File.Exists(materialFullPath))
				{
					Debug.LogWarning(String.Format("Material \"{0}\" not found.", material));

					continue;
				}

				var materialFile = File.ReadAllText(Path.Combine(SourcePath, material + ".vmt"));

				var regex = new Regex("\"?\\$basetexture\"?\\s+\"?([^\"]*)\"?", RegexOptions.IgnoreCase);

				var match = regex.Match(materialFile);

				if (!match.Success)
				{
					Debug.LogWarning(String.Format("Can't find $basetexture in material \"{0}\".", material));

					continue;
				}

				var texture = match.Groups[1].Value;

				var textureFullPath = Path.Combine(SourcePath, texture + ".vtf");

				var destinationFullPath = Path.Combine(DestinationPath, texture + ".tga");

				var directory = Path.GetDirectoryName(destinationFullPath);

				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				var process = Process.Start
				(
					new ProcessStartInfo
					{
						CreateNoWindow = true,
						UseShellExecute = false,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						FileName = Path.Combine(Application.dataPath, "vtf2tga.exe"),
						WindowStyle = ProcessWindowStyle.Hidden,
						Arguments = String.Format("-i \"{0}\" -o \"{1}\"", textureFullPath, destinationFullPath)
					}
				);

				while (!process.StandardError.EndOfStream)
				{
					string line = process.StandardError.ReadLine();

					Debug.LogWarning(line);
				}
			}

			base.Run();
		}
	}
}
