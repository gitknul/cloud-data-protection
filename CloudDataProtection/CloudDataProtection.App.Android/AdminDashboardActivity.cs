using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using CloudDataProtection.App.Android.Intents;
using CloudDataProtection.App.Android.Adapters;
using CloudDataProtection.App.Shared.ViewModels;
using Newtonsoft.Json;
using ReactiveUI;
using CloudDataProtection.App.Android.Intents.Extras;
using CloudDataProtection.App.Android.Extensions;

namespace CloudDataProtection.App.Android
{
    [Activity(Label = "Admin dashboard", Theme = "@style/AppTheme")]
    public class AdminDashboardActivity : ReactiveActivity<AdminDashboardViewModel>, SwipeRefreshLayout.IOnRefreshListener
    {
        RecyclerView NameRecyclerView { get; set; }
        EditText SearchEditText { get; set; }
        LinearLayoutManager LayoutManager { get; set; }
        SwipeRefreshLayout SwipeContainer { get; set; }

        NameAdapter Adapter { get; set; }

        Toast loadingToast;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_admin_dashboard);

            this.WireUpControls();

            ViewModel = new AdminDashboardViewModel();

            SwipeContainer.SetOnRefreshListener(this);

            loadingToast = Toast.MakeText(this, "Loading...", ToastLength.Short);

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel.Rows)
                    .Where(rows => rows != null)
                    .Select(rows => Adapter = new NameAdapter(rows, HandleNameClick))
                    .Subscribe(InitializeNameRecylerView)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.SearchQuery, a => a.SearchEditText.Text)
                    .DisposeWith(d);

                ViewModel.Search.IsExecuting.Subscribe(OnSearchIsExecuting);
                ViewModel.Search.ThrownExceptions.Subscribe(OnSearchThrownExceptions);
            });
        }

        private void HandleNameClick(NameViewModel viewModel)
        {
            NameExtra extra = new NameExtra(viewModel.Name);

            Intent intent = new Intent(this, typeof(NameUnavailableActivity));

            intent.PutExtra<NameExtra>(IntentExtras.NameExtra, extra);

            StartActivity(intent);
        }

        private void InitializeNameRecylerView(NameAdapter adapter)
        {
            LayoutManager = new LinearLayoutManager(Application.Context, LinearLayoutManager.Vertical, false);

            // This prevents a IndexOutOfBoundsException with message "invalid view holder adapter position"
            NameRecyclerView.SetItemAnimator(null);

            NameRecyclerView.SetLayoutManager(LayoutManager);

            NameRecyclerView.SetAdapter(adapter);
        }

        private void OnSearchIsExecuting(bool isExecuting)
        {
            if (isExecuting)
            {
                loadingToast.Show();
            }
            else
            {
                loadingToast.Cancel();
            }
        }

        private void OnSearchThrownExceptions(Exception exception)
        {
            SwipeContainer.Refreshing = false;
        }

        public void OnRefresh()
        {
            SwipeContainer.Refreshing = false;

            Observable.Return(Unit.Default).InvokeCommand(ViewModel.Search);
        }
    }
}