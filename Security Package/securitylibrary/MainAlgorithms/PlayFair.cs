using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        /// <summary>
        /// The most common diagrams in english (sorted): TH, HE, AN, IN, ER, ON, RE, ED, ND, HA, AT, EN, ES, OF, NT, EA, TI, TO, IO, LE, IS, OU, AR, AS, DE, RT, VE
        /// </summary>
        /// <param name="plainText"></param> // 27 -> 15
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public string Analyse(string plainText)
        {
            throw new NotSupportedException();

        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string cipherText, string key)
        {


            cipherText = cipherText.ToLower();

            string[,] matrix = new string[5, 5];
            HashSet<char> charSet = new HashSet<char>();


            Dictionary<char, bool> alphabetDictionary = new Dictionary<char, bool>();


            for (char c = 'a'; c <= 'z'; c++)
            {
                alphabetDictionary[c] = false;
            }



            int i = 0;
            int j = 0;
            for (int va = 0; va < key.Length; va++)
            {
                if (key[va] == 'i' || key[va] == 'j')
                {
                    if (alphabetDictionary[key[va]])
                        continue;



                    matrix[i, j] = "i".ToString();
                    alphabetDictionary['i'] = true;
                    alphabetDictionary['j'] = true;
                    j++;
                    if (j == 5)
                    {

                        j = 0;
                        i++;
                    }

                }
                else if (!alphabetDictionary[key[va]])
                {
                    matrix[i, j] = key[va].ToString();
                    alphabetDictionary[key[va]] = true;
                    j++;
                    if (j == 5)
                    {

                        j = 0;
                        i++;
                    }
                }





            }



            foreach (char c in alphabetDictionary.Keys.ToList())
            {
                if (!alphabetDictionary[c])
                {
                    if (c == 'i' || c == 'j')
                    {

                        matrix[i, j] = "i".ToString();
                        alphabetDictionary['i'] = true;
                        alphabetDictionary['j'] = true;
                    }


                    else
                    {
                        matrix[i, j] = c.ToString();
                        alphabetDictionary[c] = true;
                    }


                    j++;
                    if (j == 5)
                    {

                        j = 0;
                        i++;
                    }
                }

            }

            string rxt = "";
            for (int ias = 0; ias < cipherText.Length - 1; ias += 2)
            {
                string c1 = cipherText[ias].ToString();
                string c2 = cipherText[ias + 1].ToString();
                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                if (c1 == "i" || c1 == "j")
                    c1 = "i";
                if (c2 == "i" || c2 == "j")
                    c2 = "i";
                for (int ia = 0; ia < 5; ++ia)
                {
                    for (int ja = 0; ja < 5; ++ja)
                    {
                        if (matrix[ia, ja] == c1)
                        {
                            x1 = ia;
                            y1 = ja;
                        }
                        if (matrix[ia, ja] == c2)
                        {
                            x2 = ia; y2 = ja;
                        }


                    }

                }

                if (x1 == x2)
                {
                    if (y1 == 0)
                    {

                        rxt += matrix[x1, 4 % 5];
                    }
                    else
                    {
                        rxt += matrix[x1, (y1 - 1) % 5];
                    }


                    if (y2 == 0)
                    {

                        rxt += matrix[x2, 4 % 5];
                    }
                    else
                    {
                        rxt += matrix[x2, (y2 - 1) % 5];
                    }

                }
                else if (y1 == y2)
                {
                    if (x1 == 0)
                    {
                        rxt += matrix[(4) % 5, y1];
                    }
                    else
                    {
                        rxt += matrix[(x1 - 1) % 5, y1];
                    }


                    if (x2 == 0)
                    {
                        rxt += matrix[(4) % 5, y2];
                    }
                    else
                    {
                        rxt += matrix[(x2 - 1) % 5, y2];
                    }



                }
                else
                {

                    rxt += matrix[x1, y2];
                    rxt += matrix[x2, y1];
                }

            }
            string p = "";

            p += rxt[0];
            p += rxt[1];
            char g = rxt[1];
            for (int id = 2; id < rxt.Length - 1; id += 2)
            {

                if (g == 'x' && p[p.Length - 2] == rxt[id])
                {
                    p = p.Remove(p.Length - 1, 1);
                }
                p += rxt[id];
                p += rxt[id + 1];

                g = rxt[id + 1];

            }

            if (p[p.Length - 1] == 'x')
                p = p.Remove(p.Length - 1, 1);
            return p.ToUpper();









        }

        public string Encrypt(string plainText, string key)
        {

            string sp = "";
            plainText = plainText.ToLower();
            int lsa = 0;
            for (int ias = 0; ias < plainText.Length - 1; ias += 2)
            {
                if (plainText[ias] == plainText[ias + 1])
                {
                    sp += plainText[ias].ToString();
                    sp += 'x'.ToString();
                    ias--;
                }
                else
                {
                    sp += plainText[ias].ToString();
                    sp += plainText[ias + 1].ToString();
                }

                lsa = ias;
            }
            for (lsa = lsa + 2; lsa < plainText.Length; lsa++)
            {
                sp += plainText[lsa];
            }
            if (sp.Length % 2 != 0)
                sp += 'x'.ToString();
            Console.WriteLine(sp);
            plainText = sp;
            string[,] matrix = new string[5, 5];
            HashSet<char> charSet = new HashSet<char>();


            Dictionary<char, bool> alphabetDictionary = new Dictionary<char, bool>();


            for (char c = 'a'; c <= 'z'; c++)
            {
                alphabetDictionary[c] = false;
            }



            int i = 0;
            int j = 0;
            for (int va = 0; va < key.Length; va++)
            {
                if (key[va] == 'i' || key[va] == 'j')
                {
                    if (alphabetDictionary[key[va]])
                        continue;



                    matrix[i, j] = "i".ToString();
                    alphabetDictionary['i'] = true;
                    alphabetDictionary['j'] = true;
                    j++;
                    if (j == 5)
                    {

                        j = 0;
                        i++;
                    }

                }
                else if (!alphabetDictionary[key[va]])
                {
                    matrix[i, j] = key[va].ToString();
                    alphabetDictionary[key[va]] = true;
                    j++;
                    if (j == 5)
                    {

                        j = 0;
                        i++;
                    }
                }





            }



            foreach (char c in alphabetDictionary.Keys.ToList())
            {
                if (!alphabetDictionary[c])
                {
                    if (c == 'i' || c == 'j')
                    {

                        matrix[i, j] = "i".ToString();
                        alphabetDictionary['i'] = true;
                        alphabetDictionary['j'] = true;
                    }


                    else
                    {
                        matrix[i, j] = c.ToString();
                        alphabetDictionary[c] = true;
                    }


                    j++;
                    if (j == 5)
                    {

                        j = 0;
                        i++;
                    }
                }

            }
            for (int ia = 0; ia < 5; ++ia)
            {
                for (int ja = 0; ja < 5; ++ja)
                {
                    Console.Write(matrix[ia, ja]);
                }
                Console.WriteLine();
            }
            string rxt = "";
            for (int ias = 0; ias < plainText.Length - 1; ias += 2)
            {
                string c1 = plainText[ias].ToString();
                string c2 = plainText[ias + 1].ToString();
                int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                if (c1 == "i" || c1 == "j")
                    c1 = "i";
                if (c2 == "i" || c2 == "j")
                    c2 = "i";
                for (int ia = 0; ia < 5; ++ia)
                {
                    for (int ja = 0; ja < 5; ++ja)
                    {
                        if (matrix[ia, ja] == c1)
                        {
                            x1 = ia;
                            y1 = ja;
                        }
                        if (matrix[ia, ja] == c2)
                        {
                            x2 = ia; y2 = ja;
                        }


                    }

                }

                if (x1 == x2)
                {
                    rxt += matrix[x1, (y1 + 1) % 5];
                    rxt += matrix[x2, (y2 + 1) % 5];
                }
                else if (y1 == y2)
                {
                    rxt += matrix[(x1 + 1) % 5, y1];
                    rxt += matrix[(x2 + 1) % 5, y2];
                }
                else
                {

                    rxt += matrix[x1, y2];
                    rxt += matrix[x2, y1];
                }

            }
            Console.WriteLine(rxt);
            return rxt.ToUpper();
        }
    }
}
