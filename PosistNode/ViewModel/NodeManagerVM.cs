using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosistNode.App_Code;
namespace PosistNode.ViewModel
{
    class NodeManagerVM
    {
        private NodeManager _nodeManager;

        private Constants.Mode _mode;

        private string _lastError;
        public string LastError
        {
            get
            {
                String error = this._lastError;
                this._lastError = "";
                return error;
            }
            set
            {
                this._lastError = value;
            }
        }

        public Constants.Mode Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        public Node DisplayNode;

        public NodeManagerVM()
        {
            _nodeManager = new NodeManager();
        }

        public long CreateNew(string name, string address, string mobile, string phone,float value,string password,int set = -1)
        {
            this.DisplayNode = _nodeManager.createNewNode(name, address, mobile, phone, value, password, set);
            return this.DisplayNode.Id;
        }

        public bool VerifyNode(string password,string key)
        {
            if(this.DisplayNode != null)
            {
                //verify for the current node!
                return (this.DisplayNode.Cipher.Password == password && this.DisplayNode.Cipher.Key == key);
            }
            else
            {
                this.DisplayNode = this._nodeManager.GetNode(password, key);
                return (this.DisplayNode != null);
            }
        }

        public bool FindNode(long nodeId)
        {
            this.DisplayNode = this._nodeManager.GetNode(nodeId);
            return (this.DisplayNode != null);
        }

        public bool MergeSets(int setId1,int setId2)
        {
            try
            {
                this._nodeManager.MergeSets(setId1, setId2);
            }catch(ArgumentNullException e)
            {
                this.LastError = e.Message;
                return false;
            }
            return true;
        }

        public bool Transfer(long transferNodeId, long transferParentNodeId,string password,string key)
        {
            //get the node that belongs to the given verification and check if it is the right node
            Node prev = this.DisplayNode;
            if(this.VerifyNode(password,key))
            {
                if(this.DisplayNode.Id != transferNodeId)
                {
                    this.DisplayNode = prev;
                    this._lastError = "Invalid Credentials";
                    return false;
                }
                this.DisplayNode = prev;
            }else{
                this._lastError = "Invalid Credentials";
                return false;
            }
            try
            {
                this._nodeManager.shiftNode(transferNodeId, transferParentNodeId);
            }catch(ArgumentNullException e)
            {
                this._lastError = e.Message;
                return false;
            }
            return true;
        }

        public int MaxChainLength()
        {
            return this._nodeManager.GetMaxLength();
        }
    }
}
