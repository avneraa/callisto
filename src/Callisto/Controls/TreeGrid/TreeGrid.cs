﻿//
// Copyright (c) 2012 Tim Heuer
//
// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Callisto.Controls.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using System.Collections.Specialized;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Markup;


// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Callisto.Controls
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(TreeGridRow))]
    public sealed class TreeGrid : Control //, IUpdateVisualStatecolumn.splitter
    {
        
        private ScrollViewer _root;
        private Grid _rootGrid = new Grid();
        private ObservableCollection<TreeGridColumn> _columnDefinitions = new ObservableCollection<TreeGridColumn>();
        private ObservableCollection<TreeGridRow> _rows = new ObservableCollection<TreeGridRow>();
        private Dictionary<RowDefinition, ItemInfo> _rowItemMap = new Dictionary<RowDefinition, ItemInfo>(); 
        private bool _headerRowAdded = false;


        #region CTOR

        public TreeGrid()
        {
            this.DefaultStyleKey = typeof(TreeGrid);
            _columnDefinitions.CollectionChanged += _columnDefinitions_CollectionChanged;
            _rows.CollectionChanged += _rows_CollectionChanged;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _root = GetTemplateChild("RootScrollViewer") as ScrollViewer;
            _root.Content = _rootGrid;
        }

        #endregion

        #region Data Binding support

        public DataTemplate ItemTemplate
        {
            get;
            set;
        }

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
                //ClearGrid();
                foreach (var item in value)
                {
                    TreeGridRow row = ItemTemplate.LoadContent() as TreeGridRow;
                    row.DataContext = item;
                    foreach (var cell in row.Cells)
                    {
                        cell.DataContext = item;
                    }
                    AddRow(row);
                }
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(TreeGrid), null);
        #endregion

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
                _rootGrid.ColumnDefinitions.Add(column.GridColumn);
                int idx = _columnDefinitions.IndexOf(column);

                column.Splitter.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                _rootGrid.Children.Add(column.Splitter);
                column.SplitterColumn.Width = new GridLength(column.Splitter.ActualWidth);
                _rootGrid.ColumnDefinitions.Add(column.SplitterColumn);
                Grid.SetColumn(column.Splitter, idx * 2 + 1);
                Grid.SetRow(column.Splitter, 0);
                Grid.SetRowSpan(column.Splitter, int.MaxValue);
                //Hide the last column
                if (idx > 0)
                {
                    var previousColumn = _columnDefinitions[idx - 1];
                    previousColumn.Splitter.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
            }
        }

        void _rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                AddRows(e.NewItems.Cast<TreeGridRow>());
            }
        }

        private void AddRows(IEnumerable<TreeGridRow> newRows)
        {
            foreach (TreeGridRow item in newRows)
            {
                AddRow(item);
            }
        }

        private void AddRow(TreeGridRow item)
        {
            //Check that the number of columns matches the item fields
#if DEBUG
            if (item.Cells.Count() > _columnDefinitions.Count)
            {
                throw new ArgumentException("Number of fields is different than numbers of the grid columns");
            }
#endif
            //Create new row at the end
            var itemInfo = new ItemInfo() { IndentLevel = 0 };
            InsertNewRows(_rootGrid.RowDefinitions.Count, 1);
            UpdateRowForItem(item, _rootGrid.RowDefinitions.Count - 2, itemInfo);
        }

        private void UpdateRowForItem(TreeGridRow item, int index, ItemInfo info)
        {
            //Populate columns
            var fields = item.Cells.ToList();
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
            AddSplitterForRow(index + 1, item);
        }

        private void AddToGrid(int row, int column, FrameworkElement item)
        {
            _rootGrid.Children.Add(item);
            Grid.SetColumn(item, column * 2);
            Grid.SetRow(item, row);
        }

        void row_Tapped(object sender, TappedRoutedEventArgs e)
        {

            TreeGridIcon icon = (TreeGridIcon)sender;
            var tag = (ItemInfo)icon.Tag;
            var sp = (StackPanel)tag.Root;
            var item = (TreeGridRow) sp.Tag;
            //Cache children

            var currentIndex = Grid.GetRow(sp);
            var parentInfo = _rowItemMap[_rootGrid.RowDefinitions[currentIndex]];
            if(icon.Collapsed)
            {
                var children = ExpandCallback(item.ExpandCallbackArg);
                icon.Collapsed = false;
                //TODO - add animation if it takes too long to get the children
                icon.Content = ExpandedIcon;
                //insert new rows
                var count = children.Count();
                InsertNewRows(currentIndex + 2, count);
                UpdateExistingRows(currentIndex, count);
                foreach (var child in children)
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
        private void UpdateExistingRows(int currentIndex, int count)
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

        #region Insert new rows

        private void InsertNewRows(int currentIndex, int count)
        {
            var index = currentIndex;
            for (int i = 0; i < count; i++)
			{
                //item row
                var r = new RowDefinition() { Height = GridLength.Auto };
                _rootGrid.RowDefinitions.Insert(index,r);
                index++;
                //splitter row
                r = new RowDefinition() { Height = GridLength.Auto };
                _rootGrid.RowDefinitions.Insert(index,r);
                index++;
			}
        }
        private void AddSplitterForRow(int index, TreeGridRow row)
        {
            _rootGrid.Children.Add(row.Splitter);
            _rootGrid.RowDefinitions[index].Height = new GridLength(row.Splitter.ActualHeight);
            Grid.SetRow(row.Splitter, index);
            Grid.SetColumn(row.Splitter, 0);
            Grid.SetColumnSpan(row.Splitter, int.MaxValue);
        }

        #endregion



        #region Row and Column collections

        public ObservableCollection<TreeGridColumn> Columns
        {
            get
            {
                return _columnDefinitions;
            }
        }
        
       

        public ObservableCollection<TreeGridRow> Rows
        {
            get 
            {
                return _rows;
            }
        }

        #endregion

        #region Header row

        public TreeGridRow Header
        {
            get 
            { 
                return (TreeGridRow)GetValue(HeaderProperty); 
            }
            set 
            { 
                SetValue(HeaderProperty, value);
                AddHeader(value);
            }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(TreeGridRow),
                typeof(TreeGrid),
                null);

        private void AddHeader(TreeGridRow row)
        {

            //TODO get rid of this and allow replacing the header
            if (_headerRowAdded)
            {
                throw new InvalidOperationException("Header already exists");
            }
            if (row != null)
            {

                InsertNewRows(0, 1); //Add Header row
                UpdateExistingRows(0, 1);

                var fields = row.Cells.ToList();
                for (int i = 0; i < fields.Count; i++)
                {
                    AddToGrid(0, i, fields[i]);
                }

                AddSplitterForRow(1, row);
                _headerRowAdded = true;
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

        #region Collapsed and Expanded Icons

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

        #region Settings and call back function
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

        public static readonly DependencyProperty IndentSizeProperty = 
            DependencyProperty.Register(
                "IndentSize",
                typeof(int),
                typeof(TreeGrid),
                null);

        public Func<object, IEnumerable<TreeGridRow>> ExpandCallback { get; set; }

        #endregion

        #region helper classes

        class ItemInfo
        {
            public object Root { get; set; }
            public int IndentLevel { get; set; }
        }

        #endregion




    }
}
