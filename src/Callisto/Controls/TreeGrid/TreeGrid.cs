using Callisto.Controls.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Collections.Specialized;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;


// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Callisto.Controls
{
    public sealed class TreeGrid : Control, IUpdateVisualState
    {
        
        private ScrollViewer _root;
        private Grid _rootGrid = new Grid();
        private ObservableCollection<TreeGridColumn> _columnDefinitions = new ObservableCollection<TreeGridColumn>();
        private ObservableCollection<TreeGridItem> _items = new ObservableCollection<TreeGridItem>();
        private Dictionary<RowDefinition, ItemInfo> _rowItemMap = new Dictionary<RowDefinition, ItemInfo>(); 
        private GridLength _rowHeight;
        private double _rowSplitterHeight = 1;
        private Brush _rowSplitterBrush = new SolidColorBrush(Colors.Blue);
        private double _rowSplitterOpacity = 1;
        private bool _headerRowAdded = false;
        
        public TreeGrid()
        {
            this.DefaultStyleKey = typeof(TreeGrid);
            _columnDefinitions.CollectionChanged += _columnDefinitions_CollectionChanged;
            _items.CollectionChanged += _items_CollectionChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _root = GetTemplateChild("RootScrollViewer") as ScrollViewer;
            _root.Content = _rootGrid;
        }

        #region CollectionChanged event handlers

        private void _columnDefinitions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddColumns(e.NewItems.Cast<TreeGridColumn>());
            }
        }

        private void AddColumns(IEnumerable<TreeGridColumn> newColumns)
        {
            foreach (var column in newColumns)
            {
                int idx = _columnDefinitions.IndexOf(column);
                bool first = idx == 0 ? true : false;
                if (!first)
                {
                    _rootGrid.ColumnDefinitions.Add(column.SplitterColumn);
                }
                _rootGrid.ColumnDefinitions.Add(column.GridColumn);


                //move columns if the new column is not added at the end
                if (idx != _columnDefinitions.Count - 1)
                {
                    //Swap columns
                    //rootGrid.ColumnDefinitions.Insert(idx * 2, column.GridColumn);
                    //rootGrid.ColumnDefinitions.Insert(idx + 1, column.SplitterColumn);
                }

                if (column.Content != null)
                {
                    AddHeader(column, idx * 2, !first);
                }
            }
            foreach (var child in _rootGrid.Children)
            {
                var rect = child as Rectangle;
                if (rect != null)
                {
                    if (Grid.GetRow(rect) > 0)
                    {
                        Grid.SetColumnSpan(rect, _rootGrid.ColumnDefinitions.Count);
                    }
                }
            }
        }

        void _items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                AddItems(e.NewItems.Cast<TreeGridItem>());
            }
        }

        private void AddItems(IEnumerable<TreeGridItem> newItems)
        {
            foreach (TreeGridItem item in newItems)
            {
                //Check that the number of columns matches the item fields
#if DEBUG
                if (item.Fields.Count() > _columnDefinitions.Count)
                {
                    throw new ArgumentException("Number of fields is different than numbers of the grid columns");
                }
#endif
                //Create new row at the end
                var itemInfo = new ItemInfo() { IndentLevel = 0 };
                InsertNewRows(_rootGrid.RowDefinitions.Count, 1);
                UpdateRowForItem(item, _rootGrid.RowDefinitions.Count - 2, itemInfo);

            }
        }

        private void UpdateRowForItem(TreeGridItem item, int index, ItemInfo info)
        {
            //Populate columns
            var fields = item.Fields.ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                if(i == 0)
                {
                    //first column is treated special because of the image
                    //BUGBUG add RTL support
                    double indentwidth = info.IndentLevel * IndentSize;
                    var sp = new StackPanel() { Orientation = Orientation.Horizontal, Tag = item };
                    info.Root = sp;
                    if (item.HasChildren)
                    {
                        TreeGridIcon collapsedIcon = new TreeGridIcon() { Content = CollapsedIcon, Tag=info, Collapsed = true }; 
                        collapsedIcon.Tapped += row_Tapped;
                        sp.Children.Add(collapsedIcon);
                    }
                    else
                    {
                        //TODO Replace this with indentation based on the real image. This requires changing the control to something that has width.
                        indentwidth += 12;
                    }
                    sp.Margin = new Thickness(indentwidth, 0, 0, 0);
                    fields[i].Margin = new Thickness(5,0,0,0);
                    sp.Children.Add(fields[i]);
                    AddToGrid(index, i, sp);
                    _rowItemMap.Add(_rootGrid.RowDefinitions[index], info);
                }
                else
                {
                    AddToGrid(index, i, fields[i]);
                }
            }
            AddSplitterForRow(index + 1);
        }

        private void AddToGrid(int index, int i, FrameworkElement item)
        {
            _rootGrid.Children.Add(item);
            Grid.SetColumn(item, i * 2);
            Grid.SetRow(item, index);
        }

        void row_Tapped(object sender, TappedRoutedEventArgs e)
        {

            TreeGridIcon icon = (TreeGridIcon)sender;
            var tag = (ItemInfo)icon.Tag;
            var sp = (StackPanel)tag.Root;
            var item = (TreeGridItem) sp.Tag;
            //Cache children

            var currentIndex = Grid.GetRow(sp);
            var parentInfo = _rowItemMap[_rootGrid.RowDefinitions[currentIndex]];
            if(icon.Collapsed)
            {
                icon.Collapsed = false;
                //TODO - add animation if it takes too long to get the children
                icon.Content = ExpandedIcon;
                //insert new rows
                var count = item.GetChildren().Count();
                InsertNewRows(currentIndex + 2, count);
                UpdateExistingItems(currentIndex, count);
                foreach (var child in item.GetChildren())
                {
                    currentIndex += 2;
                    var childInfo = new ItemInfo(){IndentLevel = parentInfo.IndentLevel + 1};
                    UpdateRowForItem(child, currentIndex, childInfo);
                }
            }
            else
            {
                icon.Collapsed = true;
                icon.Content = CollapsedIcon;
                var totalRows = _rootGrid.RowDefinitions.Count -1;
                var childrenCount = GetChildrenCount(parentInfo.IndentLevel, currentIndex);
                var lastRemainingRow = _rootGrid.RowDefinitions.Count - childrenCount - 1;
                RemoveExistingElements(currentIndex, childrenCount) ;
                for (int i = totalRows; i > lastRemainingRow; i--)
                {
                    var row = _rootGrid.RowDefinitions[i];
                    _rowItemMap.Remove(row);
                    _rootGrid.RowDefinitions.Remove(row);
                }
            }
            
        }

        private int GetChildrenCount(int parentIndentLevel, int currentIndex)
        {
            for (int i = currentIndex + 2; i < _rootGrid.RowDefinitions.Count; i = i + 2)
            {
                var itemInfo = _rowItemMap[_rootGrid.RowDefinitions[i]];
                if (itemInfo.IndentLevel <= parentIndentLevel)
                {
                    //We found the next item that has the same indent or lower in the grid.
                    return i - currentIndex - 2;
                }
            }
            //We reached the last row in the grid
            return _rootGrid.RowDefinitions.Count - currentIndex - 2;
        }

        private void RemoveExistingElements(int currentIndex, int countOfDeletedRows)
        {
            var l = new List<FrameworkElement>();
            var lastDeletedRow = currentIndex + countOfDeletedRows;
            foreach (FrameworkElement fe in _rootGrid.Children)
            {
                var row = Grid.GetRow(fe);
                if (row > currentIndex + 1 && row < lastDeletedRow + 1)
                {
                    l.Add(fe);
                }
                else if (row > lastDeletedRow + 1)
                {
                    var newRowIndex = row - countOfDeletedRows;
                    Grid.SetRow(fe, newRowIndex);
                }
            }

            foreach (FrameworkElement fe in l)
            {
                _rootGrid.Children.Remove(fe);
            }

            //Update the ItemInfo
            for (int i = lastDeletedRow + 2; i < _rootGrid.RowDefinitions.Count; i = i + 2)
            {
                var oldRow = _rootGrid.RowDefinitions[i];
                var itemInfo = _rowItemMap[oldRow];
                _rowItemMap[_rootGrid.RowDefinitions[i- countOfDeletedRows]] = itemInfo;
            }

        }
        private void UpdateExistingItems(int currentIndex, int count)
        {
            foreach (FrameworkElement fe in _rootGrid.Children)
            {
                var row = Grid.GetRow(fe);
                if (row > currentIndex + 1)
                {
                    int newRowIndex = row + count * 2;
                    //move the ui to the new row
                    Grid.SetRow(fe, newRowIndex);
                }
            }       
        }

        #endregion
        private void AddHeader(TreeGridColumn column, int index, bool addSplitter)
        {
            //Add header
            if (!_headerRowAdded)
            {
                InsertNewRows(0,1); //Add Header row
                AddSplitterForRow(1);
                _headerRowAdded = true;
            }
            _rootGrid.Children.Add(column);
            Grid.SetColumn(column, index);
            Grid.SetRow(column, 0);
            //Add columns splitter
            if(addSplitter)
            {
                _rootGrid.Children.Add(column.Splitter);
                Grid.SetColumn(column.Splitter, index - 1);
                Grid.SetRow(column.Splitter, 0);
                Grid.SetRowSpan(column.Splitter, int.MaxValue);
            }
        }
        private void InsertNewRows(int currentIndex, int count)
        {
            var index = currentIndex;
            for (int i = 0; i < count; i++)
			{
                //item row
                var r = new RowDefinition() {Height = _rowHeight};
                _rootGrid.RowDefinitions.Insert(index,r);
                index++;
                //splitter row
                r= new RowDefinition(){Height = new GridLength(_rowSplitterHeight)};
                _rootGrid.RowDefinitions.Insert(index,r);
                index++;

			}
        }
        private void AddSplitterForRow(int index)
        {
            var rect = new Rectangle()
            {
                Height = _rowSplitterHeight,
                Fill = _rowSplitterBrush,
                Opacity = _rowSplitterOpacity
            };
            _rootGrid.Children.Add(rect);
            Grid.SetRow(rect, index);
            Grid.SetColumn(rect, 0);
            Grid.SetColumnSpan(rect, int.MaxValue);
        }

        #region Row and Column collections

        public ObservableCollection<TreeGridColumn> ColumnDefinitions
        {
            get
            {
                return _columnDefinitions;
            }
        }
        public ObservableCollection<TreeGridItem> Items
        {
            get 
            {
                return _items;
            }
        }

        #endregion

        #region Grid properties

        //public new Brush Background
        //{
        //    get
        //    {
        //        return _rootGrid.Background;
        //    }
        //    set
        //    {
        //        _rootGrid.Background = value;
        //    }
        //}

#endregion

        #region Row settings
        public GridLength RowHeight{
            get{
                return _rowHeight;
            }
            set{
                _rowHeight = value;
            }
        }

        public double RowSplitterHeight
        {
            get
            {
                return _rowSplitterHeight;
            }
            set
            {
                _rowSplitterHeight = value;
            }
        }

        public Brush RowSplitterBrush
        {
            get
            {
                return _rowSplitterBrush;
            }
            set 
            {
                _rowSplitterBrush = value;
            }
        }

        public double RowSplitterOpacity
        {
            get
            {
                return _rowSplitterOpacity;
            }
            set
            {
                _rowSplitterOpacity = value;
            }
        }

        #endregion

        #region Scroll bar visiblity
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get
            {
                return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
            }
            set
            {
                SetValue(VerticalScrollBarVisibilityProperty, value);
            }
        }
        public ScrollBarVisibility HorizontalScrollBarVisibility {
            get 
            {
                return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
            }
            set
            {
                SetValue(HorizontalScrollBarVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register(
            "VerticalScrollBarVisibility",
            typeof(ScrollBarVisibility),
            typeof(TreeGrid), 
            null);

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register(
            "HorizontalScrollBarVisibility",
            typeof(ScrollBarVisibility),
            typeof(TreeGrid),
            null);

#endregion

        #region Collapsed and Expanded images

        public string CollapsedIcon
        {
            get 
            {
                return (string)GetValue(CollapsedIconProperty);
            }
            set
            {
                SetValue(CollapsedIconProperty, value);
            }
        }

        public static readonly DependencyProperty CollapsedIconProperty = DependencyProperty.Register(
            "CollapsedIcon",
            typeof(string),
            typeof(TreeGrid),
            null);

        public string ExpandedIcon
        {
            get
            {
                return (string)GetValue(ExpandedIconProperty);
            }
            set
            {
                SetValue(ExpandedIconProperty, value);
            }
        }

        public static readonly DependencyProperty ExpandedIconProperty = DependencyProperty.Register(
            "ExpandedIcon",
            typeof(string),
            typeof(TreeGrid),
            null);

        #endregion

        #region Indent Size
        public int IndentSize
        {
            get
            {
                return (int)GetValue(IndentSizeProperty);
            }
            set
            {
                SetValue(IndentSizeProperty, value);
            }
        }

        public static readonly DependencyProperty IndentSizeProperty = DependencyProperty.Register(
            "IndentSize",
            typeof(int),
            typeof(TreeGrid),
            null); 

        #endregion
        
        internal InteractionHelper Interaction { get; private set; }
        public void UpdateVisualState(bool useTransitions)
        {
            Interaction.UpdateVisualStateBase(useTransitions);
        }

        #region helper classes

        class ItemInfo
        {
            public object Root { get; set; }
            public int IndentLevel { get; set; }
        }

        #endregion
    }
}
