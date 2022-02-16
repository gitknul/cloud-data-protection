using ReactiveUI;

namespace CloudDataProtection.App.Shared.ViewModels
{
    public class NameViewModel : ReactiveObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public NameViewModel(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is NameViewModel other && Name == other.Name;
        }
    }
}