using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader
{
	public abstract class ParserTask
	{
		public abstract void Run(Node root);

		public bool CanRun
		{
			get
			{
				return Dependencies[GetType()].All(VMFParser.TaskDone);
			}
		}

		protected static readonly Dictionary<Type, List<Type>> Dependencies;

		static ParserTask()
		{
			Dependencies = new Dictionary<Type, List<Type>>();

			var taskTypes = Assembly.GetCallingAssembly().GetTypes().Where(type => type.BaseType == typeof (ParserTask));

			foreach (var taskType in taskTypes)
			{
				var attributes = Attribute.GetCustomAttributes(taskType).Where(attribute => attribute.GetType() == typeof(DependsOnTaskAttribute)).Cast<DependsOnTaskAttribute>();
				Dependencies[taskType] = new List<Type>();

				foreach (var dependency in attributes)
				{
					Dependencies[taskType].Add(dependency.RequiredTaskType);
				}
			}
		}
	}
}