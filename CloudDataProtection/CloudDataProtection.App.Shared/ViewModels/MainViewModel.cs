using System;
using System.Reactive.Threading.Tasks;
using CloudDataProtection.App.Shared.Rest;
using Newtonsoft.Json;
using ReactiveUI;
using Xamarin.Essentials;

namespace CloudDataProtection.App.Shared.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private LoginUserOutput _user;
        public LoginUserOutput User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }
        
        private bool? _loggedIn;
        public bool? LoggedIn
        {
            get => _loggedIn;
            set => this.RaiseAndSetIfChanged(ref _loggedIn, value);
        }


        public MainViewModel()
        {
            SecureStorage.GetAsync("user").ToObservable()
                .Subscribe(HandleUser);
        }

        private void HandleUser(string json)
        {
            if (json != null)
            {
                User = JsonConvert.DeserializeObject<LoginUserOutput>(json);
                LoggedIn = true;
            }
            else
            {
                LoggedIn = false;
            }
        }
    }
}