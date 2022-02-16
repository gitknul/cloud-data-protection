using System;
using Android.App;
using Android.OS;
using System.Net;
using System.Reactive.Linq;
using CloudDataProtection.App.Shared.Rest;
using ReactiveUI;
using CloudDataProtection.App.Shared.ViewModels;
using Xamarin.Essentials;

namespace CloudDataProtection.App.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, NoHistory = true)]
    public class MainActivity : ReactiveActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);

            ViewModel = new MainViewModel();

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(a => a.ViewModel.LoggedIn, a => a.ViewModel.User, (loggedIn, user) => (LoggedIn: loggedIn, User: user))
                    .Where(tuple => tuple.LoggedIn.HasValue)
                    // ReSharper disable once PossibleInvalidOperationException
                    // We already check with HasValue in this sequence
                    .Subscribe(OnLoggedIn);
            });
        }

        public void OnLoggedIn((bool? LoggedIn, LoginUserOutput User) tuple)
        {
            if (!tuple.LoggedIn.GetValueOrDefault(false) || tuple.User == null)
            {
                StartActivity(typeof(LoginActivity));
                return;
            }

            switch (tuple.User.Role)
            {
                case UserRole.Admin:
                    StartActivity(typeof(AdminDashboardActivity));
                    break;
                case UserRole.Client:
                    StartActivity(typeof(ClientDashboardActivity));
                    break;
                default:
                    throw new Exception("Unknown user role");
            }
        }
    }
}
