using System.Collections.ObjectModel;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CloudDataProtection.App.Shared.ViewModels;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.AndroidX;

namespace CloudDataProtection.App.Android.Adapters
{
    public class NameAdapter : ReactiveRecyclerViewAdapter<NameViewModel>
    {
        public override int ItemCount => backingList.Count;

        private readonly ReadOnlyObservableCollection<NameViewModel> backingList;

        public NameAdapter(ReadOnlyObservableCollection<NameViewModel> backingList) : base(backingList.ToObservableChangeSet())
        {
            this.backingList = backingList;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.FromContext (parent.Context).Inflate (Resource.Layout.view_name, parent, false);
            
            return new NameViewHolder (view);
        }
    }

    public class NameViewHolder : ReactiveRecyclerViewViewHolder<NameViewModel>
    {
        private TextView NameTextView { get; set; }
        
        public NameViewHolder(View view) : base(view)
        {
            this.WireUpControls();

            this.OneWayBind(ViewModel, v => v.Name, vh => vh.NameTextView.Text);
        }
    }
}