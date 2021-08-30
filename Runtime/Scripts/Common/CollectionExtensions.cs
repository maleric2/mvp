using System;
using System.Collections.Generic;
using System.Linq;

namespace maleric.MVP.Common
{
	public static class CollectionExtensions
	{
		public static T Random<T>(this IEnumerable<T> collection)
		{
			var enumerable = collection as T[] ?? collection.ToArray();
			if (enumerable.Length == 0)
			{
				throw new IndexOutOfRangeException(collection + ": Cannot get a random element from an empty collection!");
			}
			return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Length));
		}

		public static T[] Shuffle<T>(this IEnumerable<T> collection, int seed)
		{
			var random = new Random(seed);
			var shuffledArray = ShuffleCollection(collection, random);
			return shuffledArray;
		}

		public static T[] Shuffle<T>(this IEnumerable<T> collection)
		{
			var random = new Random();
			var shuffledArray = ShuffleCollection(collection, random);
			return shuffledArray;
		}

		private static T[] ShuffleCollection<T>(this IEnumerable<T> collection, Random random)
		{
			var newArray = collection.ToArray();

			for (var i = newArray.Length; i > 1; i--)
			{
				var j = random.Next(i);
				var tmp = newArray[j];
				newArray[j] = newArray[i - 1];
				newArray[i - 1] = tmp;
			}

			return newArray;
		}
	}
}
