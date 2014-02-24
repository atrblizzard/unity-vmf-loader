using System.Linq;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ImportBrushesTask))]
	public class ImportDetailBrushesTask : ParserTask
	{
		public override void Run()
		{
			var root = Importer.GetTask<ParseNodesTask>().Root;
			var solids = Importer.GetTask<ImportBrushesTask>().Solids;

			foreach (var entity in root.Children.OfType<Entity>())
			{
				var entities = entity.Children.OfType<Solid>();

				solids = solids == null ? entities : solids.Concat(entities);
			}

			Importer.GetTask<ImportBrushesTask>().Solids = solids;

			base.Run();
		}
	}
}
