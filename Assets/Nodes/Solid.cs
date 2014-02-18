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
	public class Solid : Node
	{
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
}
