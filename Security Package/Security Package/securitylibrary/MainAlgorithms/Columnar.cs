using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {

        static List<int[]> Permute(int[] array, int i, int n)
        {
            List<int[]> result = new List<int[]>();
            int j;
            if (i == n)
            {
                result.Add(array.ToArray()); // Clone the array to avoid modifications
            }
            else
            {
                for (j = i; j <= n; j++)
                {
                    Swap(ref array[i], ref array[j]);
                    result.AddRange(Permute(array, i + 1, n));
                    Swap(ref array[i], ref array[j]); // backtrack
                }
            }
            return result;
        }

        static void Swap(ref int a, ref int b)
        {
            int tmp;
            tmp = a;
            a = b;
            b = tmp;
        }
        public List<int> Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            for (int i = 1; i <= plainText.Length; i++)
            {
                List<int> arr = new List<int>();
                for (int ii = 1; ii <= i; ii++)
                    arr.Add(ii);


                List<int[]> p = Permute(arr.ToArray(), 0, i - 1);
                foreach (var val in p)
                {
                    if (this.Encrypt(plainText, val.ToList()) == cipherText)
                    {
                        return val.ToList();
                    }
                }

            }
            return null;


        }

        public string Decrypt(string cipherText, List<int> key)
        {


            foreach (var row in key)
            {
                Console.Write(row);

            }
            //  Console.WriteLine();

            //    Console.WriteLine(cipherText);
            //   Console.WriteLine(key.Count);

            int size_of_key = key.Count;
            int rows = (int)Math.Ceiling((cipherText.Length + 0.0) / size_of_key);
            int incre = (int)cipherText.Length - ((cipherText.Length / size_of_key) * size_of_key);
            //   Console.WriteLine(rows);
            //    Console.WriteLine(size_of_key);

            List<List<string>> L = new List<List<string>>(rows);

            for (int i = 0; i < rows; i++)
            {
                List<string> row = new List<string>();

                for (int j = 0; j < size_of_key; j++)
                {
                    row.Add("");
                }

                L.Add(row);
            }

            int k = 0;
            for (int j = 1; j <= size_of_key; j++)
            {
                int idx = 0;
                idx = key.IndexOf(j);
                int i = 0;
                for (i = 0; i < (cipherText.Length / size_of_key); i++)
                {


                    L[i][idx] = cipherText[k].ToString();
                    k++;



                }
                if (incre > 0)
                {

                    L[i][idx] = cipherText[k].ToString();
                    k++;
                    incre--;
                }
            }




            string outs = "";
            foreach (var row in L)
            {
                foreach (var element in row)
                {
                    outs += element;
                }

            }


            //       Console.WriteLine(outs);
            return outs;
        }

        public string Encrypt(string plainText, List<int> key)
        {

            List<List<string>> L = new List<List<string>>();
            int j = 1;
            List<String> s = new List<string>();
            for (int i = 0; i < plainText.Length; i++)
            {
                j += 1;
                s.Add(plainText[i].ToString());

                if (j > key.Count)
                {
                    //            Console.WriteLine("s");
                    //           Console.WriteLine(s.Count);
                    j = 1;
                    L.Add(s);
                    s = new List<String>();

                }
            }
            if (j > 1)
            {
                for (; j <= key.Count; j++)
                    s.Add("");
                L.Add(s);
            }





            string outs = "";

            int Si = L.Count;
            int Si2 = key.Count;
            for (int y = 1; y <= Si2; y++)
            {
                int ind = key.IndexOf(y);

                for (int i = 0; i < Si; i++)
                {

                    outs += L[i][ind];
                }

            }
            //  Console.WriteLine(outs);
            return outs;
            // throw new NotImplementedException();
        }
    }
}
