using System;

namespace UnityVMFLoader
{
	[AttributeUsage(AttributeTargets.Class)]
	public class DependsOnTaskAttribute : Attribute
	{
		public Type RequiredTaskType;

		public DependsOnTaskAttribute(Type taskType)
		{
			RequiredTaskType = taskType;
		}
	}
}