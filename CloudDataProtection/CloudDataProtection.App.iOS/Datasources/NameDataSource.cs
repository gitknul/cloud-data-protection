using System;
using System.Collections.ObjectModel;
using System.Linq;
using CloudDataProtection.App.Shared.ViewModels;
using Foundation;
using ReactiveUI;
using UIKit;

namespace CloudDataProtection.App.iOS.Datasources
{
    public class NameDataSource : ReactiveTableViewSource<NameViewModel>
    {
        private readonly Action<NameViewModel> _itemSelected;

        private ReadOnlyObservableCollection<NameViewModel> Collection =>
            Data[0].Collection as ReadOnlyObservableCollection<NameViewModel>;

        public const string CellIdentifier = "NameTableViewCell";

        public NameDataSource(UITableView tableView, ReadOnlyObservableCollection<NameViewModel> rows, Action<NameViewModel> itemSelected) : base(tableView)
        {
            _itemSelected = itemSelected;
            
            Data = new[]
                { new TableSectionInformation<NameViewModel, NameTableViewCell>(rows, _ => NSString.Empty, (float)UITableView.AutomaticDimension) };
        }

        public override nint RowsInSection(UITableView tableview, nint section) => Collection.Count;

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            base.RowSelected(tableView, indexPath);

            _itemSelected(Collection.ElementAt(indexPath.Row));
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CellIdentifier) as NameTableViewCell;

            cell.ViewModel = Collection.ElementAt(indexPath.Row);

            return cell;
        }
    }
}