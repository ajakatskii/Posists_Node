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

        public NodeManagerVM()
        {
            _nodeManager = new NodeManager();
        }

        public bool createNew(string name, string address, string mobile, string phone,float value,string password,int set = -1)
        {
            _nodeManager.createNewNode(name, address, mobile, phone, value, password, set);
            return true;
        }
    }
}
