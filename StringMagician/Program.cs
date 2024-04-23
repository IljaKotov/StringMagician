using System.Text;
using StringMagician;

Console.WriteLine("Enter the expression:");
var exp = Console.ReadLine();
var op=new OperationFactory();
var hndl = new ExpressionHandler(op);

Console.WriteLine(hndl.Handle(exp));