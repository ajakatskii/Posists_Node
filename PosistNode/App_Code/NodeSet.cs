using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosistNode.App_Code
{
    /// <summary>
    /// this manages a complete set of nodes
    /// </summary>
    class NodeSet
    {
        private int _setId = -1;

        private List<NodeChain> _nodeList;

        public NodeSet(int setId)
        {
            this._setId = setId;
            this._nodeList = new List<NodeChain>();
        }

        /// <summary>
        /// adds a node to the given set
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(Node node)
        {
            node.NodeNumber = this._setId;
            this._nodeList.Add(new NodeChain(node));
        }
    }
}
