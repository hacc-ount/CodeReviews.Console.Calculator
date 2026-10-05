using Calculator.Libraries;
using Spectre.Console;
using System.Text;

namespace Calculator.Interface
{
    // Class for program styles
    internal static class Styles
    {
        internal static Style ErrorStyle { get; private set; } = new Style(foreground: Color.Red, decoration: Decoration.SlowBlink);
        internal static Style ErrorAnyKeyStyle { get; private set; } = new Style(foreground: Color.Red);
        internal static Style TextStyle { get; private set; } = new Style(foreground: Color.Honeydew2);
        internal static Color TextColorStyle { get; private set; } = Color.Honeydew2;
    }

    // Class for the program interface
    internal class CalcInterface
    {
        // Class properties
        //Grids
        internal Grid MainGrid { get; private set; } = new Grid();
        //Panels
        private Panel defaultDisplayPanel = new Panel("Defaultghghghg") { Width = 30 }.Header("Display");
        private Panel defaultCalcListPanel = new Panel("Defaultccc") { Width = 30 }.Header("Calculations");

        private Panel _DisplayPanel { get; set; } = new Panel("----------") { Width = 30 }.Header("Display");
        private Panel _CalculationsPanel { get; set; } = new Panel("----------") { Width = 30 }.Header("Past Calculations");
        private Panel _LatestNumberPanel { get; set; } = new Panel("--") { Width = 30 }.Header("Latest Number");

        // Class constructor
        internal CalcInterface()
        {  
            this.MainGrid = SetGridTemplate();
        }

        // Function sets initial layout to update the default fallback.
        private Grid SetGridTemplate()
        {
            Grid grid = new Grid();

            grid.AddColumn();

            return grid;
        }

        // Function to create and update display panel
        internal void UpdateDisplayPanel(List<string> currentCalculations)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string calculation in currentCalculations)
            {
                sb.Append(calculation);
            }
            // Update with latest calculation (String)
            _DisplayPanel = new Panel(sb.ToString()) { Width = 30 }.Header("Display");
        }

        internal void UpdateCalculationsPanel(List<string> calculationList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string calculation in calculationList)
            {
                sb.Append($"> {calculation}\n");
            }
            _CalculationsPanel = new Panel(sb.ToString()) { Width = 30 }.Header("Calculations");
        }

        internal void UpdateLatestNumberPanel(double number)
        {
            _LatestNumberPanel = new Panel(number.ToString()) { Width = 20 }.Header("Latest Number");
        }

        // Function updates root grid with new panels (fake live updates)
        internal void UpdateMainGrid(Enum choice)
        {
            switch (choice)
            {
                case Enums.MenuChoice.Calculate:
                    MainGrid = SetGridTemplate();
                    MainGrid.AddRow(_CalculationsPanel);
                    MainGrid.AddRow(_LatestNumberPanel);
                    break;
                case Enums.MenuChoice.View_Calculations:
                    MainGrid = SetGridTemplate();
                    MainGrid.AddRow(_CalculationsPanel);
                    break;
            }
        }

        // Function writes the root layout variable
        internal void DisplayGrid()
        {
            // If the object is not initialized then the default layout is applied, throw an exception.
            if (MainGrid == null)
            {
                throw new InvalidOperationException("InvalidOperationException: Interface object was never initialized.\n");
            }
            else
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(MainGrid);
            }
        }
    }

    // Class relating to program messages
    internal static class Messages
    {
        internal static void Print(string sentence, Style style)
        {
            Text message = new Text(sentence, style);
            AnsiConsole.Write(message);
        }

        internal static void ErrorPressAnyKey()
        {
            Text message = new Text("Press any Key to Exit.\n", Styles.ErrorAnyKeyStyle);
            AnsiConsole.Write(message);
            Console.ReadKey();
        }
    }
}
