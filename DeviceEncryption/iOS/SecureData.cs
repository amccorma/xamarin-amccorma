using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Security;
using System.Security.Cryptography;
using System.IO;

namespace DeviceEncryption.IOS
{
    public class SecureData : ISecure
    {
        private static string cryptoKey = "j7gdft5'(eqA84Mo";
        private const char FillCharacter = '_';
        private const int KeyLength = 16;

        public SecureData()
        {

        }

        private object[] RSA
        {
            get
            {
                return null;
            }
        }

        public Encrypt Encode(EncryptType type, string value)
        {
            try
            {
                string finalKey = cryptoKey.PadRight(KeyLength, FillCharacter);

                var result = new Encrypt();
                result.Type = EncryptType.OK;
                RijndaelManaged crypto = null;
                MemoryStream mStream = null;
                ICryptoTransform encryptor = null;
                CryptoStream cryptoStream = null;

                byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(value);

                try
                {
                    crypto = new RijndaelManaged();
                    crypto.KeySize = 128;
                    crypto.Padding = PaddingMode.PKCS7;
                    // HERE IS THE "ISSUE"
                    // The default mode on .NET 4 is CBC, wich seems not to be the case on MonoTouch.
                    crypto.Mode = CipherMode.CBC;
                    encryptor = crypto.CreateEncryptor(Encoding.UTF8.GetBytes(finalKey), Encoding.UTF8.GetBytes(finalKey));

                    mStream = new MemoryStream();
                    cryptoStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                }
                finally
                {
                    if (crypto != null)
                        crypto.Clear();

                    cryptoStream.Close();
                }

                result.Value = mStream.ToArray();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Decode(Encrypt obj)
        {
            string finalKey = cryptoKey.PadRight(KeyLength, FillCharacter);

            RijndaelManaged crypto = null;
            MemoryStream mStream = null;
            ICryptoTransform decryptor = null;
            CryptoStream cryptoStream = null;

            try
            {
                byte[] cipheredData = obj.Value;

                crypto = new RijndaelManaged();
                crypto.KeySize = 128;
                crypto.Padding = PaddingMode.PKCS7;

                decryptor = crypto.CreateDecryptor(Encoding.UTF8.GetBytes(finalKey), Encoding.UTF8.GetBytes(finalKey));

                mStream = new System.IO.MemoryStream(cipheredData);
                cryptoStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read);
                StreamReader creader = new StreamReader(cryptoStream, Encoding.UTF8);

                String data = creader.ReadToEnd();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (crypto != null)
                {
                    crypto.Clear();
                }

                if (cryptoStream != null)
                {
                    cryptoStream.Close();
                }
            }
        }
        //--
        public bool Exists(string key)
        {
            SecRecord record = null;
            try
            {
                return Exists(key, out record);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (record != null)
                {
                    record.Dispose();
                }
            }
        }

        public bool Exists(string key, out SecRecord record)
        {
            var rec = new SecRecord(SecKind.GenericPassword)
            {
                Generic = NSData.FromString(key)
            };

            SecStatusCode res;
            record = SecKeyChain.QueryAsRecord(rec, out res);
            if (res == SecStatusCode.Success)
                return true;

            return false;
        }

        public string Get(string key)
        {
            SecRecord record;
            if (Exists(key, out record))
            {
                return record.ValueData.ToString();
            }
            return String.Empty;
        }

        public bool Delete(string key)
        {
            SecRecord record;
            if (Exists(key, out record))
            {
                SecKeyChain.Remove(record);
                return true;
            }
            return false;
        }

        public bool Save(string key, string value)
        {
            Delete(key);
            try
            {
                var s = new SecRecord(SecKind.GenericPassword)
                {
                    Accessible = SecAccessible.WhenUnlockedThisDeviceOnly,
                    ValueData = NSData.FromString(value),
                    Generic = NSData.FromString(key)
                };

                var err = SecKeyChain.Add(s);

                if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Clear()
        {
            return true;
        }
    }
}