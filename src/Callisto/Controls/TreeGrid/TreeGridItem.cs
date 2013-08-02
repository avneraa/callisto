using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Callisto.Controls
{
    public class TreeGridItem
    {
        private IEnumerable<TreeGridCell> _fields;
        private Func<TreeGridItem, IEnumerable<TreeGridItem>> _getChildren;

        public TreeGridItem()
        {

        }

        public TreeGridItem(Func<TreeGridItem, IEnumerable<TreeGridItem>> getChildren)
        {
            _getChildren = getChildren;
        }
        public IEnumerable<TreeGridCell> Fields
        {
            get
            {
                if(_fields == null)
                {
                    _fields = new List<TreeGridCell>();
                }
                return _fields;

            }
            set
            {
                _fields = value;
            }
        }
        internal IEnumerable<TreeGridItem> GetChildren()
        {

                if (_getChildren != null)
                {
                    return _getChildren(this);
                }
                else
                {
                    return Enumerable.Empty<TreeGridItem>();
                }

        }

        public object Tag { get; set; }

        public bool HasChildren { get; set; }

        //TODO Expose rowheight and apply it to the underlying GridRow 
    }
}
