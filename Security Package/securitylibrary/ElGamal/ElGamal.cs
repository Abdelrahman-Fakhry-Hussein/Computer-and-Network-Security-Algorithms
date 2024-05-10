using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="qt"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public List<long> Encrypt(int qt, int alpha, int y, int k, int m)
        {
            int K, x = 1, i;


            //to calculate K 
            i = 0;
            while (i < k)
            {
                x = x * y;
                x %= qt;
                i++;
            }
            K = x;


            int C1;
            x = 1;
            //t
            //to calculate C1 
            i = 0;
            while (i < k)
            {
                x = x * alpha;
                x %= qt;
                i++;
            }
            C1 = x;
            //then
            int C2;
            x = 0;
            i = 0;

            //to calculate C2
            while (i < m)
            {
                x = x + K;
                x %= qt;
                i++;
            }
            C2 = x;

            //generate list to store C1 and C2
            List<long> C = new List<long>();
            C.Add(C1);
            C.Add(C2);
            return C;
        }


        public int GetMultiplicativeInverse(int n, int baseN)
        {
            int rem = baseN;
            int x = 1, y = 0;

            if (baseN == 1)
                return 0;

            while (n > 1)
            {
                //quotient
                int q = n / baseN;
                int t = baseN;

                //remainder

                baseN = n % baseN;
                n = t;
                t = y;

                //update x,y

                y = x - q * y;
                x = t;
            }

            //make sure x positive
            if (x < 0)
                x += rem;

            return x;
        }

        public int Decrypt(int c1, int c2, int x, int q)
        {
            int finalOP = 1;
            int K;
            int v = 1;
            int i;

            //to calculate K
            i = 0;
            while (i < x)
            {
                v = v * c1;
                v %= q;
                i++;
            }
            K = v;

            int v1 = c2 % q;
            int v2 = GetMultiplicativeInverse(K, q);

            //calculate finalOP
            finalOP = (v1 * v2) % q;
            return finalOP;
        }

    }
}
