using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ParseNodesTask))]
	public class GroupNodesTask : Task
	{
		public Dictionary<Group, GameObject> Groups;

		public override void Run()
		{
			var root = Importer.GetTask<ParseNodesTask>().Root;

			// Create groups from the parsed tree.

			Groups = new Dictionary<Group, GameObject>();

			foreach (var group in root.Children.OfType<World>().First().Children.OfType<Group>())
			{
				Groups[group] = new GameObject("Group " + group.Identifier);
			}

			base.Run();
		}
	}
}