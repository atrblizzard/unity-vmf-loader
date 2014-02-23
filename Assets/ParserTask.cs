using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityVMFLoader.Nodes;

namespace UnityVMFLoader
{
	public abstract class ParserTask
	{
		public abstract void Run();

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

			// Get all Types with ParserTask as the base type.

			var taskTypes = Assembly.GetCallingAssembly().GetTypes().Where(type => type.BaseType == typeof (ParserTask));

			foreach (var taskType in taskTypes)
			{
				// Find the first (and the only) DependsOnTaskAttribute of the type.

				var attributes = Attribute.GetCustomAttributes(taskType);

				var dependencyAttribute = attributes.Where(attribute => attribute.GetType() == typeof(DependsOnTaskAttribute)).Cast<DependsOnTaskAttribute>().FirstOrDefault();

				// If there are no dependencies, move over to the next ParserTask type.

				if(dependencyAttribute == null)
				{
					continue;
				}

				Dependencies[taskType] = dependencyAttribute.RequiredTasks.ToList();
			}
		}
	}
}