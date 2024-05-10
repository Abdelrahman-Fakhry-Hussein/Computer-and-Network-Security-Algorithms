using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            int z = 0;
            for (int i = 1; i < plainText.Length; i++)
            {
                string de = Encrypt(plainText, i);
                de = de.Replace("\0", "");
                bool areEqualIgnoreCase = de.Equals(cipherText, StringComparison.OrdinalIgnoreCase);
                if (areEqualIgnoreCase)
                {

                    z = i;
                    break;
                }
            }
            return z;
        }

        public string Decrypt(string cipherText, int key)
        {
            int columns = (int)Math.Ceiling((double)cipherText.Length / key);
            char[,] charMatrix = new char[key, columns];
            char s = ' ';
            string outText = "";
            int charIndex = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (charIndex >= cipherText.Length)
                    {
                        break;
                    }

                    charMatrix[i, j] = cipherText[charIndex];
                    charIndex++;
                }
            }
            for (int j = 0; j < charMatrix.GetLength(1); j++)
            {
                for (int i = 0; i < charMatrix.GetLength(0); i++)
                {
                    s = (charMatrix[i, j]);
                    outText += s;
                }
            }
            return outText;
        }
        public string Encrypt(string plainText, int key)
        {
            double ptlengthf = Math.Ceiling((double)plainText.Length / key);
            int ptlength = (int)ptlengthf;
            //List<List<string>> encryptedList = new List<List<string>>();
            //int index = 0;
            string outencrypted = "";
            string outencryptedF = "";
            for (int i = 0; i < ptlength * key; i++)
            {
                if (outencrypted.Length < plainText.Length)
                {
                    outencrypted += plainText[i];

                }
                else
                {
                    outencrypted += ' ';


                }
            }
            for (int i = 0; i < key; i++)
            {
                for (int j = i; j < ptlength * key; j += key)
                {
                    outencryptedF += outencrypted[j];
                }
            }
            outencryptedF = outencryptedF.Replace(" ", "");
            Console.WriteLine(outencryptedF);
            return outencryptedF;
        }
    }
}
