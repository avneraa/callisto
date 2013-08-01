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
        private List<object> _fields = new List<object>();
        private Func<TreeGridItem, IEnumerable<TreeGridItem>> _getChildren;

        public TreeGridItem()
        {

        }

        public TreeGridItem(Func<TreeGridItem, IEnumerable<TreeGridItem>> getChildren)
        {
            _getChildren = getChildren;
        }
        public List<object> Fields
        {
            get
            {
                return _fields;
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
    }
}
