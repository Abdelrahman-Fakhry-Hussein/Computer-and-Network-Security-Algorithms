using System;
using System.Linq;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        private readonly string englishChars = "abcdefghijklmnopqrstuvwxyz";

        public string Analyse(string plainText, string cipherText)
        {
            string key = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                char cipherChar = char.ToLower(cipherText[i]);
                char plainChar = char.ToLower(plainText[i]);
                int cipherIndex = englishChars.IndexOf(cipherChar);
                int plainIndex = englishChars.IndexOf(plainChar);

                int diff = (cipherIndex - plainIndex + 26) % 26;
                key += englishChars[diff];
            }

            string result = "";
            for (int i = 0; i < key.Length; i++)
            {
                string substring = key.Substring(i);
                if (plainText.ToLower().Contains(substring))
                {
                    break;
                }
                else
                {
                    result += key[i];
                }
            }

            return result;
        }

        public string Decrypt(string cipherText, string key)
        {
            string plainText = "";
            int keyIndex = 0;

            foreach (char cipherChar in cipherText)
            {
                char keyChar = char.ToLower(key[keyIndex]);

                int cipherIndex = englishChars.IndexOf(char.ToLower(cipherChar));
                int keyIndexAlpha = englishChars.IndexOf(keyChar);

                int result = (cipherIndex - keyIndexAlpha + 26) % 26;
                plainText += englishChars[result];

                key += englishChars[result];
                keyIndex = (keyIndex + 1) % key.Length;
            }

            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            string extendedKey = key + plainText.Substring(0, plainText.Length - key.Length);
            string cipherText = "";

            int index = 0;
            foreach (char plainChar in plainText)
            {
                char keyChar = char.ToLower(extendedKey[index]);

                int plainIndex = englishChars.IndexOf(char.ToLower(plainChar));
                int keyIndex = englishChars.IndexOf(keyChar);

                int result = (plainIndex + keyIndex) % 26;
                cipherText += englishChars[result];

                index++;
            }
            return cipherText;
        }
    }
}