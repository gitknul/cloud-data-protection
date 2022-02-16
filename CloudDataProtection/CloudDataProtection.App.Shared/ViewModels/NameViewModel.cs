using System;
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

        public string Id { get; } = Guid.NewGuid().ToString();
        
        public NameViewModel(string name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj is NameViewModel other)
            {
                return Name == other.Name &&
                        Id == other.Id;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}