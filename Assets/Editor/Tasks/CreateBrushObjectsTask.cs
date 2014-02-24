using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityVMFLoader.Tasks
{
	[DependsOnTask(typeof(ImportBrushesTask), typeof(GroupNodesTask))]
	public class CreateBrushObjectsTask : Task
	{
		public override void Run()
		{
			var solids = Importer.GetTask<ImportBrushesTask>().Solids;
			var groups = Importer.GetTask<GroupNodesTask>().Groups;

			foreach (var solid in solids)
			{
				var gameObject = new GameObject("Solid " + solid.Identifier);

				gameObject.AddComponent<UnityEngine.MeshRenderer>();
				gameObject.AddComponent<MeshFilter>();

				gameObject.GetComponent<MeshFilter>().sharedMesh = (Mesh) solid;

				// Assign the placeholder material.

				var material = (Material) AssetDatabase.LoadAssetAtPath("Assets/Materials/dev_measuregeneric01b.mat", typeof(Material));

				gameObject.GetComponent<UnityEngine.MeshRenderer>().material = material;

				// The vertices of the mesh are in world coordinates so we'll need to center them.

				var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;

				var center = mesh.vertices.Average();

				var vertices = mesh.vertices;

				for (var vertex = 0; vertex < vertices.Count(); vertex++)
				{
					vertices[vertex] -= center;
				}

				mesh.vertices = vertices;

				mesh.RecalculateBounds();

				// And move the object itself to those world coordinates.

				gameObject.transform.position = center;

				// In order to make lightmap baking work, make object static.

				gameObject.isStatic = true;

				// Add a MeshCollider.

				var collider = gameObject.AddComponent<MeshCollider>();

				collider.convex = true;

				// If the solid is in a group, move it there.

				var editor = solid.Parent.Children.OfType<Nodes.Editor>().FirstOrDefault();

				if (editor != null)
				{
					var pair = groups.FirstOrDefault(x => x.Key.Identifier == editor.GroupIdentifier);

					if (pair.Value != null)
					{
						gameObject.transform.parent = pair.Value.transform;
					}
				}
			}

			base.Run();
		}
	}
}
