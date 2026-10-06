// Program.cs

// Project function library
using Calculator.Libraries;
// Project interface library
using Calculator.Interface;

using System.Text.RegularExpressions;
using Spectre.Console;

// Set initial variables
bool endApp = false;
bool calculating = false;

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
       .Title("Select an option:")
       .AddChoices(Enum.GetValues<Enums.MenuChoice>())
       );

    switch (menuChoice)
    {
        case Enums.MenuChoice.Calculate:
            calculator.StartCalculating();
            break;
        case Enums.MenuChoice.View_Calculations:
            calculating = false;
            calcInterface.UpdateDisplayPanel(calculator._Calculations);
            calcInterface.UpdateMainGrid(Enums.MenuChoice.View_Calculations);
            calcInterface.DisplayGrid();
            // Display calculations
            break;
        case Enums.MenuChoice.Close:
            endApp = true;
            break;
        default:
            endApp = true;
            break;
    }

    while (calculator._Calculating)
    {
        // Get the users input (singular number)
        try
        {
            calculator.GetInput();
        }
        catch (Exception ex)
        {
            endApp = true;
            Messages.Print(ex.Message, Styles.ErrorStyle);
            Messages.ErrorPressAnyKey();
        }

        // Do operation (only if two numbers exist)
        if (calculator.CheckCanOperate())
        {
            try
            {
                calculator.DoOperation();
            }
            catch (Exception ex)
            {
                endApp = true;
                Messages.Print(ex.Message, Styles.ErrorStyle);
                Messages.ErrorPressAnyKey();
            }
        }

        // Update the display panel with "Latest number"
        calcInterface.UpdateLatestNumberPanel(calculator._Result);
        calcInterface.UpdateCalculationsPanel(calculator._CurrentCalculation);

        // Update and display the grid
        calcInterface.UpdateMainGrid(Enums.MenuChoice.Calculate);
        calcInterface.DisplayGrid();

        // Show operation types and assign the users choice
        Enums.OperationChoice operationChoice = AnsiConsole.Prompt(
           new SelectionPrompt<Enums.OperationChoice>()
           .Title("Select an operation:")
           .AddChoices(Enum.GetValues<Enums.OperationChoice>())
           );

        // Try set the menu choice
        try
        {
            calculator.SetOperation(operationChoice);
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

    }
    
/*
    // Update calculations list
    calculator.UpdateCalculationsList();

    calcInterface.UpdateDisplayPanel(calculator._CurrentCalculation);
    calcInterface.UpdateCalculationsPanel(calculator.Calculations);
    calcInterface.UpdateMainGrid();
    calcInterface.DisplayGrid();

    

    // Reset some validation properties
    calculator.ResetInputValidation(); */

}
calculator.FinishJsonWriter();
return;
