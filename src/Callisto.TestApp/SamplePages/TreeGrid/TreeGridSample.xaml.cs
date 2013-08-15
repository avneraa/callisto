using Callisto.Controls;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Callisto.TestApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TreeGridSample : Page
    {
        Brush _white = new SolidColorBrush(Colors.White);
        SampleParseTreeData _sampleData = new SampleParseTreeData();

        public TreeGridSample()
        {
            this.InitializeComponent();
            SampleGrid.ItemsSource = _sampleData.RootNodes;
            SampleGrid.ExpandCallback = GetChildren;
            //PopulateDebuggerGrid();
        }

        private void PopulateDebuggerGrid() 
        {


            foreach (var parseNode in _sampleData.RootNodes)
            {
                var newRow = CreateTreeGridRow(parseNode);
                SampleGrid.Rows.Add(newRow);
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
            row.Cells.Add(new TreeGridCell() { Content = new TextBlock() { Text = parseNode.Value, TextWrapping = TextWrapping.Wrap, Foreground = _white } });
            row.Cells.Add(new TreeGridCell() { Content = new TextBlock() { Text = parseNode.Type, TextWrapping = TextWrapping.Wrap, Foreground = _white } });

        }

        IEnumerable<TreeGridRow> GetChildren(object node)
        {
            var pn = (ParseNode)node;
            return pn.Children.Select((c) => CreateTreeGridRow(c));
        }



    }
}
