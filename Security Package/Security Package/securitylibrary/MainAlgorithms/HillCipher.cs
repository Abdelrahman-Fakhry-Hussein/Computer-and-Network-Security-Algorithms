using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Threading.Tasks;

namespace SecurityLibrary
{

    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
        public struct Variables3x3
        {
            public int A;
            public int B;
            public int C;
        };

        public struct Equation3x3
        {
            public Variables3x3 coefficients;
            public int result;
        };

        public struct Variables2x2
        {
            public int A;
            public int B;
        };

        public struct Equation2x2
        {
            public Variables2x2 coefficients;
            public int result;
        };

        public List<Equation2x2> Create2x2Equations(List<List<int>> plainMatrix, List<List<int>> cipherMatrix)
        {
            List<Equation2x2> equations = new List<Equation2x2>();
            for (int i = 0; i < 2; i++)
            {
                Equation2x2 equation1 = new Equation2x2();
                Equation2x2 equation2 = new Equation2x2();
                
                equation1.coefficients.A = plainMatrix[0][0];
                equation1.coefficients.B = plainMatrix[1][0];
                equation1.result = cipherMatrix[i][0];

                equation2.coefficients.A = plainMatrix[0][1];
                equation2.coefficients.B = plainMatrix[1][1];
                equation2.result = cipherMatrix[i][1];

                equations.Add(equation1);
                equations.Add(equation2);
            }
            return equations;
        }

        #region Equation Solving Functions
        public List<Equation3x3> Create3x3Equations(List<List<int>> plainMatrix, List<List<int>> cipherMatrix)
        {
            List<Equation3x3> equations = new List<Equation3x3>();
            for (int i = 0; i < 3; i++)
            {
                Equation3x3 equation1 = new Equation3x3();
                Equation3x3 equation2 = new Equation3x3();
                Equation3x3 equation3 = new Equation3x3();

                equation1.coefficients.A = plainMatrix[0][0];
                equation1.coefficients.B = plainMatrix[1][0];
                equation1.coefficients.C = plainMatrix[2][0];
                equation1.result = cipherMatrix[i][0];

                equation2.coefficients.A = plainMatrix[0][1];
                equation2.coefficients.B = plainMatrix[1][1];
                equation2.coefficients.C = plainMatrix[2][1];
                equation2.result = cipherMatrix[i][1];

                equation3.coefficients.A = plainMatrix[0][2];
                equation3.coefficients.B = plainMatrix[1][2];
                equation3.coefficients.C = plainMatrix[2][2];
                equation3.result = cipherMatrix[i][2];

                equations.Add(equation1);
                equations.Add(equation2);
                equations.Add(equation3);
            }
            return equations;
        }

        public List<int> SolveEquations(List<Equation2x2> equations)
        {
            List<int> answer = new List<int>();
            for (int i = 0; i < equations.Count; i+=2)
            {
                int variable1 = -1, variable2 = -1;
                int A1 = equations[i].coefficients.A;
                int B1 = equations[i].coefficients.B;
                int res1 = equations[i].result;

                int A2 = equations[i+1].coefficients.A;
                int B2 = equations[i+1].coefficients.B;
                int res2 = equations[i + 1].result;
                
                int copyB1 = B1 * A2;
                int copyRes1 = res1 * A2;
                int copyB2 = B2 * A1;
                int copyRes2 = res2 * A1;
                int subtractionResultB = copyB1 - copyB2;
                int subtractionResultResult = copyRes1 - copyRes2;

                Console.WriteLine("Subtraction result result before anything:");
                Console.WriteLine(subtractionResultResult);
                Console.WriteLine("Subtraction B before anything: ");
                Console.WriteLine(subtractionResultB);
                // Cannot be solved
                if (subtractionResultB == 0  && subtractionResultResult != 0)
                {
                    throw new SecurityLibrary.InvalidAnlysisException();
                }


                while (subtractionResultB < 0 || subtractionResultResult < 0)
                {
                    if (subtractionResultB < 0)
                        subtractionResultB += 26;                    
                    
                    if (subtractionResultResult < 0)
                        subtractionResultResult += 26;
                }

                Console.WriteLine("Subtraction result result after removing -ve:");
                Console.WriteLine(subtractionResultResult);
                Console.WriteLine("Subtraction B after removing -ve: ");
                Console.WriteLine(subtractionResultB);
                subtractionResultB %= 26;
                subtractionResultResult %= 26;
                if (subtractionResultResult % subtractionResultB == 0 && subtractionResultB != 0)
                {
                    variable2 = (subtractionResultResult / subtractionResultB) % 26;
                    Console.WriteLine("Variable 2:");
                    Console.WriteLine(variable2);
                    Console.WriteLine("Subtraction result: ");
                    Console.WriteLine(subtractionResultResult);
                    Console.WriteLine("Subtraction B: ");
                    Console.WriteLine(subtractionResultB);
                }
                else
                {
                    int modRes = subtractionResultResult % 26;
                    int modB = subtractionResultB % 26;
                    Console.WriteLine("Check this out:");
                    Console.WriteLine(modB);
                    Console.WriteLine(modRes);
                    Console.WriteLine(ModInverse(modB, 26));
                    Console.WriteLine("**********************************************************************************");
                    if (ModInverse(modRes, modB) == -1)
                        throw new SecurityLibrary.InvalidAnlysisException();
                    int num = 2;
                    while(num <= 1000)
                    {
                        if (modB * num % 26 == modRes)
                        {
                            Console.WriteLine("Variable 2:");
                            Console.WriteLine(num);
                            variable2 = num;
                            break;
                        }
                        num++;
                    }
                }
                int resultOfSubstitution = res1 - B1 * variable2;

                while (resultOfSubstitution < 0)
                {
                    resultOfSubstitution += 26;
                }

                resultOfSubstitution %= 26;
                A1 %= 26;
                if (resultOfSubstitution % A1 == 0 && A1 != 0)
                {
                    variable1 = (resultOfSubstitution / A1) % 26;
                }
                else
                {
                    int modRes = resultOfSubstitution % 26;
                    int modA = A1 % 26;

                    // Does not have a multiplicative inverse
                    if (ModInverse(modRes, modA) == -1)
                        throw new SecurityLibrary.InvalidAnlysisException();

                    int num = 2;
                    while (num <= 1000)
                    {
                        if (modA * num % 26 == modRes)
                        {

                            variable1 = num;
                            Console.WriteLine("Variable 1:");
                            Console.WriteLine(variable1);
                            Console.WriteLine("Subtraction result: ");
                            Console.WriteLine(resultOfSubstitution);
                            Console.WriteLine("A1: ");
                            Console.WriteLine(A1);
                            break;
                        }
                        num++;
                    }
                }
                if (variable1 == -1 || variable2 == -1)
                {
                    throw new SecurityLibrary.InvalidAnlysisException();
                }
                answer.Add(variable1);
                answer.Add(variable2);
                //Console.WriteLine("Variable 1 & 2: {0}, {1}", variable1, variable2);
            }
            return answer;
        }

        public List<int> SolveEquationsSpecialCase(List<Equation2x2> equations)
        {
            List<int> answer = new List<int>();
            for (int i = 0; i < equations.Count; i += 2)
            {
                int variable1 = -1, variable2 = -1;
                int A1 = equations[i].coefficients.A;
                int B1 = equations[i].coefficients.B;
                int res1 = equations[i].result;

                int A2 = equations[i + 1].coefficients.A;
                int B2 = equations[i + 1].coefficients.B;
                int res2 = equations[i + 1].result;

                int copyA1 = B2 * A1;
                int copyRes1 = res1 * B2;
                int copyA2 = B1 * A2;
                int copyRes2 = res2 * B1;
                int subtractionResultA = copyA1 - copyA2;
                int subtractionResultResult = copyRes1 - copyRes2;

                Console.WriteLine("Subtraction result result before anything:");
                Console.WriteLine(subtractionResultResult);
                Console.WriteLine("Subtraction A before anything: ");
                Console.WriteLine(subtractionResultA);
                // Cannot be solved
                if (subtractionResultA == 0 && subtractionResultResult != 0)
                {
                    throw new SecurityLibrary.InvalidAnlysisException();
                }


                while (subtractionResultA < 0 || subtractionResultResult < 0)
                {
                    if (subtractionResultA < 0)
                        subtractionResultA += 26;

                    if (subtractionResultResult < 0)
                        subtractionResultResult += 26;
                }

                Console.WriteLine("Subtraction result result after removing -ve:");
                Console.WriteLine(subtractionResultResult);
                Console.WriteLine("Subtraction A after removing -ve: ");
                Console.WriteLine(subtractionResultA);
                subtractionResultA %= 26;
                subtractionResultResult %= 26;
                if (subtractionResultResult % subtractionResultA == 0 && subtractionResultA != 0)
                {
                    variable2 = (subtractionResultResult / subtractionResultA) % 26;
                    Console.WriteLine("Variable 2:");
                    Console.WriteLine(variable2);
                    Console.WriteLine("Subtraction result: ");
                    Console.WriteLine(subtractionResultResult);
                    Console.WriteLine("Subtraction A: ");
                    Console.WriteLine(subtractionResultA);
                }
                else
                {
                    int modRes = subtractionResultResult % 26;
                    int modB = subtractionResultA % 26;
                    Console.WriteLine("Check this out:");
                    Console.WriteLine(modB);
                    Console.WriteLine(modRes);
                    Console.WriteLine(ModInverse(modB, 26));
                    Console.WriteLine("**********************************************************************************");
                    //if (ModInverse(modRes, modB) == -1)
                    //    throw new SecurityLibrary.InvalidAnlysisException();
                    int num = 2;
                    while (num <= 1000)
                    {
                        if (modB * num % 26 == modRes)
                        {
                            Console.WriteLine("Variable 2:");
                            Console.WriteLine(num);
                            variable2 = num;
                            break;
                        }
                        num++;
                    }
                }
                int resultOfSubstitution;
                if (B1 != 0)
                {
                    resultOfSubstitution = res1 - A1 * variable2;
                }
                else
                {
                    resultOfSubstitution = res2 - A2 * variable2;
                }

                while (resultOfSubstitution < 0)
                {
                    resultOfSubstitution += 26;
                }

                resultOfSubstitution %= 26;
                B1 %= 26;
                Console.WriteLine(B1);
                Console.WriteLine(resultOfSubstitution);
                if (B1 == 0)
                {
                    if (resultOfSubstitution % B2 == 0)
                    {
                        variable1 = (resultOfSubstitution / B2) % 26;
                    }
                    else
                    {
                        int modRes = resultOfSubstitution % 26;
                        int modA = B2 % 26;

                        // Does not have a multiplicative inverse
                        //if (ModInverse(modRes, modA) == -1)
                        //    throw new SecurityLibrary.InvalidAnlysisException();

                        int num = 2;
                        while (num <= 1000)
                        {
                            if (modA * num % 26 == modRes)
                            {

                                variable1 = num;
                                Console.WriteLine("Variable 1:");
                                Console.WriteLine(variable1);
                                Console.WriteLine("Subtraction result: ");
                                Console.WriteLine(resultOfSubstitution);
                                Console.WriteLine("B2: ");
                                Console.WriteLine(B2);
                                break;
                            }
                            num++;
                        }
                    }

                }
                else
                {
                    if (resultOfSubstitution % B1 == 0)
                    {
                        variable1 = (resultOfSubstitution / B1) % 26;
                    }
                    else
                    {
                        int modRes = resultOfSubstitution % 26;
                        int modA = B1 % 26;

                        // Does not have a multiplicative inverse
                        //if (ModInverse(modRes, modA) == -1)
                        //    throw new SecurityLibrary.InvalidAnlysisException();

                        int num = 2;
                        while (num <= 1000)
                        {
                            if (modA * num % 26 == modRes)
                            {

                                variable1 = num;
                                Console.WriteLine("Variable 1:");
                                Console.WriteLine(variable1);
                                Console.WriteLine("Subtraction result: ");
                                Console.WriteLine(resultOfSubstitution);
                                Console.WriteLine("B1: ");
                                Console.WriteLine(B1);
                                break;
                            }
                            num++;
                        }
                    }
                }

               
                
                if (variable1 == -1 || variable2 == -1)
                {
                    throw new SecurityLibrary.InvalidAnlysisException();
                }
                answer.Add(variable2);
                answer.Add(variable1);
                Console.WriteLine("Variable 1 & 2: {0}, {1}", variable1, variable2);
            }
            return answer;
        }
        public List<int> SolveEquations(List<Equation3x3> equations)
        {
            List<int> answer = new List<int>();
            for (int i = 0; i < equations.Count; i += 3)
            {
                int variable1 = -1, variable2 = -1, variable3 =-1;
                int A1 = equations[i].coefficients.A;
                int B1 = equations[i].coefficients.B;
                int C1 = equations[i].coefficients.C;
                int res1 = equations[i].result;

                int A2 = equations[i + 1].coefficients.A;
                int B2 = equations[i + 1].coefficients.B;
                int C2 = equations[i + 1].coefficients.C;
                int res2 = equations[i + 1].result;

                int A3 = equations[i + 2].coefficients.A;
                int B3 = equations[i + 2].coefficients.B;
                int C3 = equations[i + 2].coefficients.C;
                int res3 = equations[i + 2].result;

                // Creating equations to remove variables step by step until we are left with only one variable 
                // E -> equation, A/B/C -> variable
                Equation3x3 equation1 = new Equation3x3();
                Equation3x3 equation2 = new Equation3x3();
                Equation3x3 equation3 = new Equation3x3();
                
                // Equation 1 (Removing A)
                int copyB1 = B1 * A2;
                int copyC1 = C1 * A2;
                int copyRes1 = res1 * A2;

                int copyB2 = B2 * A1;
                int copyC2 = C2 * A1;
                int copyRes2 = res2 * A1;
                 
                equation1.coefficients.A = 0;
                equation1.coefficients.B = copyB1 - copyB2;
                equation1.coefficients.C = copyC1 - copyC2;
                equation1.result = copyRes1 - copyRes2;


                // Equation 2 (Removing A)
                int copyB3 = B3 * A2;
                int copyC3 = C3 * A2;
                int copyRes3 = res3 * A2;

                copyB2 = B2 * A3;
                copyC2 = C2 * A3;
                copyRes2 = res2 * A3;

                equation2.coefficients.A = 0;
                equation2.coefficients.B = copyB2 - copyB3;
                equation2.coefficients.C = copyC2 - copyC3;
                equation2.result = copyRes2 - copyRes3;

                // Equation 3 (Removing B, using Equations 1 & 2 formed from the previous steps)
                equation3.coefficients.A = 0;
                equation3.coefficients.B = 0;
                equation3.coefficients.C = equation2.coefficients.B * equation1.coefficients.C - equation1.coefficients.B * equation2.coefficients.C;
                equation3.result = equation2.coefficients.B * equation1.result - equation1.coefficients.B * equation2.result;

                // Cannot be solved
                if (equation3.coefficients.C == 0 && equation3.result != 0)
                {
                    throw new SecurityLibrary.InvalidAnlysisException();
                }

                while (equation3.coefficients.C < 0 || equation3.result < 0)
                {
                    if (equation3.coefficients.C < 0)
                        equation3.coefficients.C += 26;

                    if (equation3.result < 0)
                        equation3.result += 26;
                }

                equation3.coefficients.C %= 26;
                equation3.result %= 26;
                if (equation3.result % equation3.coefficients.C == 0 && equation3.coefficients.C != 0)
                {

                    variable3 = (equation3.result / equation3.coefficients.C) % 26;
                    Console.WriteLine("Variable 3:");
                    Console.WriteLine(variable3);
                    Console.WriteLine("Subtraction result: ");
                    Console.WriteLine(equation3.result);
                    Console.WriteLine("Subtraction C: ");
                    Console.WriteLine(equation3.coefficients.C);
                }
                else
                {
                    int modRes = equation3.result % 26;
                    int modC = equation3.coefficients.C % 26;
                    if (ModInverse(modC, 26) == -1)
                        throw new SecurityLibrary.InvalidAnlysisException();
                    int num = 2;
                    while (true)
                    {
                        if (modC * num % 26 == modRes)
                        {
                            variable3 = num;
                            Console.WriteLine("Variable 3:");
                            Console.WriteLine(variable3);
                            Console.WriteLine("Subtraction result: ");
                            Console.WriteLine(equation3.result);
                            Console.WriteLine("Subtraction C: ");
                            Console.WriteLine(equation3.coefficients.C);
                            break;
                        }
                        num++;
                    }
                }

                // substitution in eq 1 and 2
                int resultOfSubstitutionInNewSystem = equation1.result - equation1.coefficients.C * variable3;

                while (resultOfSubstitutionInNewSystem < 0 || equation1.coefficients.B < 0)
                { 
                    if (equation1.coefficients.B < 0)
                        equation1.coefficients.B += 26;

                    if (resultOfSubstitutionInNewSystem < 0)
                        resultOfSubstitutionInNewSystem += 26;
                }

                resultOfSubstitutionInNewSystem %= 26;
                equation1.coefficients.B %= 26;
                if (resultOfSubstitutionInNewSystem % equation1.coefficients.B == 0 && equation1.coefficients.B != 0)
                {
                    variable2 = (resultOfSubstitutionInNewSystem / equation1.coefficients.B) % 26;
                    Console.WriteLine("Variable 2:");
                    Console.WriteLine(variable2);
                    Console.WriteLine("Subtraction result: ");
                    Console.WriteLine(resultOfSubstitutionInNewSystem);
                    Console.WriteLine("Subtraction B: ");
                    Console.WriteLine(equation1.coefficients.B);
                }
                else
                {
                    int modRes = resultOfSubstitutionInNewSystem % 26;
                    int modB = equation1.coefficients.B % 26;
                    // Does not have a multiplicative inverse
                    if (ModInverse(modB, 26) == -1)
                        throw new SecurityLibrary.InvalidAnlysisException();

                    int num = 2;
                    while (true)
                    {
                        if (modB * num % 26 == modRes)
                        {
                            variable2 = num;
                            Console.WriteLine("Variable 2:");
                            Console.WriteLine(variable2);
                            Console.WriteLine("Subtraction result: ");
                            Console.WriteLine(resultOfSubstitutionInNewSystem);
                            Console.WriteLine("Subtraction B: ");
                            Console.WriteLine(equation1.coefficients.B);
                            break;
                        }
                        num++;
                    }
                }

                int resultOfSubstitution = res1 - B1 * variable2 - C1 * variable3;
                while (resultOfSubstitution < 0 || A1 < 0)
                {
                    if (A1 < 0)
                        A1 += 26;

                    if (resultOfSubstitution < 0)
                        resultOfSubstitution += 26;
                }

                resultOfSubstitution %= 26;
                A1 %= 26;
                if (resultOfSubstitution % A1 == 0 && A1 != 0)
                {
                    variable1 = (resultOfSubstitution / A1) % 26;
                    Console.WriteLine("Variable 1:");
                    Console.WriteLine(variable1);
                    Console.WriteLine("Subtraction result: ");
                    Console.WriteLine(resultOfSubstitution);
                    Console.WriteLine("A: ");
                    Console.WriteLine(A1);
                }
                else
                {
                    int modRes = resultOfSubstitution % 26;
                    int modA = A1 % 26;
                    // Does not have a multiplicative inverse
                    if (ModInverse(modA, 26) == -1)
                        throw new SecurityLibrary.InvalidAnlysisException();

                    int num = 2;
                    while (true)
                    {
                        if (modA * num % 26 == modRes)
                        {
                            variable1 = num;
                            Console.WriteLine("Variable 1:");
                            Console.WriteLine(variable1);
                            break;
                        }
                        num++;
                    }
                }
                
                Console.WriteLine("variable 1, 2, 3: {0}    {1}    {2}", variable1, variable2, variable3);
                if (variable1 == -1 || variable2 == -1 || variable3 == -1)
                {
                    throw new SecurityLibrary.InvalidAnlysisException();
                }
                answer.Add(variable1);
                answer.Add(variable2);
                answer.Add(variable3);
                //Console.WriteLine("Variable 1 & 2: {0}, {1}", variable1, variable2);
            }
            return answer;
        }
        #endregion

        #region Display Elements

        public void PrintEquation(List<Equation2x2> equations)
        {
            for (int i = 0; i < equations.Count; i++)
            {
                Console.WriteLine("Equation {0}:", i + 1);
                Console.WriteLine("Coefficient 1: {0}", equations[i].coefficients.A);
                Console.WriteLine("Coefficient 2: {0}", equations[i].coefficients.B);
                Console.WriteLine("Result: {0}", equations[i].result);
            }
        }

        public void PrintEquation(List<Equation3x3> equations)
        {
            for (int i = 0; i < equations.Count; i++)
            {
                Console.WriteLine("Equation {0}:", i + 1);
                Console.WriteLine("Coefficient 1: {0}", equations[i].coefficients.A);
                Console.WriteLine("Coefficient 2: {0}", equations[i].coefficients.B);
                Console.WriteLine("Coefficient 3: {0}", equations[i].coefficients.C);
                Console.WriteLine("Result: {0}", equations[i].result);
            }
        }

        public void PrintMatrix<T>(List<List<T>> matrix)
        {
            Console.WriteLine("Matrix:");
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    Console.Write(matrix[i][j]);
                    Console.Write("\t");
                }
                Console.WriteLine();
            }
        }
        
        public void PrintList(List<int> list)
        {
            Console.Write("List: ");
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write("{0} ", list[i]);
            }
            Console.WriteLine();
            
        }
        #endregion

        #region Create Matrix
        public List<List<T>> CreateMatrix<T>(int rowNumber, int columnNumber)
        {
            List<List<T>> matrix = new List<List<T>>();
            for (int i = 0; i < rowNumber; i++)
            {
                List<T> row = new List<T>();
                for (int j = 0; j < columnNumber; j++)
                {
                    row.Add(default(T));
                }
                matrix.Add(row);
            }
            return matrix;
        }

        public List<List<int>> CreateMatrix(List<int> list, int rowNumber, int columnNumber, bool isOffsetByRow)
        {
            
            int columnNum = 1;
            int rowNum = 1;
            if (isOffsetByRow)
            {
                columnNum = columnNumber;
            }
            else
            {
                rowNum = rowNumber;
            }

            List<List<int>> matrix = new List<List<int>>();
            for (int i = 0; i < rowNumber; i++)
            {
                List<int> row = new List<int>();
                for (int j = 0; j < columnNumber; j++)
                {
                    row.Add(list.ElementAt(i * columnNum + j * rowNum));
                }
                matrix.Add(row);
            }
            return matrix;
        }

        public List<List<double>> CreateMatrixDouble(List<int> list, int rowNumber, int columnNumber, bool isOffsetByRow)
        {

            int columnNum = 1;
            int rowNum = 1;
            if (isOffsetByRow)
            {
                columnNum = columnNumber;
            }
            else
            {
                rowNum = rowNumber;
            }

            List<List<double>> matrix = new List<List<double>>();
            for (int i = 0; i < rowNumber; i++)
            {
                List<double> row = new List<double>();
                for (int j = 0; j < columnNumber; j++)
                {
                    row.Add(list.ElementAt(i * columnNum + j * rowNum));
                }
                matrix.Add(row);
            }
            return matrix;
        }
        #endregion

        #region Conversion
        public string ConvertListIntoString(List<int> list)
        {
            string s = "";
            for (int i = 0; i < list.Count; i++)
            {
                char c = (char)('a' + list[i]);
                s += c;
                Console.WriteLine(s);
            }
            return s;
        }
        public List<int> ConvertStringIntoList(string s)
        {
            s = s.ToLower();
            List<int> list = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                list.Add(s[i] - 'a');
            }
            return list;
        }
        public List<int> ConvertMatrixIntoList(List<List<int>> matrix)
        {
            List<int> result = new List<int>();
            for (int j = 0; j < matrix[0].Count; j++)
            {
                for (int i = 0; i < matrix.Count; i++)
                {
                    result.Add(matrix[i][j]);
                }
            }
            return result;
        }

        public List<int> ConvertMatrixIntoList(List<List<double>> matrix)
        {
            List<int> result = new List<int>();
            for (int j = 0; j < matrix[0].Count; j++)
            {
                for (int i = 0; i < matrix.Count; i++)
                {
                    result.Add((int)matrix[i][j]);
                }
            }
            return result;
        }
        #endregion

        #region Modulus Matrix
        public List<List<int>> ModulusMatrix(List<List<int>> matrix)
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    matrix[i][j] = (int) matrix[i][j];
                    matrix[i][j] = matrix[i][j] % 26;
                    while (matrix[i][j] < 0)
                    {
                        matrix[i][j] += 26;
                    }
                }
            }
            return matrix;
        }

        public List<List<double>> ModulusMatrix(List<List<double>> matrix)
        {

            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    matrix[i][j] = (int)matrix[i][j];
                    matrix[i][j] = matrix[i][j] % 26;
                    while (matrix[i][j] < 0)
                    {
                        matrix[i][j] += 26;
                    }
                }
            }
            return matrix;
        }
        #endregion

        #region GCD
        public int GetGreatestCommonDivisorBetweenTwoSizes(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }
            int remainder = a % b;
            a = b;
            b = remainder;
            return GetGreatestCommonDivisorBetweenTwoSizes(a, b);
        }
        #endregion

        #region MatrixMultiplication
        public List<List<int>> MultiplyMatrix(List<List<int>> keyMatrix, List<List<int>> plainTextMatrix, List<List<int>> result)
        {
            for (int i = 0; i < keyMatrix.Count; i++)
            {
                for (int k = 0; k < plainTextMatrix[i].Count; k++)
                {
                    for (int j = 0; j < keyMatrix[i].Count; j++)
                    {
                        result[i][k] += keyMatrix[i][j] * plainTextMatrix[j][k];
                    }
                }
            }
            return result;
        }

        public List<List<double>> MultiplyMatrix(List<List<double>> keyMatrix, List<List<int>> cipherTextMatrix, List<List<double>> result)
        {
            for (int i = 0; i < keyMatrix.Count; i++)
            {
                for (int k = 0; k < cipherTextMatrix[i].Count; k++)
                {
                    for (int j = 0; j < keyMatrix[i].Count; j++)
                    {
                        result[i][k] += keyMatrix[i][j] * cipherTextMatrix[j][k];
                    }
                }
            }
            return result;
        }


        public List<List<double>> MatrixMultiplicationByConstant(List<List<double>> matrix, int constant)
        {
            for (int i = 0; i < matrix.Count; i++)
            {
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    matrix[i][j] *= constant;
                }
            }
            return matrix;
        }
        #endregion

        #region Determinant
        public double DeterminantOfAMatrix(List<List<double>> matrix) 
        {
            double det = 0;
                int n = matrix.Count;

            if (n == 1)
            {
                if (matrix[0][0] == 0)
                {
                    return 0;
                }
                return matrix[0][0];
            }

            if (n == 2)
            {
                if ((matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0]) == 0)
                {
                    return 0;
                }
                return matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
            }

            for (int j = 0; j < n; j++)
            {
                List<List<double>> minorMatrix = new List<List<double>>();
                for (int i = 1; i < n; i++)
                {
                    List<double> row = new List<double>();
                    for (int k = 0; k < n; k++)
                    {
                        if (k != j)
                            row.Add(matrix[i][k]);
                    }
                    minorMatrix.Add(row);
                }
                int sign = (j % 2 == 0) ? 1 : -1;
                det += sign * matrix[0][j] * DeterminantOfAMatrix(minorMatrix);
            }

            if (det == 0)
            {
                return 0;
            }

            return det;
        }
        #endregion

        #region Matrix Inverse
        public List<List<double>> CreateCofactorMatrix(List<List<double>> matrix)
        {
            int numRows = matrix.Count;
            int numCols = matrix[0].Count;

            List<List<double>> cofactorMatrix = new List<List<double>>();

            for (int i = 0; i < numRows; i++)
            {
                List<double> row = new List<double>();

                for (int j = 0; j < numCols; j++)
                {
                    double cofactor = GetCofactor(matrix, i, j);
                    row.Add(cofactor);
                }

                cofactorMatrix.Add(row);
            }

            return cofactorMatrix;
        }

        public double GetCofactor(List<List<double>> matrix, int row, int col)
        {
            int numRows = matrix.Count;
            int numCols = matrix[0].Count;

            List<List<double>> minorMatrix = new List<List<double>>();

            for (int i = 0; i < numRows; i++)
            {
                if (i != row)
                {
                    List<double> minorRow = new List<double>();

                    for (int j = 0; j < numCols; j++)
                    {
                        if (j != col)
                        {
                            minorRow.Add(matrix[i][j]);
                        }
                    }

                    minorMatrix.Add(minorRow);
                }
            }

            double minorDeterminant = DeterminantOfAMatrix(minorMatrix);

            // Calculate the sign based on the position of the element
            double sign = ((row + col) % 2 == 0) ? 1 : -1;

            return sign * minorDeterminant;
        }

        public static List<List<double>> TransposeMatrix(List<List<double>> matrix)
        {
            int numRows = matrix.Count;
            int numCols = matrix[0].Count;

            List<List<double>> transpose = new List<List<double>>();

            for (int j = 0; j < numCols; j++)
            {
                List<double> row = new List<double>();
                for (int i = 0; i < numRows; i++)
                {
                    row.Add(matrix[i][j]);
                }
                transpose.Add(row);
            }

            return transpose;
        }

        public List<List<double>> CreateAdjointMatrix(List<List<double>> matrix)
        {
            List<List<double>> cofactorMatrix = CreateCofactorMatrix(matrix);
            List<List<double>> adjointMatrix = TransposeMatrix(cofactorMatrix);
            return adjointMatrix;
        }
        #endregion

        #region Modulus
        public int Mod(int a)
        {
            int factor = 1;
            int res = 1;
            do
            {
              factor++;
            } while ((a * factor) % 26 != 1);
            return factor;
        }
        public int ModInverse(int a, int m)
        {
            int m0 = m;
            int y = 0, x = 1;

            while (a > 1 || m > 0)
            {
                // Handle division by zero
                if (m == 0)
                {
                    // A does not have a multiplicative inverse modulo B
                    return -1;
                }
                // Calculate quotient
                int q = a / m;
                int t = m;

                // Update m and a
                m = a % m;

                a = t;

                // Update x and y using the Extended Euclidean Algorithm
                t = y;
                y = x - q * y;
                x = t;
            }

            // Ensure x is positive
            if (x < 0)
                x += m0;

            return x;
        }


        #endregion


        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            const int shape = 2;
            int plainCount = plainText.Count;
            int cipherCount = cipherText.Count;
            List<List<int>> keyMatrix = CreateMatrix<int>(shape, shape);
            List<List<int>> plainMatrix = CreateMatrix(plainText, shape, plainCount / shape, false);
            List<List<int>> cipherMatrix = CreateMatrix(cipherText, shape, cipherCount / shape, false);
            Console.WriteLine(ModInverse(20, 18));
            

            Console.Write("Plain ");
            PrintMatrix(plainMatrix);
            Console.Write("Cipher ");
            PrintMatrix(cipherMatrix);
            string line = new string('-', 50);
            Console.WriteLine(line);
            Console.WriteLine("Since Key matrix is 2x2 then we need to solve for 4 variables\n" +
                              "1 row contains 2 variables then we will require 2 equations per row\n" +
                              "1 row -> 2 variables -> 2 equations -> 2 columns from Plain Text matrix");
            Console.WriteLine(line);

            List<Equation2x2> equations = Create2x2Equations(plainMatrix, cipherMatrix);
            PrintEquation(equations);
            List<int> key = SolveEquations(equations);
            Console.WriteLine(line);
            return key;
        }

        public string Analyse(string plainText, string cipherText)
        {
            const int shape = 2;
            List<int> plainTextList = ConvertStringIntoList(plainText);
            List<int> cipherTextList = ConvertStringIntoList(cipherText);
            int plainCount = plainTextList.Count;
            int cipherCount = cipherTextList.Count;
            PrintList(plainTextList);
            PrintList(cipherTextList);
            List<List<int>> plainMatrix = CreateMatrix(plainTextList, shape, plainCount / shape, false);
            List<List<int>> cipherMatrix = CreateMatrix(cipherTextList, shape, cipherCount / shape, false);
            List<Equation2x2> equations = Create2x2Equations(plainMatrix, cipherMatrix);
            PrintEquation(equations);
            List<int> key = SolveEquationsSpecialCase(equations);
            List<List<double>> keyMatrix = CreateMatrixDouble(key, shape, shape, true);

            double determinant = DeterminantOfAMatrix(keyMatrix);
            Console.WriteLine(determinant);
            Console.WriteLine("----------------------");
            if (determinant <= 0 && determinant != -1)
            {
                throw new SecurityLibrary.InvalidAnlysisException();
            }
            double s1 = Math.Max(determinant, 26);
            double s2 = Math.Min(determinant, 26);

            // Check if determinant doesn't have an inverse modulus -> cannot make an inverse matrix
            int modInverse = ModInverse((int)determinant, 26);
            if (modInverse == -1)
            {
                throw new Exception();
            }
            string s = ConvertListIntoString(key);
            return s;
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> plainText = new List<int>();
            
            // Get sizes
            int n = key.Count();
            int m = cipherText.Count();
            int gcd = GetGreatestCommonDivisorBetweenTwoSizes(m, n);

            // Display the lists and their sizes
            Console.WriteLine("Key Size: {0}\nP.T Size: {1}", n, m);
            Console.WriteLine("Cipher Text List: ");
            PrintList(cipherText);
            Console.WriteLine("Key List: ");
            PrintList(key);


            // Get key and cipher text matrices shapes
            const int x = 2;
            double keyRowNumberDouble = (int)Math.Log(n, x), keyColumnNumberDouble = (int)Math.Log(n, x);
            
            // not important
            double keyRowNumberDoubleSqrt = Math.Sqrt(n);
            
            int keyColumnNumber = (int) keyRowNumberDouble, keyRowNumber = (int) keyRowNumberDouble;
            int cipherTextRowNumber = keyColumnNumber;
            int cipherTextColumnNumber = m / cipherTextRowNumber;
            
            // Check if it's not a square matrix -> cannot make an inverse matrix
            if (keyRowNumber - (int) keyRowNumber != 0)
            {
                throw new Exception();
            }

            // Create key matrix and checking determinant
            List<List<double>> keyMatrix = CreateMatrixDouble(key, keyRowNumber, keyColumnNumber, true);
            
            // Check if determinant equals zero -> cannot make an inverse matrix
            double determinant = DeterminantOfAMatrix(keyMatrix);
            if (determinant == 0)
            {
                throw new Exception();
            }
            double s1 = Math.Max(determinant, 26);
            double s2 = Math.Min(determinant, 26);

            // Check if determinant doesn't have an inverse modulus -> cannot make an inverse matrix
            int modInverse = ModInverse((int) determinant, 26);
            if (modInverse == -1 && determinant != 26)
            {
                throw new Exception();
            }

            // Handle -ve determinant
            double determinantMod = determinant % 26;
            Console.WriteLine("Determinant: {0}\nDeterminant Mod 26: {1}", determinant, determinantMod);
            while (determinant < 0)
            {
                determinant += 26;
            }
            Console.WriteLine("+ve Determinant: {0}", determinant);


            List<List<double>> plainTextMatrix = CreateMatrix<double>(keyRowNumber, cipherTextColumnNumber);
            List<List<int>> cipherTextMatrix = CreateMatrix(cipherText, cipherTextRowNumber, cipherTextColumnNumber, false);

            // Print matrices
            Console.Write("Key ");
            PrintMatrix<double>(keyMatrix);
            Console.Write("Cipher ");
            PrintMatrix<int>(cipherTextMatrix);
            
            //Console.Write("Key Inverse ");
            //PrintMatrix(inverseKeyMatrix);
            
            List<List<double>> adjointKeyMatrix = CreateAdjointMatrix(keyMatrix);
            Console.Write("Key Adjoint ");
            PrintMatrix(adjointKeyMatrix);
            List<List<double>> adjointModKeyMatrix = ModulusMatrix(adjointKeyMatrix);
            Console.Write("Key Modulus Adjoint ");
            PrintMatrix(adjointModKeyMatrix);
            Console.WriteLine(modInverse);
            int multiplicativeInverse = Mod((int)determinant);
            adjointModKeyMatrix = MatrixMultiplicationByConstant(adjointModKeyMatrix, multiplicativeInverse);
            Console.Write("Key Multiplied ");
            PrintMatrix(adjointModKeyMatrix);
            adjointModKeyMatrix = ModulusMatrix(adjointModKeyMatrix);
            plainTextMatrix = MultiplyMatrix(adjointModKeyMatrix, cipherTextMatrix, plainTextMatrix);
      
            plainTextMatrix = ModulusMatrix(plainTextMatrix);
            plainText = ConvertMatrixIntoList(plainTextMatrix);
            Console.Write("Plain ");
            PrintMatrix(plainTextMatrix);
            Console.WriteLine("Plain List: ");
            PrintList(plainText);

            return plainText;
        }

        public string Decrypt(string cipherText, string key)
        {
            List<int> cipherTextList = ConvertStringIntoList(cipherText);
            List<int> keyList = ConvertStringIntoList(key);
            PrintList(cipherTextList);
            PrintList(keyList);
            List<int> plainTextList = Decrypt(cipherTextList, keyList);
            PrintList(cipherTextList);
            string plainText = ConvertListIntoString(plainTextList);
            Console.WriteLine(plainText);
            return plainText;
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {

            List<int> cipherText = new List<int>();

            int n = key.Count();
            int m = plainText.Count();
            int x = GetGreatestCommonDivisorBetweenTwoSizes(m, n);
            int keyRowNumber = n / x;
            int keyColumnNumber, plainTextRowNumber;
            keyColumnNumber = plainTextRowNumber = x;
            int plainTextColumnNumber = m / x;

            // list
            x = 2;
            keyRowNumber = keyColumnNumber = (int) Math.Log(n, x);
            plainTextRowNumber = keyColumnNumber;
            plainTextColumnNumber = m / plainTextRowNumber;
            int plainTextRowCount = m / keyRowNumber;
            
            // Display Size
            Console.WriteLine("Key Size: {0}\nP.T Size: {1}", n, m);
            
            // Display Lists
            Console.WriteLine("Plain Text List: ");
            PrintList(plainText);
            Console.WriteLine("Key List: ");
            PrintList(key);

            // Create Matrix
            List<List<int>> keyMatrix = CreateMatrix(key, keyRowNumber, keyColumnNumber, true);
            List<List<int>> plainTextMatrix = CreateMatrix(plainText, plainTextRowNumber, plainTextColumnNumber, false);
            List<List<int>> encryptedMatrix = CreateMatrix<int>(keyRowNumber, plainTextColumnNumber);

            // Create List
            //List<int> encryptedList = new List<int>();
            PrintMatrix(keyMatrix);
            PrintMatrix(plainTextMatrix);
            List<List<int>> encryptedTextMatrix = MultiplyMatrix(keyMatrix, plainTextMatrix, encryptedMatrix);
            encryptedMatrix = ModulusMatrix(encryptedMatrix);
            PrintMatrix(encryptedTextMatrix);
            
            cipherText = ConvertMatrixIntoList(encryptedTextMatrix);
            PrintList(cipherText);
            return cipherText;
        }

        public string Encrypt(string plainText, string key)
        {
            List<int> plainTextList = ConvertStringIntoList(plainText); 
            List<int> keyList = ConvertStringIntoList(key);
            PrintList(plainTextList);
            PrintList(keyList);
            List<int> cipherTextList = Encrypt(plainTextList, keyList);
            PrintList(cipherTextList);
            string cipherText = ConvertListIntoString(cipherTextList);
            Console.WriteLine(cipherText);
            return cipherText;
        }

        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            const int shape = 3;
            int plainCount = plain3.Count;
            int cipherCount = cipher3.Count;
            List<List<int>> keyMatrix = CreateMatrix<int>(shape, shape);
            List<List<int>> plainMatrix = CreateMatrix(plain3, shape, plainCount / shape, false);
            List<List<int>> cipherMatrix = CreateMatrix(cipher3, shape, cipherCount / shape, false);
            PrintMatrix(plainMatrix);
            PrintMatrix(cipherMatrix);
            List<Equation3x3> equations = Create3x3Equations(plainMatrix, cipherMatrix);
            PrintEquation(equations);
            List<int> key = SolveEquations(equations);
            return key;
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            const int shape = 3;
            List<int> plainTextList = ConvertStringIntoList(plain3);
            List<int> cipherTextList = ConvertStringIntoList(cipher3);
            int plainCount = plainTextList.Count;
            int cipherCount = cipherTextList.Count;
            List<List<int>> keyMatrix = CreateMatrix<int>(shape, shape);
            List<List<int>> plainMatrix = CreateMatrix(plainTextList, shape, plainCount / shape, false);
            List<List<int>> cipherMatrix = CreateMatrix(cipherTextList, shape, cipherCount / shape, false);
            PrintMatrix(plainMatrix);
            PrintMatrix(cipherMatrix);
            List<Equation3x3> equations = Create3x3Equations(plainMatrix, cipherMatrix);
            PrintEquation(equations);
            List<int> key = SolveEquations(equations);
            string keyy = ConvertListIntoString(key);
            return keyy;
        }
    }
}
