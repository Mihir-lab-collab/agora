using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Agora.Onboarding.Encryption
{
    public class Utility
    {
        static byte[] salt = new byte[2];
        private const string password = "sKnc46B3$D68a#4e8F@aB7v^2cQd3cEb47b9";
        public static string Encrypt(string clearText)
        {

            //string password = "12345";
            if (!string.IsNullOrEmpty(clearText))
            {
                byte[] _clearBytes = Encoding.Unicode.GetBytes(clearText);
                PasswordDeriveBytes _pdb = new PasswordDeriveBytes(password, salt);
                MemoryStream _ms = new MemoryStream();
                Rijndael _alg = Rijndael.Create();
                _alg.Key = _pdb.GetBytes(32);
                _alg.IV = _pdb.GetBytes(16);
                CryptoStream cs = new CryptoStream(_ms, _alg.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(_clearBytes, 0, _clearBytes.Length);
                cs.Close();
                byte[] _EncryptedData = _ms.ToArray();
                return Convert.ToBase64String(_EncryptedData);
            }
            else
            {
                return string.Empty;
            }
        }
        public static string Decrypt(string cipherText)
        {
            cipherText.Replace(' ', '+');
            try
            {
                // string password = "";
                if (!string.IsNullOrEmpty(cipherText))
                {
                    byte[] _cipherBytes = Convert.FromBase64String(cipherText);
                    PasswordDeriveBytes _pdb = new PasswordDeriveBytes(password, salt);
                    MemoryStream _ms = new MemoryStream();
                    Rijndael _alg = Rijndael.Create();
                    _alg.Key = _pdb.GetBytes(32);
                    _alg.IV = _pdb.GetBytes(16);
                    CryptoStream cs = new CryptoStream(_ms, _alg.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(_cipherBytes, 0, _cipherBytes.Length);
                    cs.Close();
                    byte[] _DecryptedData = _ms.ToArray();
                    return Encoding.Unicode.GetString(_DecryptedData);
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}