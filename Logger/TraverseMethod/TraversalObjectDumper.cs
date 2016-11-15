using Logger.Fomatters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Logger
{
    public class TraversalObjectDumper : IObjectDumper
    {
        private const string OBJECT_END = "1Vy44WGLdvqBAFCDtamiwjCKQP8uLGs3DwSdbr4NCIzj9nyFPuxzQaMyO2rh";
        private const string OBJECT_START = "dRYdHvtvJ1yb0imjQ21uJ2nWqLoHjokhXl2kjuhEix6GYRdN0lmZekY6Mc5l";
        private InternalObjectFormatter _formatter;

        public ObjectDumperSettings Settings { get; set; }

        public TraversalObjectDumper()
        {
            _formatter = new InternalObjectFormatter();
            Settings = new ObjectDumperSettings();
        }

        public void Dump(object obj, TextWriter tw)
        {
            Stack<TraverseNode> nodes = new Stack<TraverseNode>();
            nodes.Push(new TraverseNode(string.Empty, string.Empty, obj, null, 0));

            while (nodes.Count > 0)
            {
                TraverseNode node = nodes.Pop();
                if (node.Level >= Settings.MaxDepth)
                {
                    _formatter.WriteName(node.GetNames(), node.Level, tw, Settings.WriteElementType);
                    _formatter.WriteText(string.Format("{0}{1}{2}{3}", "{", node.ItemType, "}", Environment.NewLine), 0, tw);
                }
                else
                {
                    IDumpFormatter formatter = GetFormatter(node);
                    bool processed = true;
                    if (formatter != null)
                    {
                        _formatter.WriteName(node.GetNames(), node.Level, tw, Settings.WriteElementType);
                        tw.Write(formatter.Format(node.Item));
                    }
                    else
                    {
                        PrepareChildren(nodes, node, node.GetChildren(Settings));
                        processed = ProcessNode(nodes, node, tw);
                    }

                    if (processed && nodes.Count > 0)
                        _formatter.NewLine(tw);
                }
            }
        }

        private IDumpFormatter GetFormatter(TraverseNode node)
        {
            IDumpFormatter formatter = null;
            if (node.Item != null)
                formatter = Settings.GetFormatter(node.Item.GetType());
            return formatter;
        }

        private bool ProcessNode(Stack<TraverseNode> nodes, TraverseNode node, TextWriter tw)
        {
            bool result = true;
            if (node.Name == OBJECT_END)
                _formatter.WriteText("}", node.Level, tw);
            else if (node.Name == OBJECT_START)
                _formatter.WriteText("{", node.Level, tw);
            else
                result = Write(nodes, node, tw);

            return result;
        }

        private void PrepareChildren(Stack<TraverseNode> nodes, TraverseNode node, IEnumerable<TraverseNode> children)
        {
            if (children.Any())
                nodes.Push(new TraverseNode(OBJECT_END, OBJECT_END, OBJECT_END, node, node.Level));

            foreach (TraverseNode child in children)
                nodes.Push(child);

            if (children.Any())
                nodes.Push(new TraverseNode(OBJECT_START, OBJECT_START, OBJECT_START, node, node.Level));
        }

        private bool Write(Stack<TraverseNode> nodes, TraverseNode node, TextWriter tw)
        {
            bool result = false;
            bool isNamed = false;
            if (!string.IsNullOrWhiteSpace(node.Name))
            {
                isNamed = true;
                _formatter.WriteName(node.GetNames(), node.Level, tw, Settings.WriteElementType);
                result = true;
            }

            object obj = node.Item;

            if (obj == null || obj is ValueType || obj is string)
            {
                _formatter.WriteValue(obj, tw, isNamed);
                result = true;
            }
            else if (obj is IEnumerable)
            {
                IList<object> collection = (obj as IEnumerable).Cast<object>().ToList();

                List<TraverseNode> children = new List<TraverseNode>();
                for (int i = 0; i < collection.Count(); i++)
                {
                    object item = collection[i];
                    children.Add(new TraverseNode($"[{i}]", $"{item?.GetType().FullName}[]", item, node, node.Level + 1));
                }

                PrepareChildren(nodes, node, children.Reverse<TraverseNode>());
            }

            return result;
        }
    }
}
