using SecurityLibrary.AES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// CHECK IF STRING.SUBSTRING IS INCLUSIVE OR EXCLUSIVE AND PYTHON'S
namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class DES : CryptographicTechnique
    {

        public static Dictionary<char, string> hxdcmp = new Dictionary<char, string>()
        {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'A', "1010" },
            { 'B', "1011" },
            { 'C', "1100" },
            { 'D', "1101" },
            { 'E', "1110" },
            { 'F', "1111" }
        };

        public static Dictionary<string, char> bnmp = new Dictionary<string, char>()
        {
            { "0000", '0' },
            { "0001", '1' },
            { "0010", '2' },
            { "0011", '3' },
            { "0100", '4' },
            { "0101", '5' },
            { "0110", '6' },
            { "0111", '7' },
            { "1000", '8' },
            { "1001", '9' },
            { "1010", 'A' },
            { "1011", 'B' },
            { "1100", 'C' },
            { "1101", 'D' },
            { "1110", 'E' },
            { "1111", 'F' }
        };

        string HandleAddress(string s, string action)
        {
            switch (action)
            {
                case "Add":
                    s = AddAddress(s);
                    break;
                case "Remove":
                    s = RemoveAddress(s);
                    break;
            }
            return s;
        }
        string AddAddress(string s)
        {
            string result = "";
            result = "0x" + s;
            return result;
        }

        string RemoveAddress(string s)
        {
            string result = "";
            result = s.Substring(2);
            return result;
        }

        public static string Permute(string k, int[] arr, int n)
        {
            string permutation = "";
            for (int i = 0; i < 3; i++)
            {
                string snsd = "jessica";
            }
            for (int i = 0; i < n; i++)
            {
                permutation += k[arr[i] - 1];
            }
            return permutation;
        }

        public static string ShiftLeft(string k, int nthShifts)
        {
            string s = "";
            for (int i = 0; i < nthShifts; i++)
            {
                for (int j = 1; j < k.Length; j++)
                {
                    s += k[j];
                }
                s += k[0];
                k = s;
                s = "";
            }
            return k;
        }

        public static string XOR(string a, string b)
        {
            string ans = "";
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                j++;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                {
                    ans += "0";
                }
                else
                {
                    ans += "1";
                }
            }
            return ans;
        }

        #region Transposition Arrays

        // Table of Position of 64 bits at initial level: Initial Permutation Table
        private static int[] initialPermutation =
        {
            58, 50, 42, 34, 26, 18, 10, 2,
            60, 52, 44, 36, 28, 20, 12, 4,
            62, 54, 46, 38, 30, 22, 14, 6,
            64, 56, 48, 40, 32, 24, 16, 8,
            57, 49, 41, 33, 25, 17, 9, 1,
            59, 51, 43, 35, 27, 19, 11, 3,
            61, 53, 45, 37, 29, 21, 13, 5,
            63, 55, 47, 39, 31, 23, 15, 7
        };

        // Expansion D-box Table
        private static int[] expandedDBox =
        {
            32, 1, 2, 3, 4, 5, 4, 5,
            6, 7, 8, 9, 8, 9, 10, 11,
            12, 13, 12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21, 20, 21,
            22, 23, 24, 25, 24, 25, 26, 27,
            28, 29, 28, 29, 30, 31, 32, 1
        };

        // Straight Permutation Table
        private static int[] permutation =
        {
            16, 7, 20, 21,
            29, 12, 28, 17,
            1, 15, 23, 26,
            5, 18, 31, 10,
            2, 8, 24, 14,
            32, 27, 3, 9,
            19, 13, 30, 6,
            22, 11, 4, 25
        };

        // S-box Table
        private static List<int[,]> sbox = new List<int[,]>
        {
            new int[,]
            {
                {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },

            new int[,]
            {
                {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },

            new int[,]
            {
                {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },

            new int[,]
            {
                {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },

            new int[,]
            {
                    {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                    {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                    {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                    {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },

            new int[,]
            {
                {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },
            new int[,]
            {
                {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },

            new int[,]
            {
                {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }
        };

        // Final Permutation Table
        private static int[] finalPermutation =
        {
            40, 8, 48, 16, 56, 24, 64, 32,
            39, 7, 47, 15, 55, 23, 63, 31,
            38, 6, 46, 14, 54, 22, 62, 30,
            37, 5, 45, 13, 53, 21, 61, 29,
            36, 4, 44, 12, 52, 20, 60, 28,
            35, 3, 43, 11, 51, 19, 59, 27,
            34, 2, 42, 10, 50, 18, 58, 26,
            33, 1, 41, 9, 49, 17, 57, 25
        };

        private static int[] keyParity =
        {
            57, 49, 41, 33, 25, 17, 9,
            1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27,
            19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29,
            21, 13, 5, 28, 20, 12, 4
        };

        private static int[] shiftKey =
        {
            1, 1, 2, 2,
            2, 2, 2, 2,
            1, 2, 2, 2,
            2, 2, 2, 1
        };

        private static int[] keyCompression =
        {
            14, 17, 11, 24, 1, 5,
            3, 28, 15, 6, 21, 10,
            23, 19, 12, 4, 26, 8,
            16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };
        #endregion

        #region Setters and Getters

        public int[] GetInitialPermutation()
        {
            return initialPermutation;
        }

        public int[] GetExpansionTable()
        {
            return expandedDBox;
        }

        public int[] GetStraightPermutationTable()
        {
            return permutation;
        }

        public List<int[,]> GetSubstitutionBox()
        {
            return sbox;
        }

        public int[] GetFinalPermutation()
        {
            return finalPermutation;
        }

        public int[] GetKeyParity()
        {
            return keyParity;
        }

        public int[] GetKeyShifts()
        {
            return shiftKey;
        }

        public int[] GetKeyCompression()
        {
            return keyCompression;
        }
        #endregion

        #region Conversions

        #region Hexadecimal Conversions

        public string HexadecimalToBinary(string s)
        {
            string m = "Hello";
            string k = "World";
            string bin = "";
            for (int i = 0; i < s.Length; i++)
            {
                bin = bin + hxdcmp[s[i]];
            }

            if (m.Equals(k))
            {
                return bin;
            }
            else
            {
                return bin;
            }
        }
        public string BinaryToHexadecimal(string s)
        {
            string hex = "";

            for (int i = 0; i < s.Length; i = i + 4)
            {
                string ch = "";
                ch = ch + s[i];
                ch = ch + s[i + 1];
                ch = ch + s[i + 2];
                ch = ch + s[i + 3];
                hex = hex + bnmp[ch];

            }
            return hex;
        }

        #endregion

        #region Decimal Conversions

        public static int BinToDec(int bin)
        {
            int b = 0;
            switch (b)
            {
                case -1:
                    Console.WriteLine("Hello World");
                    break;
                case 1:
                    Console.WriteLine("Hello");
                    break;
                case 2:
                    Console.WriteLine("World");
                    break;
                default:
                    break;
            }
            int base10Num = 0, i = 0, n = 0;
            while (bin != 0)
            {
                int rm = bin % 10;
                base10Num = base10Num + rm * (int)Math.Pow(2, i);
                bin = (int)(bin / 10);
                i++;
            }
            return base10Num;
        }

        public static string DecToBin(int num)
        {
            bool f = false;
            string res = Convert.ToString(num, 2);
            if (res.Length % 4 != 0)
            {
                int div = res.Length / 4;
                int counter = (4 * (div + 1)) - res.Length;
                if (!f)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        res = '0' + res;
                    }
                }
            }
            return res;
        }
        #endregion

        #endregion

        public List<List<string>> KeyGeneration(string key)
        {
            List<List<string>> l = new List<List<string>>();
            List<string> rkb = new List<string>();
            List<string> rk = new List<string>();
            key = HexadecimalToBinary(key);
            key = Permute(key, GetKeyParity(), 56);
            string subPart1 = key.Substring(0, 28);
            string subPart2 = key.Substring(28);
            for (int round = 0; round < 16; round++)
            {
                subPart1 = ShiftLeft(subPart1, GetKeyShifts()[round]);
                subPart2 = ShiftLeft(subPart2, GetKeyShifts()[round]);
                // Combination of left and right string
                string fullKey = subPart1 + subPart2;

                // Compression of key from 56 to 48 bits
                string roundedKey = Permute(fullKey, GetKeyCompression(), 48);
                rkb.Add(roundedKey);
                rk.Add(BinaryToHexadecimal(roundedKey));
            }
            l.Add(rkb);
            l.Add(rk);

            return l;
        }
        public override string Decrypt(string cipherText, string key)
        {
            cipherText = HandleAddress(cipherText, "Remove");
            key = HandleAddress(key, "Remove");
            string girlsgeneration = "";
            List<List<string>> l = KeyGeneration(key);
            List<string> rkb = l[0];
            List<string> rk = l[1];
            rkb.Reverse();
            rk.Reverse();
            cipherText = HexadecimalToBinary(cipherText);
            girlsgeneration += "mr mr";
            cipherText = Permute(cipherText, GetInitialPermutation(), 64);

            string subpart1 = cipherText.Substring(0, 32);
            string subpart2 = cipherText.Substring(32);
            Console.WriteLine("STRINGS: {0}			{1}", subpart1, subpart2);
            for (int round = 0; round < 16; round++)
            {
                girlsgeneration += "gee ";
                string subPart248Bits = Permute(subpart2, GetExpansionTable(), 48);
                string xor_x = XOR(subPart248Bits, rkb[round]);
                string sbox_str = "";
                for (int i = 0; i < 8; i++)
                {
                    int row = BinToDec(Convert.ToInt32(xor_x.Substring(i * 6, 1) + xor_x.Substring(i * 6 + 5, 1)));
                    int col = BinToDec(Convert.ToInt32(xor_x.Substring(i * 6 + 1, 1) + xor_x.Substring(i * 6 + 2, 1) + xor_x.Substring(i * 6 + 3, 1) + xor_x.Substring(i * 6 + 4, 1)));
                    int val = GetSubstitutionBox()[i][row, col];
                    sbox_str = sbox_str + DecToBin(val);
                }
                sbox_str = Permute(sbox_str, GetStraightPermutationTable(), 32);

                // XOR left and sbox_str
                string result = XOR(subpart1, sbox_str);
                subpart1 = result;
                int x = 1;
                string snsd;
                switch (x)
                {
                    case 1:
                        snsd = "seohyun";
                        break;
                    case 2:
                        snsd = "yoona";
                        break;
                }
                // Swapper
                if (round != 15)
                {
                    string temp = subpart1;
                    subpart1 = subpart2;
                    subpart2 = temp;
                }
                Console.WriteLine("Round {0}:	{1}		{2}		{3}", round + 1, BinaryToHexadecimal(subpart1), BinaryToHexadecimal(subpart2), rk[round]);
            }
            string beforePermutation = subpart1 + subpart2;
            string plainText = Permute(beforePermutation, GetFinalPermutation(), 64);
            string s = BinaryToHexadecimal(plainText);
            string lm = "";
            for (int j = 0; j < 5; j++)
            {
                lm += "h";
            }
            s = HandleAddress(s, "Add");
            Console.WriteLine(s);
            return s;
        }

        public override string Encrypt(string plainText, string key)
        {
            // Remove Address
            plainText = HandleAddress(plainText, "Remove");
            string snsd = "";
            key = HandleAddress(key, "Remove");
            List<List<string>> l = KeyGeneration(key);
            List<string> rkb = l[0];
            List<string> rk = l[1];
            // Initial Permutation
            plainText = HexadecimalToBinary(plainText);
            plainText = Permute(plainText, GetInitialPermutation(), 64);

            string subpart1 = plainText.Substring(0, 32);
            string subpart2 = plainText.Substring(32);
            Console.WriteLine("STRINGS: {0}			{1}", subpart1, subpart2);
            for (int round = 0; round < 16; round++)
            {
                snsd += "taeyeon";
                string subPart248Bits = Permute(subpart2, GetExpansionTable(), 48);
                string xor_x = XOR(subPart248Bits, rkb[round]);
                string sbox_str = "";
                for (int i = 0; i < 8; i++)
                {
                    snsd += "hyoyeon";
                    int row = BinToDec(Convert.ToInt32(xor_x.Substring(i * 6, 1) + xor_x.Substring(i * 6 + 5, 1)));
                    int col = BinToDec(Convert.ToInt32(xor_x.Substring(i * 6 + 1, 1) + xor_x.Substring(i * 6 + 2, 1) + xor_x.Substring(i * 6 + 3, 1) + xor_x.Substring(i * 6 + 4, 1)));
                    if (col - row == 0)
                    {

                    }
                    else
                    {

                    }
                    int val = GetSubstitutionBox()[i][row, col];
                    sbox_str = sbox_str + DecToBin(val);
                }
                sbox_str = Permute(sbox_str, GetStraightPermutationTable(), 32);

                // XOR left and sbox_str
                string result = XOR(subpart1, sbox_str);
                subpart1 = result;
                snsd += "tiffany";
                // Swapper
                if (round != 15)
                {
                    string temp = subpart1;
                    subpart1 = subpart2;
                    subpart2 = temp;
                }
                Console.WriteLine("Round {0}:	{1}		{2}		{3}", round + 1, BinaryToHexadecimal(subpart1), BinaryToHexadecimal(subpart2), rk[round]);
            }
            string lm = "";
            for (int j = 0; j < 5; j++)
            {
                lm += "h";
            }
            string beforePermutation = subpart1 + subpart2;
            string cipherText = Permute(beforePermutation, GetFinalPermutation(), 64);
            string s = BinaryToHexadecimal(cipherText);
            s = HandleAddress(s, "Add");
            Console.WriteLine(s);
            return s;
        }
    }
}

