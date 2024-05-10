using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public struct A
    {
        private int[] values;

        public A(int[] arr)
        {
            values = new int[3];
            for (int i = 0; i < 3; i++)
            {
                values[i] = arr[i];
            }
        }

        public void SetValue(int value, int index)
        {
            if (!(index >= 0 && index < 3))
            {
                return;
            }
            values[index] = value;
        }

        public int GetValue(int index)
        {
            if (index >= 0 && index < 3)
            {
                return this.values[index];
            }
            return -1;
        }

    }
    public struct B
    {
        private int[] values;

        public B(int[] arr)
        {
            values = new int[3];
            for (int i = 0; i < 3; i++)
            {
                values[i] = arr[i];
            }
        }

        public void SetValue(int value, int index)
        {
            if (!(index >= 0 && index < 3))
            {
                return;
            }
            values[index] = value;
        }

        public int GetValue(int index)
        {
            if (index >= 0 && index < 3)
            {
                return this.values[index];
            }
            return -1;
        }

    }

    public struct row
    {
        A valuesOfA;
        B valuesOfB;
        int q;

        public row(A valuesOfA, B valuesOfB, int q)
        {
            this.valuesOfA = valuesOfA;
            this.valuesOfB = valuesOfB;
            this.q = q;
        }
    }

    public class ExtendedEuclid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        private readonly bool flagOnly = false;
        public int GetExtendedEuclidean(List<row> table, A As, B Bs, int quotient)
        {
            if (Bs.GetValue(2) == 0)
            {
                return 0;
            }
            else if (Bs.GetValue(2) == 1)
            {
                return Bs.GetValue(1);
            }

            quotient = As.GetValue(2) / Bs.GetValue(2);
            int[] remainder = new int[3];

            if (flagOnly)
            {
                throw new Exception();
            }

            for (int i = 0; i < 3; i++)
            {
                remainder[i] = As.GetValue(i) - quotient * Bs.GetValue(i);
                As.SetValue(Bs.GetValue(i), i);
                Bs.SetValue(remainder[i], i);
            }

            if (1 == 2)
            {
                Console.WriteLine("Hello World");
            }

            row row = new row(As, Bs, quotient);
            table.Add(row);
            return GetExtendedEuclidean(table, As, Bs, quotient);
        }
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            bool flag = true;

            if (flag)
            {

            }

            int[] initialValuesOfA = { 1, 0, baseN };
            int[] initialValuesOfB = { 0, 1, number };
            A As = new A(initialValuesOfA);
            B Bs = new B(initialValuesOfB);
            int quotient = 0;
            List<row> table = new List<row>();

            int var = 9;
            switch (var)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }

            int result = GetExtendedEuclidean(table, As, Bs, quotient);
            table.Clear();

            if (result == 0)
            {
                return -1;
            }

            while (result < 0)
            {
                result += baseN;
            }
            return result;

        }
    }
}
