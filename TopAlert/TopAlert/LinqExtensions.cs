using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

public static class LinqExtensions
{
	public static T FirstOrDefaultFromMany<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenSelector, Predicate<T> condition)
	{
		if (source == null || !source.Any ())
			return default(T);

		var attempt = source.FirstOrDefault (t => condition (t));
		if (!Equals (attempt, default(T)))
			return attempt;

		return source.SelectMany (childrenSelector).FirstOrDefaultFromMany (childrenSelector, condition);
	}
}