using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = "";
            int p, c;
            List<char> alpa = new List<char>();
            for (char i = 'a'; i <= 'z'; i++)
                alpa.Add(i);
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for (int i = 0; i < plainText.Length; i++)
            {
                p = plainText[i] - 'a';
                c = cipherText[i] - 'a';
                key += alpa[((c + 26) - p) % 26];
            }
            for (int i = 1; i < key.Length; i++)
            {
                if (Encrypt(plainText, key.Substring(0, i)).Equals(cipherText))
                {
                    return key.Substring(0, i);
                }
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "", newkey = "";
            int first = 0, second;
            List<char> alpa = new List<char>();
            int keylen = cipherText.Length / key.Length;
            int keymod = cipherText.Length % key.Length;
            for (int i = 0; i < keylen; i++)
                newkey += key;
            for (int i = 0; i < keymod; i++)
                newkey += key[i];
            for (char i = 'a'; i <= 'z'; i++)
                alpa.Add(i);
            cipherText = cipherText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                first = cipherText[i] - 'a';
                second = newkey[i] - 'a';
                plainText += alpa[(first - second + 26) % 26];
            }
            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            string cipherText = "", newkey = "";
            int first = 0, second;
            List<char> alpa = new List<char>();
            int keylen = plainText.Length / key.Length;
            int keymod = plainText.Length % key.Length;
            for (int i = 0; i < keylen; i++)
                newkey += key;
            for (int i = 0; i < keymod; i++)
                newkey += key[i];
            for (char i = 'a'; i <= 'z'; i++)
                alpa.Add(i);

            for (int i = 0; i < plainText.Length; i++)
            {
                first = plainText[i] - 'a';
                second = newkey[i] - 'a';
                cipherText += alpa[(first + second) % 26];
            }
            return cipherText;
        }
    }
}