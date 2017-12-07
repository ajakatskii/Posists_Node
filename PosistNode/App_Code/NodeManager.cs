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

        private long _currentUniqueId = 0;

        /// <summary>
        /// contains the set key to which new nodes without any set get's added
        /// </summary>
        private int _noSetKey = -1;

        public long UniqueId
        {
            get
            {
                return this._currentUniqueId++;
            }
        }

        public NodeManager()
        {

        }

        public bool createNewNode(string name, string address, string mobile, string phone, float value,string password, int set = -1)
        {
            //create a node to insert
            Node node = new Node(this.UniqueId, name, address, mobile, phone, value, password);
            this.addNodeToSet(this.getSetIndex(set), node);
            return true;
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

        public Node GetNodeById(long id)
        {
            Node node = null;
            foreach(NodeSet set in this._sets.Values)
            {
                
            }
            return node;
        }
    }
}
