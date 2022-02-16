using System;
using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using ReactiveUI;
using ReactiveUI.AndroidX;

namespace CloudDataProtection.App.Android.Listeners
{
	public class RecyclerViewOnScrollListener : RecyclerView.OnScrollListener
	{
		readonly ReactiveActivity activity;
		readonly bool hideKeyboardOnScroll;
		bool isKeyboardDismissedByScroll;

		// Changed ReactiveAppCompatActivity to ReactiveActivity
		public RecyclerViewOnScrollListener(ReactiveActivity activity, bool hideKeyboardOnScroll)
		{
			this.activity = activity;
			this.hideKeyboardOnScroll = hideKeyboardOnScroll;
		}

		public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
		{
			switch (newState)
			{
				case (int)ScrollState.TouchScroll:
					if (!isKeyboardDismissedByScroll && hideKeyboardOnScroll)
					{
						HideKeyboard();
						isKeyboardDismissedByScroll = !isKeyboardDismissedByScroll;
					}
					break;

				case (int)ScrollState.Idle:
					isKeyboardDismissedByScroll = false;
					break;
			}
			base.OnScrollStateChanged(recyclerView, newState);
		}

		void HideKeyboard()
		{
			var inputManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
			inputManager.HideSoftInputFromWindow(activity.CurrentFocus?.WindowToken, HideSoftInputFlags.None);
			activity.CurrentFocus?.ClearFocus();
		}
	}
}
