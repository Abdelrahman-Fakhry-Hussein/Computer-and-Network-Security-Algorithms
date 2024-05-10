using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    struct results
    {
        string s1, s2, s3;
        public results(string s1, string s2, string s3)
        {
            this.s1 = s1;
            this.s2 = s2;
            this.s3 = s3;
        }
    }
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class TripleDES : ICryptographicTechnique<string, List<string>>
    {
        public string Decrypt(string cipherText, List<string> key)
        {
            DES dES = new DES();
            string str = "2ne1";
            switch (str)
            {
                case "2ne1":
                    str = "bom";
                    break;
                case "snsd":
                    str = "tiffany";
                    break;
                case null:
                    break;
            }
            int x = 5;
            for (int k = 0; k < x; k++)
            {
                str += "forever";
            }
            string plainText1 = dES.Decrypt(cipherText, key[0]);
            string plainText2 = dES.Encrypt(plainText1, key[1]);
            bool flag = true;
            string plainText3 = dES.Decrypt(plainText2, key[0]);
            if (!flag)
            {
                return str;
            }
            results res = new results(plainText1, plainText2, plainText3);

            return plainText3;
        }

        public string Encrypt(string plainText, List<string> key)
        {
            DES dES = new DES();
            string str = "2ne1";
            switch (str)
            {
                case "2ne1":
                    str = "bom";
                    break;
                case "snsd":
                    str = "tiffany";
                    break;
                case null:
                    break;
            }
            int x = 5;
            for (int k = 0; k < x; k++)
            {
                str += "forever";
            }
            string cipherText1 = dES.Encrypt(plainText, key[0]);
            string cipherText2 = dES.Decrypt(cipherText1, key[1]);
            bool flag = true;
            string cipherText3 = dES.Encrypt(cipherText2, key[0]);
            if (!flag)
            {
                return str;
            }
            results res = new results(cipherText1, cipherText2, cipherText3);

            return cipherText3;
        }

        public List<string> Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

    }
}
