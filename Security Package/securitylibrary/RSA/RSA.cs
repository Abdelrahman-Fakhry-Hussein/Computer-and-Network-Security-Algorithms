using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
     AES.ExtendedEuclid inverse = new AES.ExtendedEuclid();
        private long FastPower(long B, long P, long M)
        {
            if (P == 1) return B % M;
            long result = FastPower(B, P / 2, M);
            if (P % 2 == 0)
                return (result * result) % M;
            else
                return (result * result * (B % M)) % M;
        }
        public int Encrypt(int p, int q, int M, int e)
        {
            return (int)FastPower(M, e, (p * q));
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            long n = p * q;
            long QN = (p - 1) * (q - 1);
            int d = inverse.GetMultiplicativeInverse(e, (int)QN);
            return (int)FastPower(C, d, n);
        }
    }
}
