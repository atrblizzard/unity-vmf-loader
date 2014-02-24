using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ImportPointEntitiesTask))]
	public class ImportLightsTask : ParserTask
	{
		public override void Run()
		{
			var entities = Importer.GetTask<ImportPointEntitiesTask>().Entities;

			var lights = entities.Where(entity => entity.ClassName.StartsWith("light"));

			foreach (var light in lights)
			{
				var lightProperties = Regex.Replace(light["_light"], @"\s+", " ").Split(' ');

				var color = lightProperties.Take(3).Select(v => float.Parse(v) / 255f).ToArray();
				var brightness = float.Parse(lightProperties[3]) * Settings.LightBrightnessScalar;

				var lightObject = new GameObject("Light " + light.Identifier);

				lightObject.transform.position = light.Origin;
				lightObject.transform.rotation = light.Angles;

				var lightComponent = lightObject.AddComponent<Light>();

				lightComponent.intensity = brightness;
				lightComponent.color = new Color(color[0], color[1], color[2]);
				lightComponent.range = 25.0f;

				switch (light.ClassName)
				{
					case "light":

						lightComponent.type = LightType.Point;

						break;

					case "light_spot":

						lightComponent.type = LightType.Spot;
						lightComponent.spotAngle = int.Parse(light["_cone"]);

						break;
				}
			}

			base.Run();
		}
	}
}
