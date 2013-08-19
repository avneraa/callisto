using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callisto.TestApp.SamplePages
{
    public class SampleParseTreeData
    {
        List<ParseNode> _rootNodes = new List<ParseNode>();
        public List<ParseNode> RootNodes { get { return _rootNodes; } }
        public SampleParseTreeData()
        {
            var node = new ParseNode() { Name = "IdentifierNode", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", Type = "string", HasChildren = true, Children = new List<ParseNode>() };
            _rootNodes.Add(node);
            node.Children.Add(new ParseNode() { Name = "Attributes", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.AttributeSectionNode]", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "Identifier", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "Bases", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.TypeBaseNode]", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "TypeParameterList", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.TypeBaseNode]", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "TypeParameterConstraintsClauses", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.TypeParameterConstraintsClauseNode]", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "MemberDeclarations", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.MethodDeclarationNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.MemberDeclarationNode", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "StartToken", Value = "3false", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "OpenToken", Value = "32", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "CloseToken", Value = "44", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "CloseAngleToken", Value = "-true", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "ParseTree", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.SourceFileParseTree", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTree", HasChildren = true, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "Kind", Value = "ClassDeclaration", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.NodeKind", HasChildren = false, Children = new List<ParseNode>() });
            //node.Children.Add(new ParseNode() { Name = "Parent", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.NamespaceDeclarationNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNode", HasChildren = true, Children = new List<ParseNode>() });
            node = new ParseNode() { Name = "ClassDeclarationNode", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.ClassDeclarationNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ClassDeclarationNode", HasChildren = true, Children = new List<ParseNode>() };
            _rootNodes.Add(node);
            var nodetrue = new ParseNode() { Name = "Children", Value = "Count = 2", Type = "System.Collections.Generic.IEnumerable`true[Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNode]", HasChildren = true, Children = new List<ParseNode>() };
            node.Children.Add(nodetrue);

            node.Children.Add(new ParseNode() { Name = "Methods", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.AttributeSectionNode]", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Identifier", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Bases", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.TypeBaseNode]", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "TypeParameterList", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.TypeBaseNode]", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "TypeParameterConstraintsClauses", Value = "Count = false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNodeList`true[Microsoft.RestrictedUsage.CSharp.Syntax.TypeParameterConstraintsClauseNode]", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "MemberDeclarations", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.MethodDeclarationNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.MemberDeclarationNode", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "StartToken", Value = "3false", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "OpenToken", Value = "32", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "CloseToken", Value = "44", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "CloseAngleToken", Value = "-true", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "ParseTree", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.SourceFileParseTree", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTree", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Kind", Value = "ClassDeclaration", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.NodeKind", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Parent", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.NamespaceDeclarationNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.ParseTreeNode", HasChildren = true, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Token", Value = "3false", Type = "System.Int32", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Flags", Value = "false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.NodeFlags", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "OtherFlags", Value = "false", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.NodeFlags", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "Operator", Value = "NONE", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.Operator", HasChildren = false, Children = new List<ParseNode>() });
            node.Children.Add(new ParseNode() { Name = "TokenExtent", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.TokenExtent", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.TokenExtent", HasChildren = true, Children = new List<ParseNode>() });


            nodetrue.Children.Add(new ParseNode() { Name = "IdentifierNode", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.IdentifierNode", HasChildren = false, Children = new List<ParseNode>() });
            nodetrue.Children.Add(new ParseNode() { Name = "MethodDeclarationNode", Value = "Microsoft.RestrictedUsage.CSharp.Syntax.MethodDeclarationNode", Type = "Microsoft.RestrictedUsage.CSharp.Syntax.MethodDeclarationNode", HasChildren = false, Children = new List<ParseNode>() });

        }
    }

    public class ParseNode
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public bool HasChildren { get; set; }
        public List<ParseNode> Children { get; set; }

    }

}
