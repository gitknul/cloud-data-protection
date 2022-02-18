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
	[Register ("NameDetailViewController")]
	partial class NameDetailViewController
	{
		[Outlet]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		UIKit.UILabel ErrorDescriptionLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}

			if (ErrorDescriptionLabel != null) {
				ErrorDescriptionLabel.Dispose ();
				ErrorDescriptionLabel = null;
			}

		}
	}
}
