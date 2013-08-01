using Callisto.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Callisto.TestApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TreeGridSample : Page
    {
        public TreeGridSample()
        {
            this.InitializeComponent();
            PopulateDebuggerGrid();
        }

        private void PopulateDebuggerGrid()
        {
            SampleGrid.ColumnDefinitions.Add(new TreeGridColumn() { Header = "Name" });
            SampleGrid.ColumnDefinitions.Add(new TreeGridColumn() { Header = "Value" });
            SampleGrid.ColumnDefinitions.Add(new TreeGridColumn() { Header = "Type" });

            var sampleData = new SampleParseTreeData();
            foreach (var parseNode in sampleData.RootNodes)
            {
                var t = CreateTreeGridItem(parseNode);
                SampleGrid.Items.Add(t);
            }
        }

        private TreeGridItem CreateTreeGridItem(ParseNode parseNode)
        {
            var item = new TreeGridItem(GetChildren);
            item.Fields.Add(parseNode.Name);
            item.Fields.Add(parseNode.Value);
            item.Fields.Add(parseNode.Type);
            item.HasChildren = parseNode.HasChildren;
            item.Tag = parseNode;
            return item;
        }

        IEnumerable<TreeGridItem> GetChildren(TreeGridItem parent)
        {
            var pn = (ParseNode)parent.Tag;
            return pn.Children.Select((c) => CreateTreeGridItem(c));
        }



    }
}
