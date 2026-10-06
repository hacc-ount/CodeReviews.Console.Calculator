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
        internal List<string[]> _CurrentCalculation { get; private set; } = new List<string[]>();

        internal List<List<string[]>> _Calculations { get; private set; } = new List<List<string[]>>();

        internal bool _Calculating { get; private set; } = false;

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
                if (Enum.Equals(Enums.OperationChoice.Finish_Calculation, menuChoice))
                {
                    UpdateOverallCalculations(); // Update the overall calculations list
                    _CurrentCalculation = new List<string[]>(); // Reset the current calculation
                    EndCalculating();
                }
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
                    case Enums.OperationChoice.Finish_Calculation:
                        
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

        // Update the current calculation (create an array and add it to a list).
        internal void UpdateCurrentCalculation(double firstNumber, double secondNumber, string operation, double result)
        {
            string[] calculation = new string[4];
            //Add each part of the calculation to the array
            calculation[0] = firstNumber.ToString();
            calculation[1] = operation;
            calculation[2] = secondNumber.ToString();
            calculation[3] = result.ToString();

            _CurrentCalculation.Add(calculation);
        }

        // Update the overall calculation used in program run time
        private void UpdateOverallCalculations()
        {
            _Calculations.Add(_CurrentCalculation);
        }

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

        internal void StartCalculating()
        {
            _Calculating = true;
        }

        internal void EndCalculating()
        {
            _Calculating = false;
            _Result = double.NaN;
        }

        public void FinishJsonWriter()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
