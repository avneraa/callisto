using Callisto.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Callisto.TestApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TreeGridSample : Page
    {
        Brush _white = new SolidColorBrush(Colors.White);
        public TreeGridSample()
        {
            this.InitializeComponent();
            PopulateDebuggerGrid();
        }

        private void PopulateDebuggerGrid() 
        {

            var sampleData = new SampleParseTreeData();
            SampleGrid.ExpandCallback = GetChildren;
            foreach (var parseNode in sampleData.RootNodes)
            {
                var t = CreateTreeGridRow(parseNode);
                SampleGrid.Items.Add(t);
            }
        }

        private TreeGridRow CreateTreeGridRow(ParseNode parseNode)
        {
            var row = new TreeGridRow() { AllowResize = true, SplitterBrush = _white };
            AddCellsForParseNode(row, parseNode);
            row.HasChildren = parseNode.HasChildren;
            row.ExpandCallbackArg = parseNode;
            return row;
        }

        private void AddCellsForParseNode(TreeGridRow row, ParseNode parseNode)
        {

            row.Cells.Add(new TreeGridCell() { Content = new TextBlock() { Text = parseNode.Name, TextWrapping = TextWrapping.Wrap, Foreground = _white } });
            row.Cells.Add(new TreeGridCell() { Content = new TextBlock() { Text = parseNode.Name, TextWrapping = TextWrapping.Wrap, Foreground = _white } });
            row.Cells.Add(new TreeGridCell() { Content = new TextBlock() { Text = parseNode.Name, TextWrapping = TextWrapping.Wrap, Foreground = _white } });

        }

        IEnumerable<TreeGridRow> GetChildren(object node)
        {
            var pn = (ParseNode)node;
            return pn.Children.Select((c) => CreateTreeGridRow(c));
        }



    }
}
