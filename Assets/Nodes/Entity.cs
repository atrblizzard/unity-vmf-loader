using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityVMFLoader.Nodes
{
	public class Entity : Node
	{
		public string ClassName;

		public Quaternion Angles;
		public Vector3 Origin;

		private readonly Dictionary<string, string> properties;  

		public Entity()
		{
			properties = new Dictionary<string, string>();
		}

		public override void Parse(string key, string value)
		{
			base.Parse(key, value);

			switch(key)
			{
				case "classname":

					ClassName = value;

					break;

				case "angles":

					var axis = value.Split(' ').Select(v => float.Parse(v)).ToArray();

					Angles = Quaternion.Euler(axis[0], axis[2], axis[1]);

					break;

				case "origin":

					var origin = value.Split(' ').Select(v => float.Parse(v)).ToArray();

					Origin = new Vector3(origin[0], origin[2], origin[1]);

					break;
				
				default:

					properties[key] = value;

					break;
			}
		}

		public string this[string key]
		{
			get
			{
				return properties[key];
			}

			set
			{
				properties[key] = value;
			}
		}
	}
}
