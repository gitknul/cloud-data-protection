using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CloudDataProtection.App.Shared.Rest;
using DynamicData;
using ReactiveUI;
using Refit;

namespace CloudDataProtection.App.Shared.ViewModels
{
    public class AdminDashboardViewModel : ReactiveObject, IActivatableViewModel
    {
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
        }
        
        readonly SourceList<NameViewModel> _rowsSource;

        ReadOnlyObservableCollection<NameViewModel> _rows;
        public ReadOnlyObservableCollection<NameViewModel> Rows {
            get => _rows;
            set => this.RaiseAndSetIfChanged (ref _rows, value);
        }

        private readonly ICloudDataProtectionApi _api;
        
        public ReactiveCommand<Unit, SearchOutput> Search { get; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();


        public AdminDashboardViewModel()
        {
            _api = RestService.For<ICloudDataProtectionApi>(UnsafeHttpClientProvider.Instance.Client);

            _rowsSource = new SourceList<NameViewModel>();

            _rowsSource.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _rows)
                .Subscribe();
            
            Search = ReactiveCommand.CreateFromObservable<Unit, SearchOutput>(_ => _api.Search(SearchQuery));

            Search.ThrownExceptions.Subscribe(OnSearchException);
            Search.Subscribe(HandleSearch);

            Observable.Return(Unit.Default).InvokeCommand(Search);

            this.WhenAnyValue(vm => vm.SearchQuery)
                .Where(s => string.IsNullOrEmpty(s) || s.Length >= 2)
                .Skip(1)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(query => query?.Trim())
                .DistinctUntilChanged()
                .Select(_ => Unit.Default)
                .InvokeCommand(Search);
        }

        private void OnSearchException(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        public void HandleSearch(SearchOutput output)
        {
            var cells = output.Names
                .Select(n => new NameViewModel(n))
                .ToList();

            if (!_rowsSource.Items.SequenceEqual (cells)) {
                _rowsSource.Edit (list => {
                    list.Clear ();
                    list.AddRange (cells);
                });
            }
        }
    }
}