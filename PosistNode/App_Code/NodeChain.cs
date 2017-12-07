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
            Node traverser = this._parentNode;
            while(traverser.ChildNode != null)
            {
                traverser = traverser.ChildNode;
            }
            traverser.ChildNode = node;
            this._length++;
        }

        internal Node GetNodeById(long id)
        {
            Node traverser = this._parentNode;
            while (traverser.ChildNode != null)
            {
                if(traverser.Id == id)
                {
                    return traverser;
                }
                traverser = traverser.ChildNode;
            }
            return null;
        }

        public Node RemoveNode(long nodeId)
        {
            Node traverser = this._parentNode;
            if(this._parentNode.Id == nodeId)
            {
                this._parentNode = this._parentNode.ChildNode;
                return traverser;
            }
            Node previous = null;
            while (traverser.ChildNode != null)
            {
                if (traverser.Id == nodeId)
                {
                    previous.ChildNode = traverser.ChildNode;
                    traverser.ChildNode = null;
                    return traverser;
                }
                previous = traverser;
                traverser = traverser.ChildNode;
            }
            return null;
        }

        /// <summary>
        /// changed set Id of all the nodes attached with this chain.
        /// </summary>
        /// <param name="setId"></param>
        internal void ChangeSetId(int setId)
        {
            Node traverser = this._parentNode;
            while (traverser != null)
            {
                traverser.NodeNumber = setId;
                traverser = traverser.ChildNode;
            }
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
