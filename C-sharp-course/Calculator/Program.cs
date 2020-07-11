using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculater
{
    class Program
    {
        //Tries to evaluate the operations with high precedence, in this case: multiplications and divisions 
        static bool CalcOperswithHigherPrecedence(ref List<double> operandsWithValue, ref List<char> operators)
        {
            
            for (int i = 0; i < operators.Count; ++i)
            {
                if (operators[i] == '*')
                {
                    operandsWithValue[i] *= operandsWithValue[i + 1];
                    operandsWithValue.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    continue;
                }
                if (operators[i] == '/')
                {
                    if (operandsWithValue[i+1] == 0)
                    {
                        Console.WriteLine("Cannot divide by zero");
                        return false;
                    }
                    operandsWithValue[i] /= operandsWithValue[i + 1];
                    operandsWithValue.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    continue;
                }
            }
            return true;
        }

        //Tries to convert operand from string to number
        //At this stage all the expressions in parentheses, that were added to the list "operands" with the help of funtion "GetOperandwithParentheses"
        //will be calculated
        static bool ConvetToNumericValues(List<string> operands, out List<double> operandsWithValues)
        {
            operandsWithValues = new List<double>();
            for (int i = 0; i < operands.Count; ++i)
            {
                double value;
                if (TryCalculate(operands[i], out value))
                {
                    operandsWithValues.Add(value);
                    continue;
                }
                return false;
            }
            return true;
        }

        //Tries to get number from the expression, starting from index i
        static bool GetNumbericOperand(ref int i, string Expr, out string CurrOperand)
        {
            double CurrNum = 0;
            int FirsIndex = i;
            ++i;
            while (i < Expr.Length && (char.IsNumber(Expr[i]) || Expr[i] == '.'))
            {
                ++i;
            }

            if (double.TryParse(Expr.Substring(FirsIndex, i - FirsIndex), out CurrNum))
            {
                CurrOperand = Convert.ToString(CurrNum);
                return true;
            }

            Console.WriteLine("wrong format for the number: {0}", Expr.Substring(FirsIndex, i - FirsIndex));   // 1.25. or 1.25.3
            CurrOperand = default(string);
            return false;
        }

        //Tries to get expression in parentheses, starting from index i
        static bool GetOperandwithParentheses(ref int i, string Expr, out string CurrOperand)
        {
            int Qnt = 1;
            int FirsIndex = i;
            ++i;
            while (i < Expr.Length && Qnt != 0)
            {
                if (Expr[i] == '(')
                {
                    ++Qnt;
                }
                else if (Expr[i] == ')')
                {
                    --Qnt;
                }
                ++i;
            }
            CurrOperand = Expr.Substring(FirsIndex + 1, i - FirsIndex - 2);

            if (Qnt == 0)
            {
                if (CurrOperand.Length > 0)
                {
                    return true;
                }
                Console.WriteLine("Empty parentheses");   // 5 + (())
                CurrOperand = default(string);
                return false;
            }

            Console.WriteLine("Wrong formatting in {0}", Expr.Substring(FirsIndex, i - FirsIndex));   //10 *(7 + 50 / (7+9)
            CurrOperand = default(string);
            return false;
        }

        //Tries to get current operand from the expression, starting from index i
        // As I consider two types of operands: numbers and expressions in parentheses, this function has two subfunctions
        //1. GetNumbericOperand
        //2. GetOperandwithParentheses
        static bool GetOperand(ref int i, string Expr, out string currOperand)
        {
            if (char.IsNumber(Expr[i]))
            {
                if (GetNumbericOperand(ref i, Expr, out currOperand))
                {
                    return true;
                }
                return false;
            }
            if (GetOperandwithParentheses(ref i, Expr, out currOperand))
            {
                return true;
            }
            return false;
        }


        //checks if the given expression is valid, extracts operands and operators from the expression. 
        //As there can be expressions in pharenthesis, the list of operands in this function is of type string
        static bool ProcessExpression(string Expr, out List<string> operands, out List<char> operators)
        {
            operands = new List<string>();
            operators = new List<char>();
            //for validity check, must be 0 || 1 -> before every operator the value should be 1 and before every operator the value should be 0
            int forcheck = 0;

            for (int i = 0; i < Expr.Length;)
            {
                string currOperand;

                if (char.IsNumber(Expr[i]) || Expr[i] == '(')
                {
                    if (GetOperand(ref i, Expr, out currOperand))
                    {
                        if (forcheck == 0)
                        {
                            operands.Add(currOperand);
                            ++forcheck;
                            continue;
                        }
                        Console.WriteLine("Two operands in a row: {0} and {1} without an operator", operands[operands.Count - 1], currOperand);   // 1 + 1.25  25
                        return false;
                    }                    
                    return false;
                }
                else if (Expr[i] == ' ') // considering spaces valid
                {
                    ++i;
                    continue;
                }
                else if (Expr[i] == '+' || Expr[i] == '-' || Expr[i] == '*' || Expr[i] == '/')
                {
                    if (i == 0)
                    {
                        operands.Add("0");
                        ++forcheck;
                    }
                    if (forcheck == 1)
                    {
                        operators.Add(Expr[i]);
                        --forcheck;
                        ++i;
                        continue;
                    }

                    Console.WriteLine("Two operators in a row: {0} and {1} without an operand", operators[operators.Count - 1], Expr[i]);   // 1 + 1.25 -  *
                    return false;
                }
                else  // ) + 5, or other invalid char
                {
                    Console.WriteLine("Invalid character: {0}", Expr[i]);   // 5 + 6 );  ^ & ...
                    return false;
                }
            }
            return true;
        }
        //Indicates if it is possible to calculate the given expression
            //1. tries to process the expression, 2. if expression is valid, tries to convert already extracted operands to their numeric values,
            //3. calculates operations with higher precedence, 4. calculates the final value of the expression
        //Assigns calculated value to the out parameter "value"
        static bool TryCalculate(string Expr, out double value)
        {
            if (double.TryParse(Expr, out value))
            {
                return true;
            }
            List<string> operands;
            List<char> operators;
            List<double> operandsWithValues;

            if (ProcessExpression(Expr, out operands, out operators))
            {
                if (operators.Count + 1 == operands.Count)
                {
                    if (ConvetToNumericValues(operands, out operandsWithValues))
                    {
                        if (CalcOperswithHigherPrecedence(ref operandsWithValues, ref operators))
                        {
                            value = operandsWithValues[0];
                            for (int i = 0; i < operators.Count; ++i)
                            {
                                if (operators[i] == '+')
                                {
                                    value += operandsWithValues[i+1];
                                    continue;
                                }
                                value -= operandsWithValues[i+1];
                            }
                            return true;
                        }
                    }
                }
            }
            value = default(double);
            return false;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                string expression = Console.ReadLine();
                double valexpression;
                if (TryCalculate(expression, out valexpression))
                {
                    Console.WriteLine("Expression equals to : {0}", valexpression);
                }
            }
        }

    }
}