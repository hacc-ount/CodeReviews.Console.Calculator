using Newtonsoft.Json;
using Calculator.Libraries;
using System.ComponentModel;

namespace Calculator.Libraries
{
    internal class CalculatorLibrary
    {
        JsonWriter writer;
        private Enum _MenuChoice { get; set; } = Enums.Default.Default;
        public CalculatorLibrary()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        internal void SetMenuChoice(Enum menuChoice)
        {
            // If the menuChoice is not in the defined enum value
            if (!Enum.IsDefined(typeof(Enums.MenuChoice), menuChoice))
            {
                throw new InvalidEnumArgumentException("InvalidEnumArgumentException: MenuChoice somehow falls outside of the defined Enum.\n");
            }
            else
            {
                this._MenuChoice = menuChoice;
            }
        }
        internal double DoOperation(double firstNumber, double secondNumber, Enum menuChoice)
        {
            double result = double.NaN;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(firstNumber);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(secondNumber);
            writer.WritePropertyName("Operation");

            switch (menuChoice)
            {
                case Enums.MenuChoice.Add:
                    result = firstNumber + secondNumber;
                    writer.WriteValue("Add");
                    break;
                case Enums.MenuChoice.Subtract:
                    result = firstNumber - secondNumber;
                    writer.WriteValue("Subtract");
                    break;
                case Enums.MenuChoice.Multiply:
                    result = firstNumber * secondNumber;
                    writer.WriteValue("Multiply");
                    break;
                case Enums.MenuChoice.Divide:
                    if (secondNumber != 0)
                    {
                        result = firstNumber / secondNumber;
                        writer.WriteValue("Divide");
                    }
                    break;
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            return result;
        }



        public void FinishJsonWriter()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
