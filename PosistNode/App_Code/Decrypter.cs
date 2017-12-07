﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PosistNode.App_Code
{
    class Decrypter
    {

        private CipherBox _cipher;

        private string _encryptedData;

        private string _data;

        private bool _success = false;

        public bool Success
        {
            get
            {
                return this._success;
            }
        }

        public Dictionary<String,String> Data
        {
            get
            {
                return this._decryptedData;
            }
        }

        private Dictionary<string, string> _decryptedData;

        public Decrypter(CipherBox cipher,string data)
        {
            this._cipher = cipher;
            this._encryptedData = data;
            _decryptedData = new Dictionary<string, string>();
        }

        private void decryptData()
        {
            Byte[] ivStream = new Byte[32];
            Byte[] plainText = new Byte[this._encryptedData.Length];
            using (RijndaelManaged rm = new RijndaelManaged())
            {
                rm.KeySize = 256;
                rm.IV = Convert.FromBase64String(this._cipher.Key);
                rm.Key = UTF8Encoding.UTF8.GetBytes(this._cipher.Password  + Encrypter.PASSWORD_PADDING).Take<Byte>(32).ToArray();
                using (ICryptoTransform tranform = rm.CreateDecryptor())
                {
                    using (MemoryStream memStream = new MemoryStream(Convert.FromBase64String(this._encryptedData)))
                    {
                        using (CryptoStream stream = new CryptoStream(memStream, tranform, CryptoStreamMode.Read))
                        {
                            int decryptedBytes = stream.Read(plainText, 0, plainText.Length);
                            this._data = UTF8Encoding.UTF8.GetString(plainText, 0, decryptedBytes);
                        }
                    }
                }
            }
            String[] dataPair = this._data.Split(new string[] { Encrypter.DATA_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            if(dataPair.Length == 5)
            {
                this._success = true;
                foreach(String pair in dataPair)
                {
                    String[] kv = pair.Split(new string[] { Encrypter.KEY_VALUE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
                    this._decryptedData.Add(kv[0], kv[1]);
                }
            }
            else
            {
                this._success = false;
            }
        }
    }
}
