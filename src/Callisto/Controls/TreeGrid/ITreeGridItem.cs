using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace Callisto.Controls
{
    public interface ITreeGridItem
    {
        IEnumerable<object> Fields {get;}
        IEnumerable<ITreeGridItem> GetChildren();
        bool HasChildren { get; }

    }
}
