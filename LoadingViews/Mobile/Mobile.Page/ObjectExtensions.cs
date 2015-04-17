using System;
using System.Reflection;
using System.Linq;

namespace mobile.models
{
	public static class ObjectExtensions
	{
		public static void InvokeVoidMethod(this object obj, string methodName, object[] parameters)
		{
			if (obj != null && methodName != null) {
			
			var result = obj.GetType ().GetRuntimeMethods ()
				.Where (prop => prop.Name.ToLower () == methodName.ToLower ())
				.Select (v => v).FirstOrDefault ();

				if (result != null) {
					result.Invoke (obj, parameters);
				}
			}
		}
	}
}


