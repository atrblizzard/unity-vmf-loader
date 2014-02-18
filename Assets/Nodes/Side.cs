using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityVMFLoader.Nodes
{
	public class Side : Node
	{
		public Vector3 PointA;
		public Vector3 PointB;
		public Vector3 PointC;

		private static readonly Regex planeRegex;

		static Side()
		{
			planeRegex = new Regex(@"(?:\((\-?\d+(?:.\S+)?) (\-?\d+(?:.\S+)?) (\-?\d+(?:.\S+)?)\) ?){3}", RegexOptions.Compiled);
		}

		public override void Parse(string key, string value)
		{
			base.Parse(key, value);

			switch (key)
			{
				case "plane":

					// (-98.0334 356.145 -1.90735e-006) (-98.0334 356.145 0.999998) (-122 334.941 0.999998)

					var match = planeRegex.Match(value);

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

					// Source uses Z for up, but Unity uses Y.

					var y = PointA.y;

					PointA.y = PointA.z;
					PointA.z = y;

					y = PointB.y;

					PointB.y = PointB.z;
					PointB.z = y;

					y = PointC.y;

					PointC.y = PointC.z;
					PointC.z = y;

					break;
			}
		}

		static public explicit operator Mesh(Side side)
		{
			var mesh = new Mesh();

			var vertices = new Vector3[4];

			var vertex = 0;

			const float inchesInMeters = 0.0254f;

			vertices[vertex++] = side.PointA * inchesInMeters;
			vertices[vertex++] = side.PointB * inchesInMeters;
			vertices[vertex++] = side.PointC * inchesInMeters;
			vertices[vertex++] = (side.PointC + (side.PointA - side.PointB)) * inchesInMeters;

			mesh.vertices = vertices;

			var textureCoordinates = new Vector2[vertices.Length];

			for (var i = 0; i < vertices.Length; i++)
			{
				textureCoordinates[i] = new Vector2(vertices[i].x, vertices[i].z);
			}

			mesh.uv = textureCoordinates;

			mesh.RecalculateNormals();

			mesh.triangles = new[]
			{
				0, 1, 2,
				2, 3, 0
			};

			return mesh;
		}
	}
}
