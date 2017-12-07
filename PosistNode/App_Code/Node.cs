using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosistNode.App_Code
{
    /// <summary>
    /// represents a client node
    /// </summary>
    class Node
    {
        /// <summary>
        /// timestamp when the node was created
        /// </summary>
        private DateTime _timestamp;
        /// <summary>
        /// unique id for the current node, each node in nodechain has a unique id
        /// </summary>
        private int _nodeId = -1;
        /// <summary>
        /// the set number to which this node belongs
        /// </summary>
        private int _nodeSetNumber = -1;
        /// <summary>
        /// encrypted data
        /// </summary>
        private String _data;

        private Node _parentNode = null;

        private Node _childNode = null;

        private int childNodeId = -1;

    }
}
