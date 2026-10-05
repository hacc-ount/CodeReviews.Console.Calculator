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

        private bool _OperationStart { get; set; } = false;
        
        internal double _LatestNumber { get; private set; } = double.NaN;

        internal double _Result { get; private set; } = double.NaN;
        private string _CurrentOperationSymbol { get; set; } = "@";
        internal List<string> _CurrentCalculation { get; private set; } = new List<string>();

        internal List<List<string>> Calculations { get; private set; } = new List<List<string>>();

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
            if (double.IsNaN(userAnswer))
            {
                throw new InvalidDataException("InvalidDataExcpetion: User input was NaN.");
            }
            else if (double.IsNaN(_Result))
            {
                // If no first number has been input, assign it
                _Result = userAnswer;
                //_CurrentCalculation.Add(_Result.ToString());
            }
            else
            {
                // Assign every other number to this current number.
                _LatestNumber = userAnswer;
                //_CurrentCalculation.Add(_LatestNumber.ToString());
            }
        }

        // Function to do the operation
        internal void DoOperation()
        {
            double temp = 0;
                switch (_OperationChoice)
                {
                    case Enums.OperationChoice.Add:
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            temp = _Result;
                            _Result = _Result + _LatestNumber;
                            UpdateCurrentCalculation(temp, _LatestNumber, "+", _Result);
                        }
                        //writer.WriteValue("Add");
                        break;
                    case Enums.OperationChoice.Subtract:
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            temp = _Result;
                            _Result = _Result - _LatestNumber;
                            UpdateCurrentCalculation(temp, _LatestNumber, "-", _Result);
                        }
                        //writer.WriteValue("Subtract");
                        break;
                    case Enums.OperationChoice.Multiply:
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            temp = _Result;
                            _Result = _Result * _LatestNumber;
                            UpdateCurrentCalculation(temp, _LatestNumber, "*", _Result);
                        }
                        //writer.WriteValue("Multiply");
                        break;
                    case Enums.OperationChoice.Divide:
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            if (_LatestNumber != 0)
                            {
                                temp = _Result;
                                _Result = _Result / _LatestNumber;
                                UpdateCurrentCalculation(temp, _LatestNumber, "/", _Result);
                            }
                            else
                            {
                                GetValidDivisionNumber();
                                temp = _Result;
                                _Result = _Result / _LatestNumber;
                                UpdateCurrentCalculation(temp, _LatestNumber, "/", _Result);
                            }
                        }
                    //writer.WriteValue("Divide");
                        break;
                    default:
                        break;
                }
            }

            /*writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(_UserFirstInput);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(_UserSecondInput);
            writer.WritePropertyName("Operation");*/

           /* 
            writer.WritePropertyName("Result");
            writer.WriteValue(_Result);
            writer.WriteEndObject();
            _Result = result;
            // Update use counter here? maybe not...*/
        

        // Function only used during division operation when trying to divide by zero
        private void GetValidDivisionNumber()
        {
            double userAnswer = 0;

            while (userAnswer == 0)
            {
                Messages.Print("Cannot divide by zero, please enter another number\n", Styles.ErrorStyle);
                userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter a number that's not zero:[/] ");
            }

            _LatestNumber = userAnswer;

        }

        internal void UpdateCurrentCalculation(double firstNumber, double secondNumber, string operation, double result)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{firstNumber.ToString()} ");
            sb.Append($" {operation} ");
            sb.Append($" {secondNumber.ToString()}");
            sb.Append($" = {result}");
            sb.Append(" -> "); // For seperating the next operation;
            _CurrentCalculation.Add(sb.ToString());
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

        // Check if operations can start
        internal bool CheckCanOperate()
        {
            if (!double.IsNaN(_Result) && !double.IsNaN(_LatestNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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
