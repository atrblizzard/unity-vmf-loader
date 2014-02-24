using System.Linq;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ImportBrushesTask))]
	public class ImportWorldBrushesTask : ParserTask
	{
		public override void Run()
		{
			var root = Importer.GetTask<ParseNodesTask>().Root;

			var solids = root.Children.OfType<World>().First().Children.OfType<Solid>();

			Importer.GetTask<ImportBrushesTask>().Solids = solids;

			base.Run();
		}
	}
}
