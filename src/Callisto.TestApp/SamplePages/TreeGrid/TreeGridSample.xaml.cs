using Callisto.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

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
            SampleGrid.ColumnDefinitions.Add(new TreeGridColumn() { Content = "Name" });
            SampleGrid.ColumnDefinitions.Add(new TreeGridColumn() { Content = "Value" });
            SampleGrid.ColumnDefinitions.Add(new TreeGridColumn() { Content = "Type" });

            var sampleData = new SampleParseTreeData();
            foreach (var parseNode in sampleData.RootNodes)
            {
                var t = CreateTreeGridItem(parseNode);
                SampleGrid.Items.Add(t);
            }
        }

        private TreeGridItem CreateTreeGridItem(ParseNode parseNode)
        {
            var item = new TreeGridItem(GetChildren) { Fields = GetCellsForParseNode(parseNode) };
            item.HasChildren = parseNode.HasChildren;
            item.Tag = parseNode;
            return item;
        }

        private List<TreeGridCell> GetCellsForParseNode(ParseNode parseNode)
        {
            var l = new List<TreeGridCell>();
            l.Add(new TreeGridCell() { Content = parseNode.Name });
            l.Add(new TreeGridCell() { Content = parseNode.Value });
            l.Add(new TreeGridCell() { Content = parseNode.Type });
            return l;
        }

        IEnumerable<TreeGridItem> GetChildren(TreeGridItem parent)
        {
            var pn = (ParseNode)parent.Tag;
            return pn.Children.Select((c) => CreateTreeGridItem(c));
        }



    }
}
