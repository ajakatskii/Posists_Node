using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Security.Cryptography;

namespace PosistNode.App_Code
{
    class Encrypter
    {
        public const string KEY_VALUE_SEPARATOR = "!:!";
        public const string DATA_SEPARATOR = "!;!";
        public const string PASSWORD_PADDING = "akj3498u3bdi3h3d3--3d-39hihoaskjdhakjsd3";

        private Dictionary<string, string> _data;

        /// <summary>
        /// the user entered password, to secure the data
        /// </summary>
        private string _password;

        private string _dataDigest;

        private string _digestMD5;

        private string _algoKey = "";

        private string _encryptedData = "";

        private CipherBox _cipher;

        public Encrypter(string password)
        {
            this._password = password;
            this._data = new Dictionary<string, string>();
        }

        public void AddDataPair(string key, string value)
        {
            this._data.Add(key, value);
        }

        public CipherBox Cipher
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._dataDigest))
                {
                    this.encrypt();
                }
                return this._cipher;
            }
        }

        public String EncryptedData
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._dataDigest))
                {
                    this.encrypt();
                }
                return this._encryptedData;
            }
        }

        private void encrypt()
        {
            this.createDataDigest();
            //to get unique algo key each time.
            Byte[] digestBytes = UTF8Encoding.UTF8.GetBytes(this._dataDigest + DateTime.UtcNow.ToString() + PASSWORD_PADDING);
            MD5CryptoServiceProvider md5Key = new MD5CryptoServiceProvider();
            Byte[] computedMD5 = md5Key.ComputeHash(digestBytes);
            md5Key.Dispose();
            this._algoKey = Convert.ToBase64String(computedMD5);
            Byte[] computedSalt = UTF8Encoding.UTF8.GetBytes(this._password + PASSWORD_PADDING).Take<byte>(32).ToArray();

            //now encrypt the text with AES
            using (RijndaelManaged encryptAlgo = new RijndaelManaged())
            {
                encryptAlgo.IV = computedMD5;
                encryptAlgo.Key = computedSalt;
                encryptAlgo.BlockSize = 256;
                using (ICryptoTransform encrypter = encryptAlgo.CreateEncryptor())
                {
                    using (var memStream = new MemoryStream())
                    {
                        using (CryptoStream stream = new CryptoStream(memStream, encrypter, CryptoStreamMode.Write))
                        {
                            stream.Write(digestBytes, 0, digestBytes.Length);
                            stream.FlushFinalBlock();
                        }
                        this._encryptedData = Convert.ToBase64String(memStream.ToArray());
                    }
                }
            }

            this._cipher = new CipherBox(this._password, this._algoKey);
        }

        private void createDataDigest()
        {
            if (_data.Keys.Count < 1)
            {
                throw new ArgumentException("Values not added for encryption.");
            }
            foreach (KeyValuePair<string, string> kv in this._data)
            {
                this._dataDigest += kv.Key + KEY_VALUE_SEPARATOR + kv.Value + DATA_SEPARATOR;
            }
        }


    }
}
