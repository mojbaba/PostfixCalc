// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography;

var infix = "5 * 6 + ( 7 + 6 / 2 )";

var postFix = ToPostfix(infix);

postFix.ForEach(p => System.Console.Write(p + " "));

var calculated = Calculate(postFix);

System.Console.WriteLine("= " + calculated);

static List<object> ToPostfix(string infix)
{
    Stack<object> postfix = new Stack<object>();
    Stack<char> operators = new Stack<char>();

    foreach (var item in infix.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    {
        var token = item switch
        {
            "+" or "-" or "*" or "/" or "^" or "(" or ")" => item[0] as object,
            _ => double.Parse(item) as object
        };


        if (token is char c)
        {
            if (c == '(')
            {
                operators.Push(c);
                continue;
            }

            if (c == ')')
            {
                while (operators.TryPeek(out var pp) && pp != '(')
                {
                    postfix.Push(operators.Pop());
                }

                operators.Pop();
                continue;
            }

            if (operators.Count == 0)
            {
                operators.Push(c);
                continue;
            }

            {
                var last = operators.Peek();

                while (operators.TryPeek(out var pp) && periority(c) <= periority(pp) && periority(pp) > 0)
                {
                    postfix.Push(operators.Pop());
                }

                operators.Push(c);
            }

        }
        else
        {
            postfix.Push(token);
        }
    }

    while (operators.TryPop(out var op))
    {
        postfix.Push(op);
    }

    return postfix.Reverse().ToList();
}

static double Calculate(List<object> postfix)
{
    var operands = new Stack<double>();

    foreach (var item in postfix)
    {
        if (item is double num)
        {
            operands.Push(num);
            continue;
        }
        else if (item is char c)
        {
            var second = operands.Pop();
            var first = operands.Pop();

            var calc = c switch
            {
                '+' => first + second,
                '-' => first - second,
                '*' => first * second,
                '/' => first / second,
                '^' => Math.Pow(first, second),
                _ => throw new NotImplementedException()
            };

            operands.Push(calc);
        }
    }

    return operands.Single();
}


static int periority(char c) => c switch
{
    '-' or '+' => 1,
    '*' or '/' => 2,
    '^' => 3,
    '(' => 0,
    _ => throw new NotSupportedException("operator not found")
};