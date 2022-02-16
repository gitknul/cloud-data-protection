using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.OS;
using Android.Widget;
using CloudDataProtection.App.Android.Extensions;
using CloudDataProtection.App.Android.Intents;
using CloudDataProtection.App.Android.Intents.Extras;
using CloudDataProtection.App.Shared.ViewModels;
using Google.Android.Material.Button;
using ReactiveUI;
using ReactiveUI.AndroidX;

namespace CloudDataProtection.App.Android
{
    [Activity(Label = "Job unavailable", Theme = "@style/AppTheme")]
    public class NameUnavailableActivity : ReactiveActivity<NameUnavailableViewModel>
    {
        TextView ErrorDescriptionTextView { get; set; }
        MaterialButton CloseButton { get; set; }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_name_unavailable);

            this.WireUpControls();

            var extra = Intent.GetExtra<NameExtra>(IntentExtras.NameExtra);

            ViewModel = new NameUnavailableViewModel(extra.Name);

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.ErrorDescription, v => v.ErrorDescriptionTextView.Text)
                    .DisposeWith(d);

                Observable.FromEventPattern(h => CloseButton.Click += h, h => CloseButton.Click -= h)
                    .Subscribe(_ => Finish());
            });
        }
    }
}
