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
	public class Editor : Node
	{
		public uint GroupIdentifier;

		public override void Parse(string key, string value)
		{
			base.Parse(key, value);

			switch (key)
			{
				case "groupid":

					GroupIdentifier = Convert.ToUInt32(value);

					break;
			}
		}
	}
}
