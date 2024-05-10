using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private const string AlphabeticChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private void Validation(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length != 26) throw new ArgumentException("Key must be exactly 26 characters long.", nameof(key));
            if (key.Distinct().Count() != 26 || key.Any(c => !AlphabeticChars.Contains(char.ToUpper(c))))
                throw new ArgumentException("Key must contain every letter of the alphabet exactly once.", nameof(key));
            //throw new ArgumentException("Key must contain every letter of the alphabet exactly once.", nameof(key));
        }

        private Dictionary<char, char> Map(string key, bool forDecryption = false)
        {
            //1
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (key.Length != AlphabeticChars.Length)
                throw new ArgumentException($"Key must be exactly {AlphabeticChars.Length} characters long.", nameof(key));
            //2
            var uniqueKeyChars = new HashSet<char>();
            foreach (var ch in key.ToUpper())
            {
                if (!AlphabeticChars.Contains(ch))
                    throw new ArgumentException("Key must contain only letters from the alphabet.", nameof(key));
                if (!uniqueKeyChars.Add(ch))
                    throw new ArgumentException("Key must contain unique characters.", nameof(key));
            }
            //3
            var mapping = new Dictionary<char, char>();
            var upperKey = key.ToUpper();

            if (forDecryption)
            {
                //decryption
                for (int i = 0; i < AlphabeticChars.Length; i++)
                {
                    mapping[upperKey[i]] = AlphabeticChars[i];
                }
            }
            else
            {
                //encryption
                for (int i = 0; i < AlphabeticChars.Length; i++)
                {
                    mapping[AlphabeticChars[i]] = upperKey[i];
                }
            }
            return mapping;
        }


        public string Decrypt(string cipherText, string key)
        {
            //check ket length
            Validation(key);
            var mapping = Map(key, true);
            return TransformText(cipherText, mapping, true);
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            //check key length
            Validation(key);
            var mapping = Map(key);
            return TransformText(plainText, mapping, false);
            //throw new NotImplementedException();
        }

        private string TransformText(string inputText, Dictionary<char, char> mapping, bool isDecrypting)
        {
            //1
            StringBuilder result = new StringBuilder(inputText.Length);
            foreach (char c in inputText)
            {
                char upperChar = char.ToUpper(c);
                if (mapping.ContainsKey(upperChar))
                {
                    var transformedChar = mapping[upperChar];
                    result.Append(char.IsLower(c) ? char.ToLower(transformedChar) : transformedChar);
                }
                else if (isDecrypting || char.IsLetter(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        public string Analyse(string plainText, string cipherText)
        {
            //1
            Dictionary<Char, Char> key = new Dictionary<char, char>();
            Char Alphabatic_Counter = 'a';
            for (int i = 0; i < 26; i++)
            {
                key.Add(Alphabatic_Counter, '.');
                Alphabatic_Counter++;
            }
            //2
            //save
            String latters = "abcdefghijklmnopqrstuvwxyz";
            int j = 0;

            foreach (Char c in plainText.ToLower())
            {
                key[c] = cipherText.ToLower()[j];
                j++;
            }

            foreach (Char c in latters)
            {
                if (!key.ContainsValue(c))
                {
                    foreach (var kvp in key)
                    {
                        if (kvp.Value == '.')
                        {
                            key[kvp.Key] = c;
                            break;
                        }
                    }

                }
            }
            String RetKey = "";
            foreach (var kvp in key)
            {
                RetKey += kvp.Value;
            }
            return RetKey;
            //throw new NotImplementedException();
        }






        public string AnalyseUsingCharFrequency(string cipher)
        {
            //1
            string FreqOrder = "etaoinsrhldcumfpgwybvkxjqz";

            var cipherFreq = cipher.ToLower()
                .Where(char.IsLetter)
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            var sortedCipherChars = cipherFreq.OrderByDescending(pair => pair.Value)
                .Select(pair => pair.Key)
                .ToList();

            //2

            Dictionary<char, char> charMap = sortedCipherChars
                .Select((c, i) => new { CipherChar = c, PlainChar = FreqOrder[i] })
                .ToDictionary(x => x.CipherChar, x => x.PlainChar);

            string plainText = new string(cipher
                .ToLower()
                .Select(c => charMap.ContainsKey(c) ? charMap[c] : c)
                .ToArray());

            return plainText;
            //throw new NotImplementedException();
        }
    }
}
