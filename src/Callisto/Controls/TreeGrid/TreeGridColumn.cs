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
    public class TreeGridColumn
    {
        private static SolidColorBrush _darkGray = new SolidColorBrush(Colors.DarkGray);
        private ContentControl _header;

        private static DependencyProperty _widthProperty;
        public TreeGridColumn()
        {
            this.GridColumn = new ColumnDefinition();
            this.SplitterColumn = new ColumnDefinition();
            this.SplitterWidth = 3;
        }
        internal ColumnDefinition GridColumn { get; set; }
        internal ColumnDefinition SplitterColumn { get; set; }

        internal ContentControl HeaderControl
        {
            get 
            {
                return _header;
            }
        }

        internal Rectangle Splitter = new Rectangle()
        {
            Fill = _darkGray,
            Opacity = 0.5,
            Stroke = _darkGray
        };

        public Brush SplitterColor
        {
            get
            {
                return Splitter.Fill;
            }
            set
            {
                Splitter.Fill = value;
                Splitter.Stroke = value;
            }
        }

        public double SplitterWidth
        {
            get
            {
                return Splitter.Width;
            }
            set
            {
                Splitter.Width = value;
                SplitterColumn.Width = new GridLength(Splitter.Width, GridUnitType.Pixel);
            }
        }

        public object Header
        {
            get
            {
                if (_header == null)
                {
                    return null;
                }
                else
                {
                    return _header.Content;
                }
            }
            set
            {
                if(_header == null)
                {
                    _header = new ContentControl();
                }
                if(value == null)
                {
                    _header = null;
                }
                else
                {
                    _header.Content = value;
                }

            }
        }

        /// <summary>
        /// Gets the calculated width of a ColumnDefinition element, or sets the GridLength value of a column that is defined by the ColumnDefinition.
        /// </summary>
        /// <returns>
        /// The GridLength that represents the width of the column. The default value is 1.0.
        /// </returns>
        public GridLength Width
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
        /// <summary>
        /// Identifies the Width dependency property.
        /// </summary>
        public static DependencyProperty WidthProperty
        {
            get
            {
                if (_widthProperty == null)
                {
                    _widthProperty = DependencyProperty.Register(
                        "Width",
                        typeof(GridLength),
                        typeof(TreeGridColumn),
                        new PropertyMetadata(1.0));
                }
                return _widthProperty;
            }
        }
    }
}
