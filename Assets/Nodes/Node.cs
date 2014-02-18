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

		private readonly List<Node> children = new List<Node>();

		public Node Parent
		{
			get
			{
				return parent;
			}

			set
			{
				if (parent != null)
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

		public uint Identifier;

		public virtual void Parse(string key, string value)
		{
			switch (key)
			{
				case "id":

					Identifier = Convert.ToUInt32(value);

					break;
			}
		}
	}
}
