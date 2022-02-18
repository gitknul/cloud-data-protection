using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using CloudDataProtection.App.Shared.Rest;
using CloudDataProtection.App.Shared.Storage;
using Freshheads.Storage;
using ReactiveUI;
using Xamarin.Essentials;

namespace CloudDataProtection.App.Shared.ViewModels
{
    public class MainViewModel : ReactiveObject, IActivatableViewModel
    {
        private LoginUserOutput _user;
        public LoginUserOutput User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }
        

        public MainViewModel()
        {
            this.WhenActivated(d =>
            {
                Freshheads.Storage.Storage.GetAsync<LoginUserOutput>(StorageKeys.User, null, StorageType.Preferences).ToObservable()
                    .Subscribe(HandleUser)
                    .DisposeWith(d);
            });
        }

        private void HandleUser(LoginUserOutput output)
        {
            if (output != null && output.Id != 0)
            {
                User = output;
            }
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();
    }
}