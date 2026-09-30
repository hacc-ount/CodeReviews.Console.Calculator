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
        internal Enum _OperationChoice { get; private set; } = Enums.Default.Default;
        internal double _FirstNumber { get; private set; } = double.NaN;
        
        internal double _LatestNumber { get; private set; } = double.NaN;

        internal double _Result { get; private set; } = 0;
        private string _CurrentOperationSymbol { get; set; } = "@";
        internal List<string> _CurrentCalculation { get; private set; } = new List<string>();

        internal List<string> Calculations { get; private set; } = new List<string>();

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

        internal void SetOperation(Enum menuChoice)
        {
            // If the menuChoice is not in the defined enum value
            if (!Enum.IsDefined(typeof(Enums.OperationChoice), menuChoice))
            {
                throw new InvalidEnumArgumentException("InvalidEnumArgumentException: MenuChoice somehow falls outside of the defined Enum.\n");
            }
            else
            {
                this._OperationChoice = menuChoice;
            }
        }

        internal void UpdateMethodSymbol()
        {
            switch (_OperationChoice)
            {
                case Enums.OperationChoice.Add:
                    _CurrentOperationSymbol = "+";
                    break;
                case Enums.OperationChoice.Subtract:
                    _CurrentOperationSymbol = "-";
                    break;
                case Enums.OperationChoice.Multiply:
                    _CurrentOperationSymbol = "*";
                    break;
                case Enums.OperationChoice.Divide:
                    _CurrentOperationSymbol = "/";
                    break;
                case Enums.Default.Default:
                    _CurrentOperationSymbol = "@";
                    throw new InvalidOperationException("InvalidOperationException: Not a valid menuChoice symbol");
            }
        }

        // Function fo singular input.
        internal void GetInput()
        {
            double userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter a number:[/]");
            if (userAnswer == double.NaN)
            {
                throw new InvalidDataException("InvalidDataExcpetion: User input was NaN.");
            }
            else if (_FirstNumber == double.NaN)
            {
                // If no first number has been input, assign it
                _FirstNumber = userAnswer;
                _CurrentCalculation.Add(_FirstNumber.ToString());
            }
            else
            {
                // Assign every other number to this current number.
                _LatestNumber = userAnswer;
                _CurrentCalculation.Add(_LatestNumber.ToString());
            }
        }

        // Function only used during division operation when trying to divide by zero
      /*  private void GetValidDivisionNumber()
        {
            double userAnswer = 0;

            while (userAnswer == 0)
            {
                Messages.Print("Cannot divide by zero, please enter another number\n", Styles.ErrorStyle);
                userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter a number that's not zero:[/] ");
            }

            _CurrentUserInput = userAnswer;

        }*/

        internal void DoOperation()
        {
            double result = double.NaN;
            /*writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(_UserFirstInput);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(_UserSecondInput);
            writer.WritePropertyName("Operation");*/

           /* switch (_OperationChoice)
            {
                case Enums.OperationChoice.Add:
                    result = _CurrentUserInput + _UserSecondInput;
                    //writer.WriteValue("Add");
                    break;
                case Enums.OperationChoice.Subtract:
                    result = _UserFirstInput - _UserSecondInput;
                    //writer.WriteValue("Subtract");
                    break;
                case Enums.OperationChoice.Multiply:
                    result = _UserFirstInput * _UserSecondInput;
                    //writer.WriteValue("Multiply");
                    break;
                case Enums.OperationChoice.Divide:
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
            // Update use counter here? maybe not...*/
        }

       /* internal void UpdateCurrentCalculation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_UserFirstInput.ToString());
            sb.Append($" {_CurrentOperationSymbol} ");
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
        }*/

        internal void UpdateCalculationsList()
        {
            Calculations.Add(_CurrentCalculation);
        }

        public void FinishJsonWriter()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
