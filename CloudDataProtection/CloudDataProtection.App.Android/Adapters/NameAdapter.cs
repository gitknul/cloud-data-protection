using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

        readonly ReadOnlyObservableCollection<NameViewModel> backingList;

        readonly Action<NameViewModel> itemSelected;

        public NameAdapter(ReadOnlyObservableCollection<NameViewModel> backingList, Action<NameViewModel> itemSelected) : base(backingList.ToObservableChangeSet())
        {
            this.backingList = backingList;
            this.itemSelected = itemSelected;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.view_name, parent, false);

            var viewHolder = new NameViewHolder(view);

            viewHolder.Selected
                .Select(i => backingList.ElementAt(i))
                .Subscribe(itemSelected);

            return viewHolder;
        }
    }

    public class NameViewHolder : ReactiveRecyclerViewViewHolder<NameViewModel>
    {
        public TextView NameTextView { get; set; }


        public NameViewHolder(View view) : base(view)
        {
            this.WireUpControls();

            this.OneWayBind(ViewModel, v => v.Name, vh => vh.NameTextView.Text);
        }
    }
}