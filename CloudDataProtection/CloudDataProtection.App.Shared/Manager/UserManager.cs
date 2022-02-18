using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CloudDataProtection.App.Shared.Rest;
using CloudDataProtection.App.Shared.Storage;
using Freshheads.Storage;
using Newtonsoft.Json;
using Refit;
using ReactiveUI;
using Xamarin.Essentials;

namespace CloudDataProtection.App.Shared.Manager
{
    public class UserManager : ReactiveObject
    {
        private const string UserKey = "user";
        
        private readonly ICloudDataProtectionApi _api;

        private static UserManager _instance;
        public static UserManager Instance => _instance ??= new UserManager();

        long _id;
        public long Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        string _email;
        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        UserRole _role;
        public UserRole Role
        {
            get => _role;
            set => this.RaiseAndSetIfChanged(ref _role, value);
        }


        private UserManager()
        {
            _api = RestService.For<ICloudDataProtectionApi>(UnsafeHttpClientProvider.Instance.Client);
        }

        public IObservable<LoginUserOutput> Login(AuthenticateInput input)
        {
            return _api.Authenticate(input)
                .Do(output => SaveUserDetails(output.User))
                .Do(SaveToken)
                .Select(o => o.User);
        }

        private async void SaveUserDetails(LoginUserOutput user)
        {
            await Freshheads.Storage.Storage.SetAsync(StorageKeys.User, user, StorageType.Preferences);

            Id = user.Id;
            Email = user.Email;
            Role = user.Role;
        }

        private async void SaveToken(AuthenticateOutput output)
        {
            await AuthenticationManager.Instance.SaveToken(output.Token);
        }
    }
}
