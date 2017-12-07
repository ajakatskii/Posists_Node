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
        private long _nodeId = -1;
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

        private CipherBox _cipher;

        public Node(long nodeId,string name, string address, string mobile, string phone, float value,string password)
        {
            this._nodeId = nodeId;
            this._timestamp = DateTime.Now;
            //encrypt all this data 
            this.getEncryptData(name, address, mobile, phone, value,password);
        }

        private void getEncryptData(string name, string address, string mobile, string phone, float value,string password)
        {
            Encrypter encrypt = new Encrypter(password);
            encrypt.AddDataPair("name", name);
            encrypt.AddDataPair("address", address);
            encrypt.AddDataPair("mobile", mobile);
            encrypt.AddDataPair("phone", phone);
            encrypt.AddDataPair("value", value.ToString());
            //populate cipher and encrypted dat ato this node.
            this._cipher = encrypt.Cipher;
            this._data = encrypt.EncryptedData;
        }

        public CipherBox Cipher
        {
            get
            {
                return this._cipher;
            }
        }

        public int NodeNumber
        {
            get
            {
                return this._nodeSetNumber;
            }
            set
            {
                this._nodeSetNumber = value;
            }
        }

    }
}
