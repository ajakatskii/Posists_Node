using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosistNode.App_Code
{
    /// <summary>
    /// this class has logic to deal with nodesets, nodechains and nodes.
    /// </summary>
    public class NodeManager
    {
        private Dictionary<int, NodeSet> _sets = new Dictionary<int, NodeSet>();

        private static long _currentUniqueId = 0;

        /// <summary>
        /// contains the set key to which new nodes without any set get's added
        /// </summary>
        private int _noSetKey = -1;

        public static long UniqueId
        {
            get
            {
                return _currentUniqueId++;
            }
        }

        public NodeManager()
        {

        }

        public Node createNewNode(string name, string address, string mobile, string phone, float value,string password, int set = -1)
        {
            //create a node to insert
            Node node = new Node(UniqueId, name, address, mobile, phone, value, password);
            this.addNodeToSet(this.getSetIndex(set), node);
            return node;
        }

        private int getSetIndex(int set)
        {
            //find the set to which this node will be pushed.
            if (set == -1)
            {
                //check if any set exists, if no set exists then create one with 1 set number
                if (this._noSetKey == -1)
                {
                    this._noSetKey = 1;
                }
                set = this._noSetKey;
            }
            if (this._noSetKey == -1)
            {
                //no default set assigned, so assign this as default set
                this._noSetKey = set;
            }
            return set;
        }

        private void addNodeToSet(int set, Node node)
        {
            if(!_sets.ContainsKey(set))
            {
                _sets.Add(set, new NodeSet(set));
            }
            this._sets[set].AddNode(node);
        }

        /// <summary>
        /// returns the node, for which thee given password and key validates.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Node GetNode(string password, string key)
        {
            Node node = null;
            //search in each set
            foreach(KeyValuePair<int,NodeSet> kv in this._sets)
            {
                node = kv.Value.GetNode(password, key);
                if (node != null)
                {
                    return node;
                }
            }
            return node;
        }

        public int GetMaxLength()
        {
            int max = 0;
            int currentLength = 0;
            foreach(NodeSet set in this._sets.Values)
            {
                currentLength = set.MaxLength;
                if(currentLength > max)
                {
                    max = currentLength;
                }
            }
            return max;
        }

        public Node GetNode(long id)
        {
            Node node = null;
            foreach(NodeSet set in this._sets.Values)
            {
                node = set.GetNodeById(id);
                if(node != null)
                {
                    return node;
                }
            }
            return node;
        }

        /// <summary>
        /// shifts the node with the given id under the node of given parent id
        /// </summary>
        /// <param name="removeNodeId"></param>
        /// <param name="newParentId"></param>
        /// <returns>the shifted node</returns>
        public Node shiftNode(long removeNodeId,long newParentId)
        {
            Node node = this.RemoveNode(removeNodeId);
            if(node == null)
            {
                throw new ArgumentNullException("The Given NodeId has no associated node with it");
            }
            if(node.ParentNode != null && node.ParentNode.Id == newParentId)
            {
                throw new ArgumentNullException("Already has the given parent");
            }
            Node parent = this.GetNode(newParentId);
            if (parent == null)
            {
                throw new ArgumentNullException("The Given Parent NodeId has no associated node with it");
            }
            node.ChangePassword(parent.Cipher.Password);
            Node temp = parent.ChildNode;
            parent.ChildNode = node;
            node.ChildNode = temp;
            return node;
        }

        public Node RemoveNode(long nodeId)
        {
            Node node = null;
            foreach(NodeSet set in this._sets.Values)
            {
                node = set.RemoveNode(nodeId);
                if(node != null)
                {
                    return node;
                }
            }
            return node;
        }

        public void MergeSets(int setId1, int setId2)
        {
            if(!this._sets.ContainsKey(setId1))
            {
                throw new ArgumentNullException("No Set with Set ID 1 present.");
            }
            if (!this._sets.ContainsKey(setId2))
            {
                throw new ArgumentNullException("No Set with Set ID 2 present.");
            }
            this._sets[setId1].Merge(this._sets[setId2]);
            this._sets.Remove(setId2);
        }
    }
}
