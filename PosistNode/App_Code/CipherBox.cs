using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosistNode.App_Code
{
    public class CipherBox
    {
        private string _password;
        private string _key;

        public string Password
        {
            get
            {
                return this._password;
            }
        }

        public string Key
        {
            get
            {
                return this._key;
            }
        }

        public CipherBox(string password, string key)
        {
            this._password = password;
            this._key = key;
        }

    }
}
