using Spectre.Console;

namespace CalculatorHelper
{
    public static class CalcHelper
    {
        public static class Messages
        {
            public static void WriteMessage(string message, string color)
            {
                AnsiConsole.MarkupLine($"[{color}]{message}[/]");
            }
        }
        
    }
}
