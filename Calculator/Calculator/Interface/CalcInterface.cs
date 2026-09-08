using Spectre.Console;

namespace Calculator.Interface
{
    // Class for program styles
    internal static class Styles
    {
        internal static Style ErrorStyle { get; private set; } = new Style(foreground: Color.Red, decoration: Decoration.SlowBlink);
        internal static Style ErrorAnyKeyStyle { get; private set; } = new Style(foreground: Color.Red);
        internal static Style TextStyle { get; private set; } = new Style(foreground: Color.Honeydew2);
    }

    // Class for the program interface
    internal class CalcInterface
    {
        // Class properties
        internal Layout RootLayout { get; private set; } = new Layout("Default Name");
        private Panel defaultDisplayPanel = new Panel("Default") { Width = 30 }.Header("Display");
        private Panel defaultCalcListPanel = new Panel("Default") { Width = 30 }.Header("Calculations");

        // Class constructor
        internal CalcInterface()
        {
            this.RootLayout = SetInitialRootLayout(defaultDisplayPanel, defaultCalcListPanel);
            AnsiConsole.Write(RootLayout);
        }

        // Function sets initial layout to update the default fallback.
        private Layout SetInitialRootLayout(Panel displayPanel, Panel calcListPanel)
        {
            Layout rootLayout = new Layout("root")
                .SplitColumns(
                new Layout("body").Size(40));

            rootLayout["body"].SplitRows(
                new Layout("display").Ratio(1),
                new Layout("calcList").Ratio(2));

            rootLayout["body"]["display"].Update(displayPanel);
            rootLayout["body"]["calcList"].Update(calcListPanel);

            return rootLayout;
        }

        // Function updates root layout with new panels (fake live updates)
        internal void UpdateRootLayout()
        {

        }

        // Function writes the root layout variable
        internal void WriteLayout()
        {
            // If the object is not initialized then the default layout is applied, throw an exception.
            if (RootLayout.Name == "Default Name")
            {
                throw new InvalidOperationException("InvalidOperationException: Interface object was never initialized.\n");
            }
            else
            {
                Console.Clear();
                AnsiConsole.Write(RootLayout);
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
