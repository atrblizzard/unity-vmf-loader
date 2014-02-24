using System;

namespace UnityVMFLoader
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DependsOnTaskAttribute : Attribute
	{
		public Type[] RequiredTasks;

		public DependsOnTaskAttribute(params Type[] tasks)
		{
			RequiredTasks = tasks;
		}
	}
}