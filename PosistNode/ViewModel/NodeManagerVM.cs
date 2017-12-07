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

        NodeManagerVM()
        {
            _nodeManager = new NodeManager();
        }

        public bool createNew(string name, string address, string mobile, string phone,float value,int set = -1)
        {

            return true;
        }
    }
}
