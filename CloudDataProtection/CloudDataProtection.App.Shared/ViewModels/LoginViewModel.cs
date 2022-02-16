using System;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CloudDataProtection.App.Shared.Manager;
using CloudDataProtection.App.Shared.Rest;
using ReactiveUI;
using Refit;
using Xamarin.Essentials;

namespace CloudDataProtection.App.Shared.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        protected string _email;
        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        protected string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public ReactiveCommand<Unit, LoginUserOutput> Login { get; }

        public LoginViewModel()
        {
            var canPerformLogin = this.WhenAnyValue(vm => vm.Email, vm => vm.Password, CanPerformLogin);

            Login = ReactiveCommand.CreateFromObservable<Unit, LoginUserOutput>
                (_ => DoLogin(new AuthenticateInput(Email, Password)), canPerformLogin, RxApp.MainThreadScheduler);

            Login.ThrownExceptions.Subscribe(OnLoginException);
        }

        private bool CanPerformLogin(string email, string password)
        {
            return !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password);
        }

        private IObservable<LoginUserOutput> DoLogin(AuthenticateInput input)
        {
            return UserManager.Instance.Login(input);
        }

        private void OnLoginException(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}