using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;

namespace Bookkmark
{
    public class Encryption
    {
        private byte[] key;
        private byte[] IV = { 0X12, 0X20, 0X38, 0X36, 0X48, 0XAB, 0XCD, 0XEF };

        public string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string EncryptQueryString(string strQueryString)
        {
            Encryption xx = new Encryption();
            if (strQueryString == "")
            {
                return "";
            }
            try
            {
                return xx.Encrypt(strQueryString, "!D#2%vin");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string DecryptQueryString(string strQueryString)
        {
            Encryption xx = new Encryption();
            if (strQueryString == "")
            {
                return "";
            }
            try
            {
                return xx.Decrypt(strQueryString.Replace(" ", "+"), "!D#2%vin");
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }

}