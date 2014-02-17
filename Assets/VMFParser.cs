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

			foreach (var solid in root.Children.OfType<World>().First().Children.OfType<Solid>())
			{
				GameObject gameObject = new GameObject("Solid " + solid.Identifier);

				gameObject.AddComponent<MeshFilter>().mesh = (Mesh) solid;
			}

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
		public uint Identifier;

		public override void Parse(string key, string value)
		{
			switch (key)
			{
				case "id":

					Identifier = Convert.ToUInt32(value);

					break;
			}
		}

		static public explicit operator Mesh(Solid solid)
		{
			var mesh = new Mesh();

			var combines = new CombineInstance[solid.Children.OfType<Side>().Count()];

			var i = 0;

			foreach (var side in solid.Children.OfType<Side>())
			{
				combines[i++].mesh = (Mesh) side;
			}

			mesh.CombineMeshes(combines, true, false);
			mesh.Optimize();

			return mesh;
		}
	}

	public class Side : Node
	{
		public uint Identifier;

		public Vector3 PointA;
		public Vector3 PointB;
		public Vector3 PointC;

		public override void Parse(string key, string value)
		{
			switch (key)
			{
				case "id":

					Identifier = Convert.ToUInt32(value);

					break;

				case "plane":

					// (-98.0334 356.145 -1.90735e-006) (-98.0334 356.145 0.999998) (-122 334.941 0.999998)

					var match = new Regex(@"(?:\((\-?\d+(?:.\S+)?) (\-?\d+(?:.\S+)?) (\-?\d+(?:.\S+)?)\) ?){3}").Match(value);

					if (!match.Success)
					{
						throw new Exception("Failed to match a plane on side " + Identifier + ".");
					}

					PointA = new Vector3
					(
						float.Parse(match.Groups[1].Captures[0].Value),
						float.Parse(match.Groups[2].Captures[0].Value),
						float.Parse(match.Groups[3].Captures[0].Value)
					);

					PointB = new Vector3
					(
						float.Parse(match.Groups[1].Captures[1].Value),
						float.Parse(match.Groups[2].Captures[1].Value),
						float.Parse(match.Groups[3].Captures[1].Value)
					);

					PointC = new Vector3
					(
						float.Parse(match.Groups[1].Captures[2].Value),
						float.Parse(match.Groups[2].Captures[2].Value),
						float.Parse(match.Groups[3].Captures[2].Value)
					);

					break;
			}
		}

		static public explicit operator Mesh(Side side)
		{
			Mesh mesh = new Mesh();

			var vertices = new Vector3[4];

			var vertex = 0;

			vertices[vertex++] = side.PointA;
			vertices[vertex++] = side.PointB;
			vertices[vertex++] = side.PointC;
			vertices[vertex++] = side.PointC + (side.PointA - side.PointB);

			mesh.vertices = vertices;

			mesh.RecalculateNormals();

			mesh.triangles = new int[]
			{
				0, 1, 2,
				2, 3, 0
			};

			return mesh;
		}
	}
}
