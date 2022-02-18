// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CloudDataProtection.App.iOS
{
	[Register ("AdminDashboardViewController")]
	partial class AdminDashboardViewController
	{
		[Outlet]
		UIKit.UITableView NamesTableView { get; set; }

		[Outlet]
		UIKit.UITextField SearchTextField { get; set; }

		[Action ("UnwindToAdminDashboard:")]
		partial void UnwindToAdminDashboard (UIKit.UIStoryboardSegue unwindSegue);

		void ReleaseDesignerOutlets ()
		{
			if (NamesTableView != null) {
				NamesTableView.Dispose ();
				NamesTableView = null;
			}

			if (SearchTextField != null) {
				SearchTextField.Dispose ();
				SearchTextField = null;
			}

		}
	}
}
