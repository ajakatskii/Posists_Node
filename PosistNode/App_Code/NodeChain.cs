using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosistNode.App_Code
{
    /// <summary>
    /// represents a single node chain, wherein the head node generally belongs to the same user
    /// </summary>
    class NodeChain
    {
        private Node _parentNode;

        public NodeChain(Node node)
        {
            this._parentNode = node;
        }

    }
}
