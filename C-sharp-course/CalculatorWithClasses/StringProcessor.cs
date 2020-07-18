using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorWithClasses
{
    static class StringProcessor
    {
        public static bool TrygetNumber(string Expression, ref int i, bool printerrors = true)
        {
            int IndexFrom = i;
            while (i < Expression.Length && (char.IsNumber(Expression[i]) || Expression[i] == '.'))
            {
                ++i;
            }
            string subString = Expression.Substring(IndexFrom, i - IndexFrom);
            double number;
            if (!double.TryParse(subString, out number))
            {
                if (printerrors) Console.WriteLine("wrong format for the number: {0}", subString);   // 1.25. or 1.25.3
                return false;
            }
            return true;
        }
        public static double ExtractNumber(string Expression, ref int i)
        {
            int IndexFrom = i;
            while (i < Expression.Length && (char.IsNumber(Expression[i]) || Expression[i] == '.'))
            {
                ++i;
            }
            return double.Parse(Expression.Substring(IndexFrom, i - IndexFrom));
        }
        public static string ExtractExprationInParentheses(ref int i, string Expration)
        {
            int Qnt = 1;
            int FirsIndex = i;
            ++i;
            while (i < Expration.Length && Qnt != 0)
            {
                if (Expration[i] == '(')
                {
                    ++Qnt;
                }
                else if (Expration[i] == ')')
                {
                    --Qnt;
                }
                ++i;
            }
            return Expration.Substring(FirsIndex + 1, i - FirsIndex - 2);
        }
    }
}
