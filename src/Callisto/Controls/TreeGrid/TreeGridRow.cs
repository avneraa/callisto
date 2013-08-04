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
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Callisto.Controls
{
    public class TreeGridRow 
    {
        private ObservableCollection<TreeGridCell> _cells = new ObservableCollection<TreeGridCell>();
        private Func<TreeGridRow, IEnumerable<TreeGridRow>> _getChildren;
        private double _splitterPadding;
        
        #region CTOR
        public TreeGridRow(){}

        public TreeGridRow(Func<TreeGridRow, IEnumerable<TreeGridRow>> getChildren)
        {
            _getChildren = getChildren;
        }

        #endregion

        #region fields and children collections
        public ObservableCollection<TreeGridCell> Cells { get { return _cells; } }

        internal IEnumerable<TreeGridRow> GetChildren()
        {
            if (_getChildren != null)
            {
                return _getChildren(this);
            }
            else
            {
                return Enumerable.Empty<TreeGridRow>();
            }
        }

        #endregion

        #region Settings

        public object Tag { get; set; }

        public bool HasChildren { get; set; }

        //public GridLength Height {get;set;}
        //{
        //    get
        //    {
        //        return _rowHeight;
        //    }
        //    set
        //    {
        //        _rowHeight = value;
        //    }
        //}
#endregion

        #region Splitter properties

        internal GridSplitter Splitter = new GridSplitter() { Kind = SplitterKind.Row, Padding = new Thickness(0,0,0,0), Height = 0 };
        public double SplitterHeight
        {
            get
            {
                return Splitter.Height;
            }
            set
            {
                Splitter.Height = value;
            }
        }

        public double SplitterActualHeight
        {
            get
            {
                return Splitter.ActualHeight;
            }
        }

        //public static readonly DependencyProperty SplitterHeightProperty =
        //    DependencyProperty.Register(
        //        "SplitterHeight",
        //        typeof(double),
        //        typeof(TreeGridRow),
        //        null);

        public Brush SplitterBrush
        {
            get
            {
                return Splitter.Background;
            }
            set
            {
                Splitter.Background = value;
            }
        }

        public double SplitterOpacity
        {
            get
            {
                return Splitter.Opacity;
            }
            set
            {
                Splitter.Opacity = value;
            }
        }

        public double SplitterPadding
        {
            get
            {
                return _splitterPadding;
            }
            set
            {
                Splitter.Padding = new Thickness(0, value / 2, 0, value / 2);
            }
        }

        #endregion

        //TODO Expose rowheight and apply it to the underlying GridRow 
    }
}
