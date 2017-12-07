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

        public Node GetNode(string password,string key)
        {
            Node node = null;
            foreach(NodeChain chain in _nodeList)
            {
                node = chain.GetNode(password, key);
                if (node != null)
                {
                    return node;
                }
            }
            return node;
        }

        public int MaxLength
        {
            get
            {
                int max = 0;
                foreach(NodeChain chain in _nodeList)
                {
                    if(chain.Length > max)
                    {
                        max = chain.Length;
                    }
                }
                return max;
            }
        }

        internal Node GetNodeById(long id)
        {
            Node node = null;
            foreach(NodeChain chain in this._nodeList)
            {
                node = chain.GetNodeById(id);
                if(node != null)
                {
                    return node;
                }
            }
            return node;
        }

        internal Node RemoveNode(long nodeId)
        {
            Node node = null;
            foreach (NodeChain chain in this._nodeList)
            {
                node = chain.RemoveNode(nodeId);
                if (node != null)
                {
                    return node;
                }
            }
            return node;
        }

        /// <summary>
        /// merges a node set together with this one.
        /// </summary>
        /// <param name="nodeSet"></param>
        public void Merge(NodeSet nodeSet)
        {
            //update the sets
            if(this.MaxLength < nodeSet.MaxLength)
            {
                this._setId = nodeSet._setId;
            }
            foreach(NodeChain chain in this._nodeList)
            {
                chain.ChangeSetId(this._setId);
            }
            foreach (NodeChain chain in nodeSet._nodeList)
            {
                chain.ChangeSetId(this._setId);
                this._nodeList.Add(chain);
            }
        }
    }
}
