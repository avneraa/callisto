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

// This control is using some of the design and logic of the CustomGridSplitter control in the WinRTXamlToolkit http://winrtxamltoolkit.codeplex.com/ 

using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;



namespace Callisto.Controls
{
    internal sealed class GridSplitter : UserControl
    {

        private static SolidColorBrush _transparent = new SolidColorBrush(Colors.Transparent);
        Border _border = new Border() { BorderBrush = _transparent };
        Rectangle _rect = new Rectangle();
        Grid _parentGrid;

        public GridSplitter()
        {
            _border.Child = _rect;
            this.Content = _border;
            _border.PointerEntered += GridSplitter_PointerEntered;
            _border.PointerExited += GridSplitter_PointerExited;
        }

        void GridSplitter_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 3);
        }

        void GridSplitter_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _currentCursor = Window.Current.CoreWindow.PointerCursor;
            if (Kind == SplitterKind.Column && AllowResizeColumn)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
            }
            else if(AllowResizeRow)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 2);
            }

        }

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            base.OnDoubleTapped(e);
            if (Kind == SplitterKind.Column && AllowResizeColumn)
            {
                var idx = Grid.GetColumn(this);
                var cLeft = GetGrid().ColumnDefinitions[idx - 1];
                var cRight = GetGrid().ColumnDefinitions[idx + 1];
                cLeft.Width = GridLength.Auto;
                var columnCount = GetGrid().ColumnDefinitions.Count;
                if(idx + 1 != columnCount -2)
                {
                    cRight.Width = GridLength.Auto;
                }
                //Keep the last column to 1* so that the grid does not shrink
                GetGrid().ColumnDefinitions[columnCount-2].Width = new GridLength(1, GridUnitType.Star);
            }
            else if (AllowResizeRow)
            {
                var idx = Grid.GetRow(this);
                var rLeft = GetGrid().RowDefinitions[idx - 1];
                var rRight = GetGrid().RowDefinitions[idx + 1];
                rLeft.Height = GridLength.Auto;
                if(idx != GetGrid().RowDefinitions.Count -1)
                {
                    rRight.Height = GridLength.Auto;
                }
            }
        }

        #region AllowResize setters

        public bool AllowResizeRow { get; set; }
        public bool AllowResizeColumn { get; set;}

        #endregion

        #region Resizing event handlers and variables

        CoreCursor _currentCursor;
        private Point _lastPosition;
        private bool _isDragging;


        private uint? _dragPointer;

        /// <summary>
        /// Called before the PointerPressed event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerPressed(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_dragPointer != null)
                return;

            _parentGrid = GetGrid();
            _dragPointer = e.Pointer.PointerId;
            _lastPosition = e.GetCurrentPoint(_parentGrid).Position;
            _isDragging = true;
            StartDirectDragging(e);
        }

        private Grid GetGrid()
        {
            return this.Parent as Grid;
        }


        private void StartDirectDragging(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.CapturePointer(e.Pointer);
            this.Focus(FocusState.Pointer);
        }

        /// <summary>
        /// Called before the PointerMoved event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerMoved(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_isDragging || _dragPointer != e.Pointer.PointerId)
            {
                return;
            }

            var position = e.GetCurrentPoint(_parentGrid).Position;
            ContinueDirectDragging(position);

            _lastPosition = position;
        }

        private void ContinueDirectDragging(Point position)
        {
            if (Kind == SplitterKind.Column && AllowResizeColumn)
            {
                var deltaX = position.X - _lastPosition.X;
                this.ResizeColumns(_parentGrid, deltaX);
            }
            else if(AllowResizeRow)
            {
                var deltaY = position.Y - _lastPosition.Y;
                this.ResizeRows(_parentGrid, deltaY);
            }
        }

        /// <summary>
        /// Called before the PointerReleased event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerReleased(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_isDragging ||
                _dragPointer != e.Pointer.PointerId)
            {
                return;
            }

            this.ReleasePointerCapture(e.Pointer);
            _isDragging = false;
            _dragPointer = null;
            _parentGrid = null;
            //Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 3);
        }

        #endregion

        #region ResizeColumns()
        private void ResizeColumns(Grid grid, double deltaX)
        {
            int column = Grid.GetColumn(this);
            int leftColumn;
            int rightColumn;


            leftColumn = column - 1;
            rightColumn = column + 1;

            if (rightColumn >= grid.ColumnDefinitions.Count)
            {
                return;
            }

            var leftColumnDefinition = grid.ColumnDefinitions[leftColumn];
            var rightColumnDefinition = grid.ColumnDefinitions[rightColumn];
            var leftColumnGridUnitType = leftColumnDefinition.Width.GridUnitType;
            var rightColumnGridUnitType = rightColumnDefinition.Width.GridUnitType;
            var leftColumnActualWidth = leftColumnDefinition.ActualWidth;
            var rightColumnActualWidth = rightColumnDefinition.ActualWidth;
            var leftColumnMaxWidth = leftColumnDefinition.MaxWidth;
            var rightColumnMaxWidth = rightColumnDefinition.MaxWidth;
            var leftColumnMinWidth = leftColumnDefinition.MinWidth;
            var rightColumnMinWidth = rightColumnDefinition.MinWidth;

            //deltaX = 200;
            if (leftColumnActualWidth + deltaX > leftColumnMaxWidth)
            {
                deltaX = Math.Max(0, leftColumnDefinition.MaxWidth - leftColumnActualWidth);
            }

            if (leftColumnActualWidth + deltaX < leftColumnMinWidth)
            {
                deltaX = Math.Min(0, leftColumnDefinition.MinWidth - leftColumnActualWidth);
            }

            if (rightColumnActualWidth - deltaX > rightColumnMaxWidth)
            {
                deltaX = -Math.Max(0, rightColumnDefinition.MaxWidth - rightColumnActualWidth);
            }

            if (rightColumnActualWidth - deltaX < rightColumnMinWidth)
            {
                deltaX = -Math.Min(0, rightColumnDefinition.MinWidth - rightColumnActualWidth);
            }

            var newLeftColumnActualWidth = leftColumnActualWidth + deltaX;
            var newRightColumnActualWidth = rightColumnActualWidth - deltaX;

            double starColumnsAvailableWidth = grid.ActualWidth;

            rightColumnDefinition.Width = new GridLength(newRightColumnActualWidth, GridUnitType.Pixel);
            leftColumnDefinition.Width = new GridLength(newLeftColumnActualWidth, GridUnitType.Pixel);

        }
        #endregion

        #region ResizeRows()
        private void ResizeRows(Grid grid, double deltaY)
        {

            int row = Grid.GetRow(this);
            int upperRow;
            int lowerRow;


            upperRow = row - 1;
            lowerRow = row + 1;
            var upperRowDefinition = grid.RowDefinitions[upperRow];
            var upperRowActualHeight = upperRowDefinition.ActualHeight;
            var upperRowMaxHeight = upperRowDefinition.MaxHeight;
            var upperRowMinHeight = upperRowDefinition.MinHeight;

            if (upperRowActualHeight + deltaY > upperRowMaxHeight)
            {
                deltaY = Math.Max(0, upperRowDefinition.MaxHeight - upperRowActualHeight);
            }

            if (upperRowActualHeight + deltaY < upperRowMinHeight)
            {
                deltaY = Math.Min(0, upperRowDefinition.MinHeight - upperRowActualHeight);
            }

            var yInGrid = ((MatrixTransform)this.TransformToVisual(grid)).Matrix.OffsetY;
            var scrollViewerMargin = ((ScrollViewer)grid.Parent).ComputedHorizontalScrollBarVisibility == Visibility.Visible ? 16 : 4;

            var newUpperRowActualHeight = upperRowActualHeight + deltaY;
            if (yInGrid + scrollViewerMargin + deltaY < grid.ActualHeight)
            {
                upperRowDefinition.Height = new GridLength(newUpperRowActualHeight, GridUnitType.Pixel);
            }

            if (lowerRow >= grid.RowDefinitions.Count)
            {
                return;
            }

            var lowerRowDefinition = grid.RowDefinitions[lowerRow];
            var lowerRowActualHeight = lowerRowDefinition.ActualHeight;
            var lowerRowMaxHeight = lowerRowDefinition.MaxHeight;
            var lowerRowMinHeight = lowerRowDefinition.MinHeight;

            if (lowerRowActualHeight - deltaY > lowerRowMaxHeight)
            {
                deltaY = -Math.Max(0, lowerRowDefinition.MaxHeight - lowerRowActualHeight);
            }

            if (lowerRowActualHeight - deltaY < lowerRowMinHeight)
            {
                deltaY = -Math.Min(0, lowerRowDefinition.MinHeight - lowerRowActualHeight);
            }


            var newLowerRowActualHeight = lowerRowActualHeight - deltaY;

            double starRowsAvailableHeight = grid.ActualHeight;

            lowerRowDefinition.Height = new GridLength(newLowerRowActualHeight, GridUnitType.Pixel);

        }
        #endregion

        #region Properties (Kind, Width, Height)

        public SplitterKind Kind { get; set; }

        new public double Width
        {
            get
            {
                return _rect.Width;
            }
            set
            {
                _rect.Width = value;
            }
        }

        new public double ActualWidth
        {
            get
            {
                if(Kind == SplitterKind.Column)
                {
                return _rect.Width + _border.BorderThickness.Left * 2;
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        new public double Height
        {
            get
            {
                return _rect.Height;
            }
            set
            {
                _rect.Height = value;
            }
        }

        new public double ActualHeight
        {
            get
            {
                if (Kind == SplitterKind.Row)
                {
                    var borderTop = _border.BorderThickness.Top == double.NaN ? 0 : _border.BorderThickness.Top;
                    return _rect.Height + borderTop * 2;
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        new public Brush Background
        {
            get
            {
                return _rect.Fill;
            }
            set
            {
                _rect.Fill = value;
            }
        }

        new public double Opacity
        {
            get
            {
                return _rect.Opacity;
            }
            set
            {
                _rect.Opacity = value;
            }
        }

        new public Thickness Padding
        {
            get
            {
                return _border.BorderThickness;
            }
            set
            {
                _border.BorderThickness = value;
            }
        }
        #endregion
    }

    #region SplitterKind Enum

    internal enum SplitterKind
    {
        Row,
        Column
    }

#endregion

}
