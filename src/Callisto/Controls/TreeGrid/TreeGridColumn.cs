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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Callisto.Controls
{
    public class TreeGridColumn : ContentControl
    {
        private static SolidColorBrush _darkGray = new SolidColorBrush(Colors.DarkGray);
        //private ContentControl _header;

        public TreeGridColumn()
        {
            this.GridColumn = new ColumnDefinition();
            this.SplitterColumn = new ColumnDefinition();
            this.SplitterWidth = 1;
        }
        internal ColumnDefinition GridColumn { get; set; }
        internal ColumnDefinition SplitterColumn;


        internal GridSplitter Splitter = new GridSplitter()
        {
            Kind = SplitterKind.Column
        };


        #region Width 


        /// <summary>
        /// Gets the calculated width of a ColumnDefinition element, or sets the GridLength value of a column that is defined by the ColumnDefinition.
        /// </summary>
        /// <returns>
        /// The GridLength that represents the width of the column. The default value is 1.0.
        /// </returns>
        new public GridLength Width
        {
            get
            {
                return GridColumn.Width;
            }
            set
            {
                GridColumn.Width = value;
            }
        }

        //new public static readonly DependencyProperty WidthProperty =
        //    DependencyProperty.Register(
        //        "Width",
        //        typeof(GridLength),
        //        typeof(TreeGridColumn),
        //        null);

        #endregion



        #region Splitter properties
        public Brush SplitterBrush
        {
            get
            {
                return (Brush)GetValue(SplitterBrushProperty);
            }
            set
            {
                SetValue(SplitterBrushProperty, value);
                Splitter.Background = (Brush)GetValue(SplitterBrushProperty);
            }
        }

        public static readonly DependencyProperty SplitterBrushProperty =
            DependencyProperty.Register(
                "SplitterBrush",
                typeof(Brush),
                typeof(TreeGridColumn),
                null);

        public double SplitterOpacity
        {
            get
            {
                return (double)GetValue(SplitterOpacityProperty);
            }
            set
            {
                SetValue(SplitterOpacityProperty, value);
                Splitter.Opacity = (double)GetValue(SplitterOpacityProperty);
            }
        }

        public static readonly DependencyProperty SplitterOpacityProperty =
            DependencyProperty.Register(
                "SplitterOpacity",
                typeof(double),
                typeof(TreeGridColumn),
                null);

        public double SplitterWidth
        {
            get
            {
                return (double)GetValue(SplitterWidthProperty);
            }
            set
            {
                SetValue(SplitterWidthProperty, value);
                Splitter.Width = (double)GetValue(SplitterWidthProperty);
                SplitterColumn.Width = new GridLength(Splitter.ActualWidth, GridUnitType.Pixel);
            }
        }

        public static readonly DependencyProperty SplitterWidthProperty =
            DependencyProperty.Register(
                "SplitterWidth",
                typeof(double),
                typeof(TreeGridColumn),
                null
            );
        

        public double SplitterPadding
        {
            get
            {
                return (double)GetValue(SplitterPaddingProperty);
            }
            set
            {
                SetValue(SplitterPaddingProperty, value);
                Splitter.Padding = new Thickness(value / 2, 0, value / 2, 0);
            }
        }

        public static readonly DependencyProperty SplitterPaddingProperty =
            DependencyProperty.Register(
                "SplitterPadding",
                typeof(double),
                typeof(TreeGridColumn),
                null
           );
        #endregion


    }
}
