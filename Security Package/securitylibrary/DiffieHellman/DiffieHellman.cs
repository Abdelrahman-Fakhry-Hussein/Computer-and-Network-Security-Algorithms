using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
    {
        static int key(int alpha, int x, int a, int b)
        {
            //initialization
            int output = 1, mul = 1, counter = 0;
            //alpha^b mod q
            int i = 0;
            while (i < b)
            {
                mul = (alpha * mul) % x;
                i++;
            }
            //(alpha^b)^a mod q
            //
            i = 0;
            while (i < a)
            {
                output = (mul * output) % x; // Calculate (alpha^b)^a mod q
                i++;
            }
            return output;
        }
        public List<int> GetKeys(int q, int alpha, int x1, int x2)
        {
            List<int> keys = new List<int>();
            keys.Add(key(alpha, q, x1, x2));
            //versas
            keys.Add(key(alpha, q, x2, x1));
            //the list of generated keys
            return keys;
        }
    }
}
