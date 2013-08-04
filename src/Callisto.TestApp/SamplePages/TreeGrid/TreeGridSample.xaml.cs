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

            var sampleData = new SampleParseTreeData();
            foreach (var parseNode in sampleData.RootNodes)
            {
                var t = CreateTreeGridRow(parseNode);
                SampleGrid.Items.Add(t);
            }
        }

        private TreeGridRow CreateTreeGridRow(ParseNode parseNode)
        {
            var row = new TreeGridRow(GetChildren) ;
            AddCellsForParseNode(row, parseNode);
            row.HasChildren = parseNode.HasChildren;
            row.Tag = parseNode;
            return row;
        }

        private void AddCellsForParseNode(TreeGridRow row, ParseNode parseNode)
        {

            row.Cells.Add(new TreeGridCell() { Content = parseNode.Name });
            row.Cells.Add(new TreeGridCell() { Content = parseNode.Value });
            row.Cells.Add(new TreeGridCell() { Content = parseNode.Type });

        }

        IEnumerable<TreeGridRow> GetChildren(TreeGridRow parent)
        {
            var pn = (ParseNode)parent.Tag;
            return pn.Children.Select((c) => CreateTreeGridRow(c));
        }



    }
}
