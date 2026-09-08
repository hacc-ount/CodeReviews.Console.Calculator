using Spectre.Console;

namespace Calculator.Interface
{
    internal static class Styles
    {
        internal static Style ErrorStyle { get; private set; } = new Style(foreground: Color.Red, decoration: Decoration.SlowBlink);
    }

    internal class CalcInterface
    {
        internal Layout RootLayout { get; private set; } = new Layout("Default Name");
        private Panel defaultDisplayPanel = new Panel("Default") { Width = 30 }.Header("Display");
        private Panel defaultCalcListPanel = new Panel("Default") { Width = 30 }.Header("Calculations");
        internal CalcInterface()
        {
            //this.RootLayout = SetInitialRootLayout(defaultDisplayPanel, defaultCalcListPanel);
            AnsiConsole.Write(RootLayout);
        }

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

        internal void UpdateRootLayout()
        {

        }

        internal void WriteLayout()
        {
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

    internal static class Messages
    {
        internal static string ErrorPressAnyKey { get; private set; } = "Press any Key to close the program: \n";
        internal static void Print(string sentence, Style style)
        {
            Text message = new Text(sentence, style);
            AnsiConsole.Write(message);
        }
    }
}
