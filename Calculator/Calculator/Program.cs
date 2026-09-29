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

    // First number input
    calculator.GetInput();
    calculator.UpdateCurrentCalculation();
    calcInterface.UpdateDisplayPanel(calculator._CurrentCalculation);
    calcInterface.UpdateMainGrid();
    calcInterface.DisplayGrid();

    // Second number input
    calculator.GetInput();
    calculator.DoOperation();
    calculator.UpdateCurrentCalculation();

    // Update calculations list
    calculator.UpdateCalculationsList();

    calcInterface.UpdateDisplayPanel(calculator._CurrentCalculation);
    calcInterface.UpdateCalculationsPanel(calculator.Calculations);
    calcInterface.UpdateMainGrid();
    calcInterface.DisplayGrid();

    

    // Reset some validation properties
    calculator.ResetInputValidation();

}
calculator.FinishJsonWriter();
return;
