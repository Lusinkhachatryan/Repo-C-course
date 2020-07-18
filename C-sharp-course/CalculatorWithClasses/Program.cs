using System;
using System.Collections.Generic;

namespace CalculatorWithClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> Expressions1 = new List<string> { "-70+50", "5 *((70+50)/10 - 20)", "7*((1+3)/1 - 9","1 (1.25 + 25)", "7*(1+ -3) - 10", "7*(-3) - 10",
                "6 + 2.5*3" };
        
            //For this list I am checking the validation, before passing expr to the calculator
            //Ctor of Calculater will not check the validity of expression and will nor print errors, this will be done with the help of Validator class
            foreach (var el in Expressions1)
            {
                Calculator Calc;
                Console.WriteLine($"Expression: {el}");
                if (Validator.ValidateArithmeticExpression(el))
                { 
                    Calc = new Calculator(el, false, false);
                    if (Calc.DivisioByZero)
                    {
                        Console.WriteLine("Division by zero is undefined ");
                    }
                    else
                    {
                        Console.WriteLine($"Value of the expression is: {Calc.ExpressionValue}");
                    }
                }
            }

            Console.WriteLine();

            List<string> Expressions2 = new List<string> { "5+7*9 -  8", "8 + 2.3. - 9", "5 + -7", " 5 7*3", "10*5:8", "7 + 5/(10 - 20*0.5)", "2*(8-4) - 5)" };
           
            foreach (var el in Expressions2)
            {
                Console.WriteLine("Expression: {0} ", el);
                Calculator Calc = new Calculator(el);
                
                if (Calc.ExpressionIsValid)
                {
                    if (Calc.DivisioByZero)
                    {
                        Console.WriteLine("Division by zero is undefined ");
                    }
                    else
                    {
                        Console.WriteLine($"Value of the expression is: {Calc.ExpressionValue}");
                    }
                }
            }
                
        }
    }
}
