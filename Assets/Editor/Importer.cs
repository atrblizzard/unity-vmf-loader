using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace UnityVMFLoader
{
	public static class Importer
	{
		public static List<string> VMFLines;

		private static List<Task> tasks;
		private static readonly List<Task> doneTasks;

		static Importer()
		{
			tasks = new List<Task>();
			doneTasks = new List<Task>();
		}

		public static void AddTask<T>() where T : Task
		{
			tasks.Add(Activator.CreateInstance<T>());
		}

		public static T GetTask<T>() where T : Task
		{
			return (T) doneTasks.FirstOrDefault(task => task.GetType() == typeof(T));
		}

		public static bool TaskDone<T>() where T : Task
		{
			return TaskDone(typeof(T));
		}

		public static bool TaskDone(Type type)
		{
			return doneTasks.Any(task => task.GetType() == type);
		}

		public static void Import(string path)
		{
			VMFLines = File.ReadAllLines(path).Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

			while (tasks.Any(task => task.CanRun))
			{
				foreach (var task in tasks)
				{
					if (!task.Done && task.CanRun)
					{
						task.Run();
					}

					if (task.Done)
					{
						doneTasks.Add(task);
					}
				}

				tasks = tasks.Where(task => !task.Done).ToList();
			}

			tasks.Clear();
			doneTasks.Clear();
		}
	}
}
