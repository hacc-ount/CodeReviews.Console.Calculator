// Program.cs
// Project function library
using Calculator.Libraries;
using Calculator.Interface;

// Project styles
//using Calculator.Styles;

// Project helper

using System.Text.RegularExpressions;
using Spectre.Console;

bool endApp = false;
CalcInterface calcInterface = new CalcInterface();
try
{
    calcInterface.WriteLayout();
}
catch (Exception ex)
{
    endApp = true;
    Messages.Print(ex.Message, Styles.ErrorStyle);
}


CalculatorLibrary calculator = new CalculatorLibrary();
while (!endApp)
{
    string? numInput1 = "";
    string? numInput2 = "";
    double result = 0;

    Console.Write("Type a number, and then press Enter: ");
    numInput1 = Console.ReadLine();

    double cleanNum1 = 0;
    while(!double.TryParse(numInput1, out cleanNum1))
    {
        Console.Write("This is not a valid input. Please enter a numeric value: ");
        numInput1 = Console.ReadLine();
    }

    Console.Write("Type a number, and then press Enter: ");
    numInput2 = Console.ReadLine();

    double cleanNum2 = 0;
    while (!double.TryParse(numInput2, out cleanNum2))
    {
        Console.Write("This is not a valid input. Please enter a numeric value: ");
        numInput1 = Console.ReadLine();
    }

    Console.WriteLine("Choose an operator from the following list:");
    Console.WriteLine("\ta - Add");
    Console.WriteLine("\ts - Subtract");
    Console.WriteLine("\tm - Multiply");
    Console.WriteLine("\td - Divide");
    Console.Write("Your option? ");

    string? op = Console.ReadLine();

    if (op == null || ! Regex.IsMatch(op, "^(a|s|m|d)$"))
    {
        Console.WriteLine("Error: Unrecognized input.");
    }
    else
    {
        try
        {
            result = calculator.DoOperation(cleanNum1, cleanNum2, op);
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation will result in a mathematical error.\n");
            }
            else Console.WriteLine("Your result: {0:0.##}\n", result);
        }
        catch (Exception e)
        {
            Console.WriteLine("An exception occured when trying to do an operation. Details: " + e.Message);
        }
    }
    Console.WriteLine("-----------------------\n");

    Console.Write("Press 'n' and Enter to close the app, or press any other key and enter to continue: ");
    if (Console.ReadLine() == "n") endApp = true;
    Console.WriteLine("\n");
}
calculator.Finish();
return;
