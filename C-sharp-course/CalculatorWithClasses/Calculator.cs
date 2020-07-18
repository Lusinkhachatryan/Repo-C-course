using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorWithClasses
{
    class Calculator
    {
        public string Expression { get; }
        public bool ExpressionIsValid { get; private set; }
        public double ExpressionValue { get; private set; }
        public bool DivisioByZero { get; private set; }
        bool GeterrorMessages { get; set; }
        List<double> Operands { get; set; }
        List<char> Operators { get; set; }
        public Calculator(string expression, bool geterrorMessages = true, bool checkValidity = true)
        {
            ExpressionIsValid = !checkValidity;
            this.GeterrorMessages = geterrorMessages;
            this.Expression = expression;
            if(!ExpressionIsValid) ExpressionIsValid = Validator.ValidateArithmeticExpression(expression, GeterrorMessages);

            if (ExpressionIsValid)
            {
                ExpressionValue = this.CalcValue();
            }
        }
        double CalcValue()
        {
            Operands = new List<double>();
            Operators = new List<char>();
            double value = 0;
            for (int i = 0; i < Expression.Length;)
            {
                if (char.IsNumber(Expression[i]))
                {
                    Operands.Add(StringProcessor.ExtractNumber(Expression,ref i));
                    continue;
                }
                if (Expression[i] == '(')
                {
                    Operands.Add(new Calculator(StringProcessor.ExtractExprationInParentheses(ref i, Expression),GeterrorMessages).ExpressionValue);
                    continue;
                }
                if (Expression[i] != ' ')
                {
                    Operators.Add(Expression[i]);
                    ++i;
                    continue;
                }
                ++i;
            }
            if (this.CanCalcOperswithHigherPrecedence())
            {
                value = Operands[0];
                for (int i = 0; i < Operators.Count; ++i)
                {
                    if (Operators[i] == '+')
                    {
                        value += Operands[i + 1];
                        continue;
                    }
                    value -= Operands[i + 1];
                }
            }
            return value;
        }
        bool CanCalcOperswithHigherPrecedence()
        {

            for (int i = 0; i < Operators.Count; ++i)
            {
                if (Operators[i] == '*')
                {
                    Operands[i] *= Operands[i + 1];
                    Operands.RemoveAt(i + 1);
                    Operators.RemoveAt(i);
                    continue;
                }
                if (Operators[i] == '/')
                {
                    if (Operands[i + 1] == 0)
                    {
                        this.DivisioByZero = true;
                        return false;
                    }
                    Operands[i] /= Operands[i + 1];
                    Operands.RemoveAt(i + 1);
                    Operators.RemoveAt(i);
                    continue;
                }
            }
            return true;
        }
    }
}
