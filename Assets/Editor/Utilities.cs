using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UnityVMFLoader
{
	public static class Utilities
	{
		private const float InchesInMeters = 0.0254f;

		public static Vector3 Average(this IEnumerable<Vector3> source)
		{
			var sum = source.Sum();
			var average = sum / source.Count();

			return average;
		}

		public static Vector3 Average<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector3> selector)
		{
			var sum = source.Sum(selector);
			var average = sum / source.Count();

			return average;
		}

		public static Vector3 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Vector3> selector)
		{
			var sum = Vector3.zero;

			foreach(var vector in source)
			{
				sum += selector(vector);
			}

			return sum;
		}

		public static Vector3 Sum(this IEnumerable<Vector3> source)
		{
			var sum = Vector3.zero;

			foreach(var vector in source)
			{
				sum += vector;
			}

			return sum;
		}

		public static Vector3 SourceToUnity(this Vector3 vec)
		{
			return new Vector3(vec.x * InchesInMeters, vec.z * InchesInMeters, vec.y * InchesInMeters);
		}
	}
}
