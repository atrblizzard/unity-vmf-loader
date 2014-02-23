using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityVMFLoader
{
	public abstract class ParserTask
	{
		public virtual void Run()
		{
			Done = true;
		}

		public bool CanRun
		{
			get
			{
				if(!Dependencies.ContainsKey(GetType()))
				{
					return true;
				}

				return Dependencies[GetType()].All(Importer.TaskDone);
			}
		}

		public bool Done;

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

				var dependencyAttribute = attributes.OfType<DependsOnTaskAttribute>().FirstOrDefault();

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