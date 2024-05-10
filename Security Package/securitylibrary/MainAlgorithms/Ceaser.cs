using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        const int alphas = 26;
        public string Encrypt(string plainText, int key)
        {
            //normalizing key
            key = (key % alphas + alphas) % alphas;
            return shiftLetters(plainText, key);
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, int key)
        {
            //normalizing key
            key = (key % alphas + alphas) % alphas;
            return shiftLetters(cipherText, -key);
            //throw new NotImplementedException();
        }

        public int Analyse(string plainText, string cipherText)
        {

            //normalizing
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();

            //Calculating the shift 
            for (int i = 0; i < plainText.Length; i++)
            {
                if (char.IsLetter(plainText[i]) && char.IsLetter(cipherText[i]))
                {
                    int shift = (cipherText[i] - plainText[i] + alphas) % alphas;
                    return shift;
                }
            }

            throw new ArgumentException("Unable to determine the key based on the input provided.");
            //throw new NotImplementedException();
        }
        private string shiftLetters(string input, int key)
        {

            //normalizing key
            key = (key % alphas + alphas) % alphas;
            char[] temp = input.ToCharArray();
            for (int i = 0; i < temp.Length; i++)
            {
                char c = temp[i];
                if (char.IsLetter(c))
                {
                    char off;
                    if (char.IsUpper(c))
                    {
                        off = 'A';

                    }
                    else
                    {
                        off = 'a';
                    }
                    //Shifting char & sure to  wrap around 26
                    int shifted = (c + key - off) % alphas + off;
                    temp[i] = (char)shifted;
                }
            }
            return new string(temp);
        }
    }
}
