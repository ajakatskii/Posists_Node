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

        private int _length = 1;

        public NodeChain(Node node)
        {
            this._parentNode = node;
        }

        public Node GetNode(string password, string key)
        {
            //in a chain atleast, the top password must be same
            if(_parentNode == null)
            {
                return null;
            }
            if(_parentNode.Cipher.Password != password)
            {
                return null;
            }
            //when password matches, iterate the entire chain to check and return the node.
            Node traverser = this._parentNode;
            while(traverser != null)
            {
                if(traverser.Cipher.Key == key)
                {
                    return traverser;
                }
                else
                {
                    traverser = traverser.ChildNode;
                }
            }
            return null;
        }

        public void AddNode(Node node)
        {
            node.ParentNode = this._parentNode;
            //add this node at then end of the chain
            Node tranverser = this._parentNode;
            while(tranverser.ChildNode != null)
            {
                tranverser = tranverser.ChildNode;
            }
            tranverser.ChildNode = node;
            this._length++;
        }

        public int Length
        {
            get
            {
                return this._length;
            }
        }
    }
}
