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

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Callisto.Controls
{
    public class TreeGridColumn : Control
    {
        private static SolidColorBrush _darkGray = new SolidColorBrush(Colors.DarkGray);
        //private ContentControl _header;

        public TreeGridColumn()
        {
            this.GridColumn = new ColumnDefinition();
            this.SplitterColumn = new ColumnDefinition() { Width = GridLength.Auto };
            this.SplitterWidth = 1;
            isDefaultWidth = true;
        }
        internal ColumnDefinition GridColumn { get; set; }
        internal ColumnDefinition SplitterColumn;


        internal GridSplitter Splitter = new GridSplitter()
        {
            Kind = SplitterKind.Column
        };


        #region Width 

        internal bool isDefaultWidth { get; set; }

        /// <summary>
        /// Gets the calculated width of a ColumnDefinition element, or sets the GridLength value of a column that is defined by the ColumnDefinition.
        /// </summary>
        /// <returns>
        /// The GridLength that represents the width of the column. The default value is 1.0*.
        /// </returns>
        new public GridLength Width
        {
            get
            {
                return GridColumn.Width;
            }
            set
            {
                isDefaultWidth = false;
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
            }
        }

        public static readonly DependencyProperty SplitterWidthProperty =
            DependencyProperty.Register(
                "SplitterWidth",
                typeof(double),
                typeof(TreeGridColumn),
                null
            );
        

        public Thickness SplitterPadding
        {
            get
            {
                return (Thickness)GetValue(SplitterPaddingProperty);
            }
            set
            {
                SetValue(SplitterPaddingProperty, value);
                Splitter.Padding = value;
            }
        }

        public static readonly DependencyProperty SplitterPaddingProperty =
            DependencyProperty.Register(
                "SplitterPadding",
                typeof(Thickness),
                typeof(TreeGridColumn),
                null
           );

        
        public bool AllowResize
        {
            get { return (bool)GetValue(AllowResizeProperty); }
            set 
            {
                Splitter.AllowResizeColumn = value;
                SetValue(AllowResizeProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for AllowResize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowResizeProperty =
            DependencyProperty.Register("AllowResize", typeof(bool), typeof(TreeGridColumn), new PropertyMetadata(false));


        #endregion


    }
}
