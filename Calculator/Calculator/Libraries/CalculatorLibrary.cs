using Newtonsoft.Json;
using Calculator.Libraries;
using System.ComponentModel;
using Spectre.Console;
using Calculator.Interface;

namespace Calculator.Libraries
{
    internal class CalculatorLibrary
    {
        JsonWriter writer;
        private Enum _MenuChoice { get; set; } = Enums.Default.Default;
        private int _UseCounter { get; set; } = 0;
        private double _UserAnswerOne { get; set; } = 0;
        private double _UserAnswerTwo { get; set; } = 0;
        private double _Result { get; set; } = 0;
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

        internal void GetInputs()
        {
            double userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter first number:[/]");
            _UserAnswerOne = userAnswer;

            double secondAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter second number:[/]");
            _UserAnswerTwo = secondAnswer;
        }

        // Function only used during division operation when trying to divide by zero
        private void GetValidDivisionNumber()
        {
            double userAnswer = 0;

            while (userAnswer == 0)
            {
                userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter a number that's not zero:[/]");
            }

            _UserAnswerTwo = userAnswer;

        }

        internal void DoOperation()
        {
            double result = double.NaN;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(_UserAnswerOne);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(_UserAnswerTwo);
            writer.WritePropertyName("Operation");

            switch (_MenuChoice)
            {
                case Enums.MenuChoice.Add:
                    result = _UserAnswerOne + _UserAnswerTwo;
                    writer.WriteValue("Add");
                    break;
                case Enums.MenuChoice.Subtract:
                    result = _UserAnswerOne - _UserAnswerTwo;
                    writer.WriteValue("Subtract");
                    break;
                case Enums.MenuChoice.Multiply:
                    result = _UserAnswerOne * _UserAnswerTwo;
                    writer.WriteValue("Multiply");
                    break;
                case Enums.MenuChoice.Divide:
                    if (_UserAnswerTwo != 0)
                    {
                        result = _UserAnswerOne / _UserAnswerTwo;
                    }
                    else
                    {
                        GetValidDivisionNumber();
                        result = _UserAnswerOne / _UserAnswerTwo;
                    }
                    writer.WriteValue("Divide");
                    break;
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(_Result);
            writer.WriteEndObject();
            _Result = result;
            // Update use counter here? maybe not...
        }

        internal void UpdateUseCounter()
        {
            this._UseCounter += 1;
        }

        public void FinishJsonWriter()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
