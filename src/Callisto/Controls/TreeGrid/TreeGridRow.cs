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
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Callisto.Controls
{
    public class TreeGridRow : Control
    {
        private ObservableCollection<TreeGridCell> _cells = new ObservableCollection<TreeGridCell>();
        
        #region CTOR
        public TreeGridRow(){}

        #endregion

        #region fields and children collections
        public ObservableCollection<TreeGridCell> Cells { get { return _cells; } }

        #endregion

        #region Settings


        public object ExpandCallbackArg
        {
            get { return (object)GetValue(ExpandCallbackArgProperty); }
            set { SetValue(ExpandCallbackArgProperty, value); }
        }

        public static readonly DependencyProperty ExpandCallbackArgProperty =
            DependencyProperty.Register("ExpandCallbackArg", typeof(object), typeof(TreeGridRow), new PropertyMetadata(null));

        public bool HasChildren
        {
            get { return (bool)GetValue(HasChildrenProperty); }
            set { SetValue(HasChildrenProperty, value); }
        }

        public static readonly DependencyProperty HasChildrenProperty =
            DependencyProperty.Register("HasChildren", typeof(bool), typeof(TreeGridRow), new PropertyMetadata(false));

        public bool AllowResize
        {           
            get { return (bool)GetValue(AllowResizeProperty); }
            set 
            {
                Splitter.AllowResizeRow = value;
                SetValue(AllowResizeProperty, value); 
            }
        }

        public static readonly DependencyProperty AllowResizeProperty =
            DependencyProperty.Register("AllowResize", typeof(bool), typeof(TreeGridRow), new PropertyMetadata(false));


#endregion

        #region Splitter properties

        internal GridSplitter Splitter = new GridSplitter() { Kind = SplitterKind.Row, Height=1, Padding=new Thickness(0,2,0,2), Background= new SolidColorBrush(Colors.Black), Opacity=0.5 };

        public double SplitterHeight
        {
            get 
            { 
                return (double)GetValue(SplitterHeightProperty); 
            }
            set 
            {
                Splitter.Height = value;
                SetValue(SplitterHeightProperty, value); 
            }
        }

        public static readonly DependencyProperty SplitterHeightProperty =
            DependencyProperty.Register("SplitterHeight", typeof(double), typeof(TreeGridRow), new PropertyMetadata(0));



        public double SplitterActualHeight
        {
            get
            {
                return Splitter.ActualHeight;
            }
        }



        public Brush SplitterBrush
        {
            get 
            { 
                return (Brush)GetValue(SplitterBrushProperty); 
            }
            set 
            {
                Splitter.Background = value;
                SetValue(SplitterBrushProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for SplitterBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SplitterBrushProperty =
            DependencyProperty.Register("SplitterBrush", typeof(Brush), typeof(TreeGridRow), null);



        public double SplitterOpacity
        {
            get 
            { 
                return (double)GetValue(SplitterOpacityProperty); 
            }
            set 
            {
                Splitter.Opacity = value;
                SetValue(SplitterOpacityProperty, value); 
            }
        }

        public static readonly DependencyProperty SplitterOpacityProperty =
            DependencyProperty.Register("SplitterOpacity", typeof(double), typeof(TreeGridRow), new PropertyMetadata(1));


        public Thickness SplitterPadding
        {
            get 
            { 
                return (Thickness)GetValue(SplitterPaddingProperty); 
            }
            set 
            {
                Splitter.Padding = value;
                SetValue(SplitterPaddingProperty, value); 
            }
        }

        // Using a DependencyProperty as the backing store for SplitterPadding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SplitterPaddingProperty =
            DependencyProperty.Register("SplitterPadding", typeof(Thickness), typeof(TreeGridRow), null);

        #endregion

    }
}
