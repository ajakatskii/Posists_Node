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
    public class Node
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

        public Node ChildNode
        {
            get
            {
                return _childNode;
            }
            set
            {
                _childNode = value;
                if(value != null)
                {
                    value._nodeSetNumber = this._nodeSetNumber;
                    value.ParentNode = this;
                    this.childNodeId = value.Id;
                }
            }
        }

        public long Id
        {
            get
            {
                return this._nodeId;
            }
        }

        public Node ParentNode
        {
            get
            {
                return this._parentNode;
            }
            set
            {
                this._parentNode = value;
            }
        }

        private long childNodeId = -1;

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

        /// <summary>
        /// when switching owner of the nodes, the new owner's password is supposed to unlock this node.
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string newPassword)
        {
            Decrypter decrypter = null;
            Encrypter encrypter = null;
            try
            {
                //decrypt the data, then create a new encrypter, with the new password
                decrypter = new Decrypter(this.Cipher, this._data);
                encrypter = new Encrypter(newPassword);
                foreach (KeyValuePair<String, String> kv in decrypter.Data)
                {
                    encrypter.AddDataPair(kv.Key, kv.Value);
                }
            }catch(Exception e)
            {
                //TODO - LOG
                return false;
            }
            this._cipher = encrypter.Cipher;
            this._data = encrypter.EncryptedData;
            return true;
        }

        private bool getNewEncrypter(Decrypter decrypter,bool modifyCipher = true)
        {
            Encrypter encrypter = null;
            try
            {
                encrypter = new Encrypter(this.Cipher.Password,"split");
                foreach (KeyValuePair<String, String> kv in decrypter.Data)
                {
                    encrypter.AddDataPair(kv.Key, kv.Value);
                }
            }
            catch (Exception e)
            {
                //TODO - LOG
                return false;
            }
            //only change the password and the key if requested, otherwise let the old key go on.
            if(modifyCipher)
            {
                this._cipher = encrypter.Cipher;
            }
            this._data = encrypter.EncryptedData;
            return true;
        }

        public int ChainLength
        {
            get
            {
                if(this._childNode == null)
                {
                    return 1;
                }
                int count = 0;
                Node tranverser = this;
                while (tranverser != null)
                {
                    count++;
                    tranverser = tranverser.ChildNode;
                }
                return count;
            }
        }

        public void BreakUpNode(float valueInThisNode)
        {
            //decrypt the data, then create a new encrypter, with the new password
            Decrypter decrypter = new Decrypter(this.Cipher, this._data);
            float storedValue = float.Parse(decrypter.Data["value"]);
            if(storedValue <= valueInThisNode)
            {
                throw new ArgumentException("Enter a value that is below the value of this node");
            }
            Node subNode = new Node(NodeManager.UniqueId,decrypter.Data["name"], decrypter.Data["address"], decrypter.Data["mobile"], decrypter.Data["phone"],
                                    storedValue - valueInThisNode,this.Cipher.Password);
            decrypter.Data["value"] = valueInThisNode.ToString();
            this.getNewEncrypter(decrypter);
            //add this node in between the chain
            Node child = this.ChildNode;
            this.ChildNode = subNode;
            subNode.ChildNode = child;
        }

        public Dictionary<String,String> GetData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            Decrypter decrypter = new Decrypter(this.Cipher, this._data);
            foreach (KeyValuePair<String, String> kv in decrypter.Data)
            {
                data.Add(kv.Key, kv.Value);
            }
            return data;
        }

    }
}
