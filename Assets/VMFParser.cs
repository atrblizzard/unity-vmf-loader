using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityVMFLoader
{
	public static class VMFParser
	{
		public static Node Parse(string path)
		{
			var lines = File.ReadAllLines(path).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

			var root = new Node();
			var active = root;
			
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

						active.Parse(key, value);
						
						break;

					default:

						// Start of a new node.

						var parent = active;

						switch (line)
						{
							case "world":

								active = new World();

								break;

							case "solid":

								active = new Solid();

								break;

							case "side":

								active = new Side();

								break;

							default:

								active = new Node();
								
								break;
						}

						active.Key = line;
						active.Parent = parent;

						break;
				}
			}

			Debug.Log(String.Format("Parsed {0} solids.", root.Children.OfType<World>().First().Children.OfType<Solid>().Count()));
			
			return root;
		}
	}

	public class Node
	{
		public string Key;

		public ReadOnlyCollection<Node> Children
		{
			get
			{
				return children.AsReadOnly();
			}
		}

		public List<Node> children = new List<Node>();

		public Node Parent
		{
			get
			{
				return parent;
			}

			set
			{
				if (parent != null && value == null)
				{
					parent.children.Remove(this);
				}

				parent = value;

				if (parent != null && value != null)
				{
					parent.children.Add(this);
				}
			}
		}

		private Node parent;

		public virtual void Parse(string key, string value)
		{

		}
	}

	public class World : Node
	{

	}

	public class Solid : Node
	{
		public uint id;

		public override void Parse(string key, string value)
		{
			switch (key)
			{
				case "id":

					id = Convert.ToUInt32(value);

					break;
			}
		}
	}

	public class Side : Node
	{
		public uint id;
		public Plane plane;

		public override void Parse(string key, string value)
		{
			switch (key)
			{
				case "id":

					id = Convert.ToUInt32(value);

					break;

				case "plane":

					// (-98.0334 356.145 -1.90735e-006) (-98.0334 356.145 0.999998) (-122 334.941 0.999998)

					var match = new Regex(@"(?:\((\-?\d+(?:.\S+)?) (\-?\d+(?:.\S+)?) (\-?\d+(?:.\S+)?)\) ?){3}").Match(value);

					if (!match.Success)
					{
						throw new Exception("Failed to match a plane on side " + id + ".");
					}

					plane.Set3Points
					(
						new Vector3
						(
							float.Parse(match.Groups[1].Captures[0].Value),
							float.Parse(match.Groups[2].Captures[0].Value),
							float.Parse(match.Groups[3].Captures[0].Value)
						),

						new Vector3
						(
							float.Parse(match.Groups[1].Captures[1].Value),
							float.Parse(match.Groups[2].Captures[1].Value),
							float.Parse(match.Groups[3].Captures[1].Value)
						),

						new Vector3
						(
							float.Parse(match.Groups[1].Captures[2].Value),
							float.Parse(match.Groups[2].Captures[2].Value),
							float.Parse(match.Groups[3].Captures[2].Value)
						)
					);

					break;
			}
		}
	}
}