using System;
using System.Reactive.Disposables;
using Android.App;
using Android.OS;
using Android.Widget;
using CloudDataProtection.App.Shared.Rest;
using CloudDataProtection.App.Shared.ViewModels;
using ReactiveUI;

namespace CloudDataProtection.App.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class LoginActivity : ReactiveActivity<LoginViewModel>
    {
        EditText EmailEditText { get; set; }
        EditText PasswordEditText { get; set; }
        Button LoginButton { get; set; }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            ViewModel = new LoginViewModel();

            SetContentView(Resource.Layout.activity_login);

            this.WireUpControls();
            
            this.WhenActivated(d => {
                this.Bind(ViewModel, vm => vm.Email, view => view.EmailEditText.Text)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Password, view => view.PasswordEditText.Text)
                    .DisposeWith(d);

                this.BindCommand(ViewModel, vm => vm.Login, view => view.LoginButton)
                    .DisposeWith(d);

                ViewModel.Login.IsExecuting.Subscribe(OnLoginIsExecuting);
                ViewModel.Login.Subscribe(OnLogin);
            });
        }
        
        private void OnLogin(LoginUserOutput user)
        {
            switch (user.Role)
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

        private void OnLoginIsExecuting(bool obj)
        {
            LoginButton.Enabled = !obj;
        }
    }
}