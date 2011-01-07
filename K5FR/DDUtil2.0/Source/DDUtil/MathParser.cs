using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DataDecoder
{
    public enum Parameters
    {
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }
    public class MathParser
    {
        private Dictionary<Parameters, decimal> _Parameters = new Dictionary<Parameters, decimal>();
        private List<String> OperationOrder = new List<string>();

        public Dictionary<Parameters, decimal> Parameters
        {
            get { return _Parameters; }
            set { _Parameters = value; }
        }

        public MathParser()
        {
            OperationOrder.Add("/");
            OperationOrder.Add("*");
            OperationOrder.Add("-");
            OperationOrder.Add("+");
        }
        public decimal Calculate(string Formula)
        {
            try
            {
                string[] arr = Formula.Split("/+-*()".ToCharArray(),    StringSplitOptions.RemoveEmptyEntries);
                foreach (KeyValuePair<Parameters, decimal> de in _Parameters)
                {
                    foreach(string s in arr)
                    {
                        if (s != de.Key.ToString() && s.EndsWith(de.Key.ToString()))
                        {
                            Formula = Formula.Replace(s, (Convert.ToDecimal(s.Replace(de.Key.ToString(), "")) * de.Value).ToString());
                        }
                    }
                    Formula = Formula.Replace(de.Key.ToString(), de.Value.ToString());
                }
                while (Formula.LastIndexOf("(") > -1)
                {
                    int lastOpenPhrantesisIndex = Formula.LastIndexOf("(");
                    int firstClosePhrantesisIndexAfterLastOpened = Formula.IndexOf(")", lastOpenPhrantesisIndex);
                    decimal result = ProcessOperation(Formula.Substring(lastOpenPhrantesisIndex + 1, firstClosePhrantesisIndexAfterLastOpened - lastOpenPhrantesisIndex - 1));
                    bool AppendAsterix = false;
                    if (lastOpenPhrantesisIndex > 0)
                    {
                        if (Formula.Substring(lastOpenPhrantesisIndex - 1, 1) != "(" && !OperationOrder.Contains(Formula.Substring(lastOpenPhrantesisIndex - 1, 1)))
                        {
                            AppendAsterix = true;
                        }
                    }

                    Formula = Formula.Substring(0, lastOpenPhrantesisIndex) + (AppendAsterix ? "*" : "") + result.ToString() + Formula.Substring(firstClosePhrantesisIndexAfterLastOpened + 1);

                }
                return ProcessOperation(Formula);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured While Calculating. Check Syntax", ex);
            }
        }

        private decimal ProcessOperation(string operation)
        {
            ArrayList arr = new ArrayList();
            string s = "";
            for (int i = 0; i < operation.Length; i++)
            {
                string currentCharacter = operation.Substring(i, 1);
                if (OperationOrder.IndexOf(currentCharacter) > -1)
                {
                    if (s != "")
                    {
                        arr.Add(s);
                    }
                    arr.Add(currentCharacter);
                    s = "";
                }
                else
                {
                    s += currentCharacter;
                }
            }
            arr.Add(s);
            s = "";
            foreach (string op in OperationOrder)
            {
                while (arr.IndexOf(op) > -1)
                {
                    int operatorIndex = arr.IndexOf(op);
                    decimal digitBeforeOperator = Convert.ToDecimal(arr[operatorIndex - 1]);
                    decimal digitAfterOperator = 0;
                    if (arr[operatorIndex + 1].ToString() == "-")
                    {
                        arr.RemoveAt(operatorIndex + 1);
                        digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]) * -1;
                    }
                    else
                    {
                        digitAfterOperator = Convert.ToDecimal(arr[operatorIndex + 1]);
                    }
                    arr[operatorIndex] = CalculateByOperator(digitBeforeOperator, digitAfterOperator, op);
                    arr.RemoveAt(operatorIndex - 1);
                    arr.RemoveAt(operatorIndex);
                }
            }
            return Convert.ToDecimal(arr[0]);
        }
        private decimal CalculateByOperator(decimal number1, decimal number2, string op)
        {
            if (op == "/")
            {
                return number1 / number2;
            }
            else if (op == "*")
            {
                return number1 * number2;
            }
            else if (op == "-")
            {
                return number1 - number2;
            }
            else if (op == "+")
            {
                return number1 + number2;
            }
            else
            {
                return 0;
            }
        }
    }
}
