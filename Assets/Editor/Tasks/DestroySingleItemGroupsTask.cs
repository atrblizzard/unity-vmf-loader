using System.Linq;
using UnityEngine;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(GroupNodesTask), typeof(CreateBrushObjectsTask))]
	public class DestroySingleItemGroupsTask : ParserTask
	{
		public override void Run()
		{
			var groups = Importer.GetTask<GroupNodesTask>().Groups;

			// Destroy the GameObjects of groups with a single child or none.

			var groupsCopy = groups.ToDictionary(entry => entry.Key, entry => entry.Value);

			foreach (var pair in groupsCopy.Where(x => x.Value.GetComponentsInChildren<Transform>().Length < 3))
			{
				var child = pair.Key.Children.FirstOrDefault();

				if (child != null)
				{
					child.Parent = pair.Key.Parent;
				}

				Object.DestroyImmediate(pair.Value);

				groups.Remove(pair.Key);
			}

			base.Run();
		}
	}
}
