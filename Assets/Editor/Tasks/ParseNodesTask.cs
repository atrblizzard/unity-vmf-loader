using System;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader.Tasks
{
	public class ParseNodesTask : Task
	{
		public Node Root;

		public override void Run()
		{
			Root = new Node();
			var active = Root;

			foreach (var line in Importer.VMFLines)
			{
				var firstCharacter = line[0];

				switch (firstCharacter)
				{
					case '{':

						break;

					case '}':

						// End of current node.

						active = active.Parent ?? Root;

						break;

					case '"':

						// A key-value.

						var keyEnd = line.IndexOf('"', 1);
						var key = line.Substring(1, keyEnd - 1);

						var valueStart = line.IndexOf('"', keyEnd + 1);
						var value = line.Substring(valueStart, line.Length - valueStart).Trim('"');

						active.Parse(key, value);

						break;

					default:

						// Start of a new node.

						var type = Type.GetType("UnityVMFLoader.Nodes." + char.ToUpper(line[0]) + line.Substring(1)) ?? typeof(Nodes.Node);

						var parent = active;

						active = (Nodes.Node) Activator.CreateInstance(type);

						active.Key = line;
						active.Parent = parent;

						break;
				}
			}

			base.Run();
		}
	}
}