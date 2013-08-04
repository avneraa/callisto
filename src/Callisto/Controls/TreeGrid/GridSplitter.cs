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
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// This control is inspired by CustomGridSplitter control in the WinRTXamlToolkit http://winrtxamltoolkit.codeplex.com/ 

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
            if (Kind == SplitterKind.Column)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
            }
            else
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeNorthSouth, 2);
            }

        }

        /// <summary>
        /// Occurs when dragging completes.
        /// </summary>
        public event EventHandler DraggingCompleted;

        #region Resizing event handlers and variables

        CoreCursor _currentCursor;
        private Point _lastPosition;
        private Point _previewDraggingStartPosition;
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
            if (Kind == SplitterKind.Column)
            {
                var deltaX = position.X - _lastPosition.X;
                this.ResizeColumns(_parentGrid, deltaX);
            }
            else
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
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 3);
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

            //grid.BeginInit();

            double totalStarColumnsWidth = 0;
            double starColumnsAvailableWidth = grid.ActualWidth;

            if (leftColumnGridUnitType == GridUnitType.Star || rightColumnGridUnitType == GridUnitType.Star)
            {
                foreach (var columnDefinition in grid.ColumnDefinitions)
                {
                    if (columnDefinition.Width.GridUnitType == GridUnitType.Star)
                    {
                        totalStarColumnsWidth += columnDefinition.Width.Value;
                    }
                    else
                    {
                        starColumnsAvailableWidth -= columnDefinition.ActualWidth;
                    }
                }
            }

            if (leftColumnGridUnitType == GridUnitType.Star)
            {
                if (rightColumnGridUnitType == GridUnitType.Star)
                {
                    // If both columns are star columns
                    // - totalStarColumnsWidth won't change and
                    // as much as one of the columns grows
                    // - the other column will shrink by the same value.

                    // If there is no width available to star columns
                    // - we can't resize two of them.
                    if (starColumnsAvailableWidth < 1)
                    {
                        return;
                    }

                    var oldStarWidth = leftColumnDefinition.Width.Value;
                    var newStarWidth = Math.Max(0, totalStarColumnsWidth * newLeftColumnActualWidth / starColumnsAvailableWidth);
                    leftColumnDefinition.Width = new GridLength(newStarWidth, GridUnitType.Star);

                    rightColumnDefinition.Width = new GridLength(Math.Max(0, rightColumnDefinition.Width.Value - newStarWidth + oldStarWidth), GridUnitType.Star);
                }
                else
                {
                    var newStarColumnsAvailableWidth =
                        starColumnsAvailableWidth +
                        rightColumnActualWidth -
                        newRightColumnActualWidth;

                    if (newStarColumnsAvailableWidth - newLeftColumnActualWidth >= 1)
                    {
                        var newStarWidth = Math.Max(
                            0,
                            (totalStarColumnsWidth -
                             leftColumnDefinition.Width.Value) *
                            newLeftColumnActualWidth /
                            (newStarColumnsAvailableWidth - newLeftColumnActualWidth));

                        leftColumnDefinition.Width =
                            new GridLength(newStarWidth, GridUnitType.Star);
                    }
                }
            }
            else
            {
                leftColumnDefinition.Width = new GridLength(newLeftColumnActualWidth, GridUnitType.Pixel);
            }

            if (rightColumnGridUnitType == GridUnitType.Star)
            {
                if (leftColumnGridUnitType != GridUnitType.Star)
                {
                    var newStarColumnsAvailableWidth = starColumnsAvailableWidth + leftColumnActualWidth - newLeftColumnActualWidth;

                    if (newStarColumnsAvailableWidth - newRightColumnActualWidth >= 1)
                    {
                        var newStarWidth = Math.Max(
                            0,
                            (totalStarColumnsWidth -
                             rightColumnDefinition.Width.Value) *
                            newRightColumnActualWidth /
                            (newStarColumnsAvailableWidth - newRightColumnActualWidth));
                        rightColumnDefinition.Width = new GridLength(newStarWidth, GridUnitType.Star);
                    }
                }
                // else handled in the left column width calculation block
            }
            else
            {
                rightColumnDefinition.Width = new GridLength(newRightColumnActualWidth, GridUnitType.Pixel);
            }

            //grid.EndInit();
        }
        #endregion

        #region ResizeRows()
        private void ResizeRows(Grid grid, double deltaX)
        {

            int row = Grid.GetRow(this);
            int upperRow;
            int lowerRow;


            upperRow = row;
            lowerRow = row + 1;


            if (lowerRow >= grid.RowDefinitions.Count)
            {
                return;
            }

            var upperRowDefinition = grid.RowDefinitions[upperRow];
            var lowerRowDefinition = grid.RowDefinitions[lowerRow];
            var upperRowGridUnitType = upperRowDefinition.Height.GridUnitType;
            var lowerRowGridUnitType = lowerRowDefinition.Height.GridUnitType;
            var upperRowActualHeight = upperRowDefinition.ActualHeight;
            var lowerRowActualHeight = lowerRowDefinition.ActualHeight;
            var upperRowMaxHeight = upperRowDefinition.MaxHeight;
            var lowerRowMaxHeight = lowerRowDefinition.MaxHeight;
            var upperRowMinHeight = upperRowDefinition.MinHeight;
            var lowerRowMinHeight = lowerRowDefinition.MinHeight;

            //deltaX = 200;
            if (upperRowActualHeight + deltaX > upperRowMaxHeight)
            {
                deltaX = Math.Max(
                    0,
                    upperRowDefinition.MaxHeight - upperRowActualHeight);
            }

            if (upperRowActualHeight + deltaX < upperRowMinHeight)
            {
                deltaX = Math.Min(
                    0,
                    upperRowDefinition.MinHeight - upperRowActualHeight);
            }

            if (lowerRowActualHeight - deltaX > lowerRowMaxHeight)
            {
                deltaX = -Math.Max(
                    0,
                    lowerRowDefinition.MaxHeight - lowerRowActualHeight);
            }

            if (lowerRowActualHeight - deltaX < lowerRowMinHeight)
            {
                deltaX = -Math.Min(
                    0,
                    lowerRowDefinition.MinHeight - lowerRowActualHeight);
            }

            var newUpperRowActualHeight = upperRowActualHeight + deltaX;
            var newLowerRowActualHeight = lowerRowActualHeight - deltaX;

            //grid.BeginInit();

            double totalStarRowsHeight = 0;
            double starRowsAvailableHeight = grid.ActualHeight;

            if (upperRowGridUnitType == GridUnitType.Star || lowerRowGridUnitType == GridUnitType.Star)
            {
                foreach (var rowDefinition in grid.RowDefinitions)
                {
                    if (rowDefinition.Height.GridUnitType == GridUnitType.Star)
                    {
                        totalStarRowsHeight += rowDefinition.Height.Value;
                    }
                    else
                    {
                        starRowsAvailableHeight -= rowDefinition.ActualHeight;
                    }
                }
            }

            if (upperRowGridUnitType == GridUnitType.Star)
            {
                if (lowerRowGridUnitType == GridUnitType.Star)
                {
                    // If both rows are star rows
                    // - totalStarRowsHeight won't change and
                    // as much as one of the rows grows
                    // - the other row will shrink by the same value.

                    // If there is no width available to star rows
                    // - we can't resize two of them.
                    if (starRowsAvailableHeight < 1)
                    {
                        return;
                    }

                    var oldStarHeight = upperRowDefinition.Height.Value;
                    var newStarHeight = Math.Max(
                        0,
                        totalStarRowsHeight * newUpperRowActualHeight /
                        starRowsAvailableHeight);
                    upperRowDefinition.Height = new GridLength(newStarHeight, GridUnitType.Star);

                    lowerRowDefinition.Height =
                        new GridLength(
                            Math.Max(
                                0,
                                lowerRowDefinition.Height.Value -
                                    newStarHeight + oldStarHeight),
                            GridUnitType.Star);
                }
                else
                {
                    var newStarRowsAvailableHeight = starRowsAvailableHeight + lowerRowActualHeight - newLowerRowActualHeight;

                    if (newStarRowsAvailableHeight - newUpperRowActualHeight >= 1)
                    {
                        var newStarHeight = Math.Max(
                            0,
                            (totalStarRowsHeight -
                             upperRowDefinition.Height.Value) *
                            newUpperRowActualHeight /
                            (newStarRowsAvailableHeight - newUpperRowActualHeight));

                        upperRowDefinition.Height = new GridLength(newStarHeight, GridUnitType.Star);
                    }
                }
            }
            else
            {
                upperRowDefinition.Height = new GridLength(newUpperRowActualHeight, GridUnitType.Pixel);
            }

            if (lowerRowGridUnitType == GridUnitType.Star)
            {
                if (upperRowGridUnitType != GridUnitType.Star)
                {
                    var newStarRowsAvailableHeight = starRowsAvailableHeight + upperRowActualHeight - newUpperRowActualHeight;

                    if (newStarRowsAvailableHeight - newLowerRowActualHeight >= 1)
                    {
                        var newStarHeight = Math.Max(
                            0,
                            (totalStarRowsHeight -
                             lowerRowDefinition.Height.Value) *
                            newLowerRowActualHeight /
                            (newStarRowsAvailableHeight - newLowerRowActualHeight));

                        lowerRowDefinition.Height = new GridLength(newStarHeight, GridUnitType.Star);
                    }
                }
                // else handled in the upper row width calculation block
            }
            else
            {
                lowerRowDefinition.Height = new GridLength(newLowerRowActualHeight, GridUnitType.Pixel);
            }

            //grid.EndInit();
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
                    return _rect.Height + _border.BorderThickness.Top * 2;
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
    }

        #endregion

    internal enum SplitterKind
    {
        Row,
        Column
    }

}
