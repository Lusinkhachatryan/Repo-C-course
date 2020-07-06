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
        static void getMult_Divval(ref int i, char op, ref string Expr, ref List<string> Nums, out string CurrOperand, ref bool IsValid)
        {
            if (Expr[i] == '(' || char.IsNumber(Expr[i]))
            {
                double Opnd1 = Calculate(Nums[Nums.Count - 1], ref IsValid);
                double Opnd2 = 0;
                if (IsValid)
                {
                    getOperand(ref i, ref Expr, out CurrOperand, ref IsValid);
                    if (IsValid)
                    {
                        Opnd2 = Calculate(CurrOperand, ref IsValid);
                        if (IsValid)
                        {
                            if (op == '*')
                            {
                                Nums[Nums.Count - 1] = (Opnd1 * Opnd2).ToString();
                                return;
                            }
                            if (Opnd2 == 0)
                            {
                                Console.WriteLine("Cannot divide by zero");
                                IsValid = false;
                                return;
                            }
                            Nums[Nums.Count - 1] = (Opnd1 / Opnd2).ToString();
                            return;
                        }
                    }
                }
                CurrOperand = "-1";
                return;
            }
            CurrOperand = "-1";
            Console.WriteLine("Invalid character: {0}", Expr[i]);   // 5 + 6 * -1;
            IsValid = false;
        }
        static void getNumber(ref int i, ref string Expr, out string CurrOperand, ref bool IsValid)
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
                return;
            }

            Console.WriteLine("wrong format for the number: {0}", Expr.Substring(FirsIndex, i - FirsIndex));   // 1.25. or 1.25.3
            IsValid = false;
            CurrOperand = "-1";
        }
        static void getOperwithparentheses(ref int i, ref string Expr, out string CurrOperand, ref bool IsValid)
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
                    return;
                }
                Console.WriteLine("Empty parentheses");   // 5 + (())
                IsValid = false;
                CurrOperand = "-1";
                return;
            }
            Console.WriteLine("Wrong formatting in {0}", Expr.Substring(FirsIndex , i - FirsIndex));   //10 *(7 + 50 / (7+9)
            IsValid = false;
            CurrOperand = "-1";
        }
        static void getOperand(ref int i, ref string Expr, out string CurrOperand, ref bool IsValid)
        {
            if (char.IsNumber(Expr[i]))
            {
                getNumber(ref i, ref Expr, out CurrOperand, ref IsValid);
                return;
            }
            getOperwithparentheses(ref i, ref Expr, out CurrOperand, ref IsValid);
            return;
        }

        static bool AddOperand(ref int forcheck, ref List<string> Nums, ref string CurrOperand, ref bool IsValid)
        {
            if (forcheck == 0)
            {
                Nums.Add(CurrOperand);
                ++forcheck;
                return true;
            }
            Console.WriteLine("Two operands in a row: {0} and {1} without an operator}", Nums[Nums.Count - 1], CurrOperand);   // 1 + 1.25  25
            IsValid = false;
            return false;
        }
        static double Calculate(string Expr, ref bool IsValid)
        {
            double Val;
            if (double.TryParse(Expr, out Val))
            {
                return Val;
            }
            List<string> Nums = new List<string>();
            List<char> Opers = new List<char>();
            int forcheck = 0;  // must be 0 || 1

            for (int i = 0; i < Expr.Length;)
            {
                string CurrOperand;
                if (char.IsNumber(Expr[i]))
                {
                    getOperand(ref i, ref Expr, out CurrOperand, ref IsValid);
                    if (!(IsValid && AddOperand(ref forcheck, ref Nums, ref CurrOperand, ref IsValid)))
                    {
                        return -1;
                    }
                }
                else if (Expr[i] == ' ') // considering spaces valid
                {
                    ++i;
                    continue;
                }
                else if (Expr[i] == '+' || Expr[i] == '-')
                {
                    if (i == 0)
                    {
                        Nums.Add("0");
                        ++forcheck;
                    }
                    if (forcheck == 1)
                    {
                        Opers.Add(Expr[i]);
                        --forcheck;
                        ++i;
                        continue;
                    }

                    Console.WriteLine("Two operators in a row: {0} and {1} without an operand", Opers[Opers.Count - 1], Expr[i]);   // 1 + 1.25 -  *
                    IsValid = false;
                    return -1;

                }
                else if (Expr[i] == '*' || Expr[i] == '/')
                {
                    char op = Expr[i];
                    if (i == 0)
                    {
                        Nums.Add("0");
                        ++forcheck;
                    }
                    ++i;
                    while (Expr[i] == ' ')
                    {
                        ++i;
                    }
                    getMult_Divval(ref i, op, ref Expr, ref Nums, out CurrOperand, ref IsValid);
                    if (!IsValid)
                    {
                        return -1;
                    }
                }
                else if (Expr[i] == '(')
                {
                    getOperand(ref i, ref Expr, out CurrOperand, ref IsValid);
                    if (!(IsValid && AddOperand(ref forcheck, ref Nums, ref CurrOperand, ref IsValid)))
                    {
                        return -1;
                    }
                }
                else  // ) + 5, or other invalid char
                {
                    Console.WriteLine("Invalid character: {0}", Expr[i]);   // 5 + 6 );  ^ & ...
                    IsValid = false;
                    return -1;
                }
            }
            if (Opers.Count + 1 == Nums.Count)
            {
                Val = Calculate(Nums[0], ref IsValid);
                for (int i = 0; i < Opers.Count; ++i)
                {
                    if (Opers[i] == '+')
                    {
                        Val += Calculate(Nums[i + 1], ref IsValid);
                        continue;
                    }
                    Val -= Calculate(Nums[i + 1], ref IsValid);
                }
            }
            return Val;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                string Expression = Console.ReadLine();
                bool IsValid = true;
                double FinalVal = Calculate(Expression, ref IsValid);
                if (IsValid)
                {
                    Console.WriteLine("Expression equals to : {0}", FinalVal);
                }
            }
        }

    }
}
