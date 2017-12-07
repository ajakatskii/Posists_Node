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
    class NodeManager
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

        NodeManager()
        {

        }

        bool createNewNode(string name, string address, string mobile, string phone, float value,string password, int set = -1)
        {
            //create a node to insert
            Node node = new Node(this.UniqueId, name, address, mobile, phone, value, password);
            this.addnodeToSet(this.getSetIndex(set), node);
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

        private void addnodeToSet(int set, Node node)
        {
            if(!_sets.ContainsKey(set))
            {
                _sets.Add(set, new NodeSet(set));
            }
            this._sets[set].AddNode(node);
        }
    }
}
