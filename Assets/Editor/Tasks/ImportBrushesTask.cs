using System.Collections.Generic;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ParseNodesTask))]
	public class ImportBrushesTask : Task
	{
		public IEnumerable<Solid> Solids;

		public override void Run()
		{
			Solids = null;

			base.Run();
		}
	}
}