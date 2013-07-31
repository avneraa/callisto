using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Callisto.Controls
{
    public sealed class TreeGridIcon : ContentControl
    {
        public TreeGridIcon()
        {
            this.DefaultStyleKey = typeof(TreeGridIcon);
        }

        internal bool Collapsed { get; set; }
    }
}
