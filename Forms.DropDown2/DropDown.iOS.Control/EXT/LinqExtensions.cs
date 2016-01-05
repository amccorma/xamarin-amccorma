using System;
using System.Collections.Generic;
using System.Linq;

namespace DropDown.iOS.Control
{
	public static class LinqExtensions
	{
		public static T FirstOrDefaultFromMany<T>(
			this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector,
			Predicate<T> condition)
		{
			// return default if no items
			if(source == null || !source.Any()) return default(T);

			// return result if found and stop traversing hierarchy
			var attempt = source.FirstOrDefault(t => condition(t));
			if(!Equals(attempt,default(T))) return attempt;

			// recursively call this function on lower levels of the
			// hierarchy until a match is found or the hierarchy is exhausted
			return source.SelectMany(childrenSelector).FirstOrDefaultFromMany(childrenSelector, condition);
		}
	}
}

