using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace UnityVMFLoader
{
	public static class Importer
	{
		public static List<string> VMFLines;

		private static List<ParserTask> tasks;
		private static readonly List<ParserTask> doneTasks;

		static Importer()
		{
			tasks = new List<ParserTask>();
			doneTasks = new List<ParserTask>();
		}

		public static void AddTask<T>() where T : ParserTask
		{
			tasks.Add(Activator.CreateInstance<T>());
		}

		public static T GetTask<T>() where T : ParserTask
		{
			return (T) doneTasks.FirstOrDefault(task => task.GetType() == typeof(T));
		}

		public static bool TaskDone<T>() where T : ParserTask
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
