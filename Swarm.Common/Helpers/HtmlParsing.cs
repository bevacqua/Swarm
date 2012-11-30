using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Swarm.Common.Helpers
{
    public static class HtmlParsingHelpers
    {
        public static IEnumerable<HtmlNode> SelectNodesOrEmpty(this HtmlNode node, string xpath)
        {
            return node.SelectNodes(xpath) ?? Enumerable.Empty<HtmlNode>();
        }

        public static HtmlNode GetParentNodeUntil(this HtmlNode node, HtmlNode parent)
        {
            while (node.ParentNode != parent)
            {
                node = node.ParentNode;
            }
            return node;
        }

        public static int GetDepth(this HtmlNode node)
        {
            int depth = 0;
            while (node.ParentNode != null)
            {
                depth++;
                node = node.ParentNode;
            }
            return depth;
        }

        public static int GetIndex(this HtmlNode node)
        {
            return node.ParentNode.ChildNodes.IndexOf(node);
        }

        /// <summary>
        /// Find the node that's closest to the source node.
        /// </summary>
        /// <param name="source">The source node.</param>
        /// <param name="sharedParentNode">The parent node in common between the node in question and all the nodes in the list.</param>
        /// <param name="nodes">A list of nodes where we want to determine the closest to the source.</param>
        /// <returns>The node that is closest to the source.</returns>
        public static HtmlNode FindClosestNode(this HtmlNode source, HtmlNode sharedParentNode, IList<HtmlNode> nodes)
        {
            if (nodes.Count < 2) // don't even bother.
            {
                return nodes.FirstOrDefault();
            }
            int index = source.GetParentNodeUntil(sharedParentNode).GetIndex(); // get the index of our node, relative to the shared parent node.

            HtmlNode closest = null;
            int distance = int.MaxValue;

            foreach (HtmlNode currentNode in nodes)
            {
                int currentIndex = currentNode.GetParentNodeUntil(sharedParentNode).GetIndex(); // get the index of this node, relative to the shared parent node.
                int currentDistance;
                if (currentIndex > index)
                {
                    currentDistance = currentIndex - index;
                }
                else
                {
                    currentDistance = index - currentIndex;
                }
                if (currentDistance < distance)
                {
                    closest = currentNode;
                    distance = currentDistance;
                }
            }
            return closest;
        }

        /// <summary>
        /// Gets the inner HTML of a node's child, or an empty string if no node matches the XPath query.
        /// </summary>
        public static string GetChildNodeInnerHtml(this HtmlNode node, string xpath)
        {
            HtmlNode child = node.SelectSingleNode(xpath);
            if (child == null)
            {
                return string.Empty;
            }
            return child.InnerHtml.Trim();
        }

        /// <summary>
        /// Sanitize any potentially dangerous tags from the provided raw HTML input using 
        /// a whitelist based approach, leaving the "safe" HTML tags
        /// </summary>
        public static string SanitizeHtml(this string html)
        {
            if (String.IsNullOrEmpty(html))
            {
                return String.Empty;
            }
            MatchCollection tags = CompiledRegex.HtmlTag.Matches(html); // match every HTML tag in the input
            for (int i = tags.Count - 1; i > -1; i--)
            {
                Match match = tags[i];
                string tag = match.Value.ToLowerInvariant();

                if (!(CompiledRegex.HtmlSafeTag.IsMatch(tag) || CompiledRegex.HtmlSafeAnchorTag.IsMatch(tag) || CompiledRegex.HtmlSafeImageTag.IsMatch(tag)))
                {
                    html = html.Remove(match.Index, match.Length);
                }
            }
            return html.Trim();
        }
    }
}
