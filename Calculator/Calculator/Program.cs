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
    // Number input
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

    

    // Do operation (only if two numbers exist).
    if (calculator.CheckCanOperate())
    {
        calculator.DoOperation();
    }

    // Update the display panel with "Latest number"
    calcInterface.UpdateLatestNumberPanel(calculator._Result);
    // Update and display the grid
    calcInterface.UpdateMainGrid();
    calcInterface.DisplayGrid();


    // Show menu options and assign choice.
    Enums.OperationChoice menuChoice = AnsiConsole.Prompt(
       new SelectionPrompt<Enums.OperationChoice>()
       .Title("Select an operation:")
       .AddChoices(Enum.GetValues<Enums.OperationChoice>())
       );

    // Try to set the menu choice
    try
    {
        calculator.SetOperation(menuChoice);
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
