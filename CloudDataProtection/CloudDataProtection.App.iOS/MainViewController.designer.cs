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
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UILabel LoadingLabel { get; set; }
		
		[Outlet] 
		UIKit.UILabel TestLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (LoadingLabel != null) {
				LoadingLabel.Dispose ();
				LoadingLabel = null;
			}

			if (TestLabel != null)
			{
				TestLabel.Dispose();
				TestLabel = null;
			}
		}

		[Action ("UnwindToMain:")]
		partial void UnwindToMain (UIKit.UIStoryboardSegue unwindSegue);
	}
}
