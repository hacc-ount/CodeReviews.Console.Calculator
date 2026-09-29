using Calculator.Interface;
using Calculator.Libraries;
using Newtonsoft.Json;
using Spectre.Console;
using System.ComponentModel;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Calculator.Libraries
{
    internal class CalculatorLibrary
    {
        JsonWriter writer;
        internal Enum _MenuChoice { get; private set; } = Enums.Default.Default;
        private int _UseCounter { get; set; } = 0;
        internal double _UserFirstInput { get; private set; } = 0;
        internal double _UserSecondInput { get; private set; } = 0;

        private bool _FirstInputInputted { get; set; } = false;
        private bool _SecondInputInputted { get; set; } = false;
        internal double _Result { get; private set; } = 0;
        private string _CurrentMethodSymbol { get; set; } = "@";
        internal string _CurrentCalculation { get; private set; } = "";

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

        internal void UpdateMethodSymbol()
        {
            switch (_MenuChoice)
            {
                case Enums.MenuChoice.Add:
                    _CurrentMethodSymbol = "+";
                    break;
                case Enums.MenuChoice.Subtract:
                    _CurrentMethodSymbol = "-";
                    break;
                case Enums.MenuChoice.Multiply:
                    _CurrentMethodSymbol = "*";
                    break;
                case Enums.MenuChoice.Divide:
                    _CurrentMethodSymbol = "/";
                    break;
                case Enums.Default.Default:
                    _CurrentMethodSymbol = "@";
                    throw new InvalidOperationException("InvalidOperationException: Not a valid menuChoice symbol");
            }
        }

        // Function fo singular input.
        internal void GetInput()
        {
            if (_FirstInputInputted == false)
            {
                double userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter first number:[/]");
                _UserFirstInput = userAnswer;
                _FirstInputInputted = true;
            }
            else
            {
                double secondAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter second number:[/]");
                _UserSecondInput = secondAnswer;
                _SecondInputInputted = true;
            }   
        }

        // Function only used during division operation when trying to divide by zero
        private void GetValidDivisionNumber()
        {
            double userAnswer = 0;

            while (userAnswer == 0)
            {
                Messages.Print("Cannot divide by zero, please enter another number\n", Styles.ErrorStyle);
                userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter a number that's not zero:[/] ");
            }

            _UserSecondInput = userAnswer;

        }

        internal void DoOperation()
        {
            double result = double.NaN;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(_UserFirstInput);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(_UserSecondInput);
            writer.WritePropertyName("Operation");

            switch (_MenuChoice)
            {
                case Enums.MenuChoice.Add:
                    result = _UserFirstInput + _UserSecondInput;
                    writer.WriteValue("Add");
                    break;
                case Enums.MenuChoice.Subtract:
                    result = _UserFirstInput - _UserSecondInput;
                    writer.WriteValue("Subtract");
                    break;
                case Enums.MenuChoice.Multiply:
                    result = _UserFirstInput * _UserSecondInput;
                    writer.WriteValue("Multiply");
                    break;
                case Enums.MenuChoice.Divide:
                    if (_UserSecondInput != 0)
                    {
                        result = _UserFirstInput / _UserSecondInput;
                    }
                    else
                    {
                        GetValidDivisionNumber();
                        result = _UserFirstInput / _UserSecondInput;
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

        internal void UpdateCurrentCalculation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_UserFirstInput.ToString());
            sb.Append($" {_CurrentMethodSymbol} ");
            if (_SecondInputInputted == false)
            {
                _CurrentCalculation = sb.ToString();
            }
            else
            {
                sb.Append(_UserSecondInput.ToString());
                sb.Append($" = {_Result.ToString()}");
                _CurrentCalculation = sb.ToString();
            } 
        }

        internal void ResetInputValidation()
        {
            _FirstInputInputted = false;
            _SecondInputInputted = false;
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
