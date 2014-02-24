using System.Linq;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ImportBrushesTask), typeof(ImportBrushEntitiesTask))]
	public class ImportDetailBrushesTask : Task
	{
		public override void Run()
		{
			var solids = Importer.GetTask<ImportBrushesTask>().Solids;

			foreach (var entity in Importer.GetTask<ImportBrushEntitiesTask>().Entities)
			{
				var entitySolids = entity.Children.OfType<Solid>();

				solids = solids == null ? entitySolids : solids.Concat(entitySolids);
			}

			Importer.GetTask<ImportBrushesTask>().Solids = solids;

			base.Run();
		}
	}
}
