using Callisto.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Callisto.TestApp.SamplePages
{
    public class ParseNodeItem : ITreeGridItem 
    {
        ParseNode _node;
        public ParseNodeItem(ParseNode node)
        {
            _node = node;
        }
        
        public IEnumerable<object> Fields
        {
            get 
            {
                yield return _node.Name;
                yield return _node.Value;
                yield return _node.Type;
            }
        }

        public IEnumerable<ITreeGridItem> GetChildren()
        {
            return _node.Children.Select((pn) => new ParseNodeItem(pn));
        }

        public bool HasChildren
        {
            get
            {
                return _node.HasChildren;
            }
        }

    }
}
