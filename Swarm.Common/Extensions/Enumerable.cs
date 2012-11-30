using System;
using System.Collections.Generic;
using System.Linq;

namespace Swarm.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> selector)
        {
            IList<T> list = enumerable.ToList();
            if (!list.Any())
            {
                return default(TResult);
            }
            else
            {
                TResult max = list.Max(selector);
                return max;
            }
        }

		public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
		{
			IDictionary<TKey, TValue> dictionary = source.ToDictionary(
				pair => pair.Key,
				pair => pair.Value
			);
			return dictionary;
		}
    }
}
