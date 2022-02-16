using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Xamarin.Essentials;

namespace CloudDataProtection.App.Shared.Manager
{
    public class AuthenticationManager : ReactiveObject
    {
        private const string TokenKey = "jwt";
        
        private static AuthenticationManager _instance;
        public static AuthenticationManager Instance => _instance ??= new AuthenticationManager();

        private string _token;
        public string Token
        {
            get => _token;
            private set => this.RaiseAndSetIfChanged(ref _token, value);
        }

        private ObservableAsPropertyHelper<bool> _isLoggedIn;
        public bool IsLoggedIn => _isLoggedIn.Value;

        
        private AuthenticationManager()
        {
            this.WhenAnyValue(m => m.Token)
                .Select(token => !string.IsNullOrEmpty(token))
                .ToProperty(this, m => m.IsLoggedIn, out _isLoggedIn, initialValue: false);

            this.WhenAnyValue(m => m.Token)
                .BindTo(this, x => x.Token);
        }
        
        public async Task SaveToken(string token)
        {
            await SecureStorage.SetAsync(TokenKey, token);
        }
    }
}