// Program.cs

// Project function library
using Calculator.Libraries;
// Project interface library
using Calculator.Interface;

using System.Text.RegularExpressions;
using Spectre.Console;

// Set initial variables
bool endApp = false;

// Initialize the programs interface
CalcInterface calcInterface = new CalcInterface();
try
{
    calcInterface.DisplayGrid();
}
catch (Exception ex)
{
    endApp = true;
    Messages.Print(ex.Message, Styles.ErrorStyle);
    Messages.ErrorPressAnyKey();
}

// Initialize the programs library
CalculatorLibrary calculator = new CalculatorLibrary();
while (!endApp)
{
    // Show menu options and assign choice.
     Enums.MenuChoice menuChoice = AnsiConsole.Prompt(
        new SelectionPrompt<Enums.MenuChoice>()
        .Title("Select an operation:")
        .AddChoices(Enum.GetValues<Enums.MenuChoice>())
        );

    // Try to set the menu choice
    try
    {
        calculator.SetMenuChoice(menuChoice);
    }
    catch (Exception ex)
    {
        endApp = true;
        Messages.Print(ex.Message, Styles.ErrorStyle);
        Messages.ErrorPressAnyKey();
    }
    
    // Try update calculation symbol
    try
    {
        calculator.UpdateMethodSymbol();
    }
    catch (Exception ex)
    {
        endApp = true;
        Messages.Print(ex.Message, Styles.ErrorStyle);
        Messages.ErrorPressAnyKey();
    }

    calculator.GetInput();
    calculator.UpdateCurrentCalculation();
    calcInterface.UpdateDisplayPanel(calculator._CurrentCalculation);
    calcInterface.UpdateMainGrid();
    calcInterface.DisplayGrid();

    //string? op = Console.ReadLine();

    /*if (op == null || ! Regex.IsMatch(op, "^(a|s|m|d)$"))
    {
        Console.WriteLine("Error: Unrecognized input.");
    }
    else
    {
        try
        {
            result = calculator.DoOperation(cleanNum1, cleanNum2, menuChoice);
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
    Console.WriteLine("\n");*/
}
calculator.FinishJsonWriter();
return;
