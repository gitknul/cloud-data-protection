using System;
using ReactiveUI;

namespace CloudDataProtection.App.Shared.ViewModels
{
    public class NameUnavailableViewModel : ReactiveObject
    {
        private string _errorDescription;
        public string ErrorDescription
        {
            get => _errorDescription;
            set => this.RaiseAndSetIfChanged(ref _errorDescription, value);
        }

        public NameUnavailableViewModel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ErrorDescription = "Deze naam is helaas niet meer beschikbaar.";
            }
            else
            {
                ErrorDescription = $"De naam {name} is helaas niet meer beschikbaar.";
            }
        }
    }
}
