using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorWithClasses
{
    static class Validator
    {       
        public static bool ValidateArithmeticExpression(string Expression, bool printerrors = true)
        {
            //for validity check, must be 0 || 1 -> before every operator the value should be 1 and before every operator the value should be 0
            int forcheck = 0;
            //validity check for parentheses
            int QntOfParentheses = 0;

            for (int i = 0; i < Expression.Length;)
            {
                if (char.IsNumber(Expression[i]))
                {
                    if (StringProcessor.TrygetNumber(Expression, ref i, printerrors))
                    {
                        if (forcheck == 0)
                        {
                            ++forcheck;
                            continue;
                        }
                        if (printerrors) Console.WriteLine("Two operands in a row without an operator");   // 1 + 1.25  25
                        return false;
                    }
                    return false;
                }
                if (Expression[i] == ' ') // considering spaces valid
                {
                    ++i;
                    continue;
                }
                if (Expression[i] == '+' || Expression[i] == '-' || Expression[i] == '*' || Expression[i] == '/')
                {
                    if (i == 0)
                        ++forcheck;
                    if (forcheck == 1)
                    {
                        --forcheck;
                        ++i;
                        continue;
                    }
                    if (printerrors) Console.WriteLine("Two operators in a row without an operand");   // 1 + 1.25 -  *
                    return false;
                }
                if (Expression[i] == '(')
                {
                    ++QntOfParentheses;
                    ++i;
                    continue;
                }
                if (Expression[i] == ')')
                {
                    if (QntOfParentheses > 0)
                    {
                        --QntOfParentheses;
                        ++i;
                        continue;
                    }
                    if (printerrors) Console.WriteLine("Invalid use of parentheses");   // 5 + 6 );  ^ & ...
                    return false;
                }
                // ) + 5, or expr with other invalid char: {, ' ...
                if (printerrors) Console.WriteLine("Invalid character: {0}", Expression[i]);   // 5 + 6 );  ^ & ...
                return false;
            }
            if (QntOfParentheses != 0)
            {
                if (printerrors) Console.WriteLine("Invalid use of parentheses");   // 5 + 6 );  ^ & ...
                return false;
            }
            //if (printerrors) Console.WriteLine("Expression is valid");
            return true;
        }
    }
}
