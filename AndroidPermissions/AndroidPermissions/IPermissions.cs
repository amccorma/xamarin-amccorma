using System;
using System.Threading.Tasks;

namespace AndroidPermissions
{
	public interface IPermissions
	{
		/// <summary>
		/// Checks Permissions and set PermissionObject.Result to true/false (task) await
		/// </summary>
		/// <returns><c>true</c> if this instance has permissions the specified obj; otherwise, <c>false</c>.</returns>
		/// <param name="obj">PermissionObject.</param>
		void HasPermissions (PermissionObject obj);

		/// <summary>
		/// Checks Permissions and returns true/false. no task
		/// </summary>
		/// <returns><c>true</c> if this instance has obj; otherwise, <c>false</c>.</returns>
		/// <param name="obj">PermissionObject.</param>
		bool Has (PermissionObject obj);

		/// <summary>
		/// Requests the permissions and set PermissionObject.Result to true/false (task) await.
		/// </summary>
		/// <param name="obj">PermissionObject.</param>
		void RequestPermissions (PermissionObject obj);

		/// <summary>
		/// Requests the permissions reason and set PermissionObject.Result to true/false (task) await.
		/// </summary>
		/// <param name="obj">PermissionObject.</param>
		void RequestPermissionsReason (PermissionObject obj);
	}
}

