using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace UnityVMFLoader
{
	public static class VMFParser
	{
		public static Node Parse(string path)
		{
			var lines = File.ReadAllLines(path).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

			var root = new Node();
			var active = root;
			
			var keywriter = new StreamWriter(Path.Combine(Application.dataPath, "tree.txt"));
			
			foreach (var line in lines)
			{
				var firstCharacter = line[0];
				
				switch (firstCharacter)
				{
					case '{':
						
						break;

					case '}':

						// End of current node.

						active = active.Parent ?? root;

						break;

					case '"':

						// A key-value.

						var keyEnd = line.IndexOf('"', 1);
						var key = line.Substring(1, keyEnd - 1);

						var valueStart = line.IndexOf('"', keyEnd + 1);
						var value = line.Substring(valueStart, line.Length - valueStart).Trim('"');
						
						keywriter.WriteLine("\"{0}\": \"{1}\"", key, value);

						active.Children.Add
						(
							new Node
							{
								Key = key,
								Value = value,
								Parent = active
							}
						);
						
						break;

					default:

						// Start of a new node.

						keywriter.WriteLine("\n{0}:\n", line);

						active = new Node
						{
							Key = line,
							Parent = active
						};

						break;
				}
			}
			
			keywriter.Close();

			Debug.Log("Parsed VMF file " + path);
			
			return root;
		}
	}

	public class Node
	{
		public string Key;
		public string Value;

		public Node Parent;
		public List<Node> Children = new List<Node>();
	}
}