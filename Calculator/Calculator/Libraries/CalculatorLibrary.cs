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
        internal JsonWriter writer;
        internal Enum _OperationChoice { get; private set; } = Enums.Default.Default;
        internal double _LatestNumber { get; private set; } = double.NaN;
        internal double _Result { get; private set; } = double.NaN;
        internal List<string[]> _CurrentCalculation { get; private set; } = new List<string[]>();

        internal List<List<string[]>> _Calculations { get; private set; } = new List<List<string[]>>();

        internal bool _Calculating { get; private set; } = false;

        internal int CalculationCount { get; private set; } = 1;

        public CalculatorLibrary()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Calculator Program");
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
                    CalculationCount += 1;
                    EndCalculating();
                }
            }
        }

        // Function for singular input.
        internal void GetInput()
        {
            writer.WriteStartObject();
            double userAnswer = AnsiConsole.Ask<double>($"[{Styles.TextColorStyle}]Enter a number:[/]");
            if (double.IsNaN(userAnswer))
            {
                throw new InvalidDataException("InvalidDataExcpetion: User input was NaN.");
            }
            else if (double.IsNaN(_Result))
            {
                // If no first number has been input, assign it
                _Result = userAnswer;
                writer.WritePropertyName("Initial Number");
                writer.WriteValue(_Result);
            }
            else
            {
                // Assign every other number to this current number.
                _LatestNumber = userAnswer;
                writer.WritePropertyName("Latest Number");
                writer.WriteValue(_LatestNumber);
            }
            writer.WriteEndObject();
        }

        // Function to do the operation
        internal void DoOperation()
        {
            writer.WriteStartObject();
            double temp = 0;
                switch (_OperationChoice)
                {
                    case Enums.OperationChoice.Add:
                        writer.WritePropertyName("Operation");
                        writer.WriteValue("Add");
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            temp = _Result;
                            _Result = _Result + _LatestNumber;
                            UpdateCurrentCalculation(temp, _LatestNumber, "+", _Result);
                            writer.WritePropertyName("Result");
                            writer.WriteValue(_Result);
                            writer.WriteEndObject();
                        }
                        break;
                    case Enums.OperationChoice.Subtract:
                        writer.WritePropertyName("Operation");
                        writer.WriteValue("Subtract");
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            temp = _Result;
                            _Result = _Result - _LatestNumber;
                            UpdateCurrentCalculation(temp, _LatestNumber, "-", _Result);
                            writer.WritePropertyName("Result");
                            writer.WriteValue(_Result);
                            writer.WriteEndObject();
                        }
                        break;
                    case Enums.OperationChoice.Multiply:
                        writer.WritePropertyName("Operation");
                        writer.WriteValue("Multiply");
                        if (double.IsNaN(_Result))
                        {
                            throw new NotFiniteNumberException("NotFiniteNumberException: Number is NaN");
                        }
                        else
                        {
                            temp = _Result;
                            _Result = _Result * _LatestNumber;
                            UpdateCurrentCalculation(temp, _LatestNumber, "*", _Result);
                            writer.WritePropertyName("Result");
                            writer.WriteValue(_Result);
                            writer.WriteEndObject();
                        }
                        
                        break;
                    case Enums.OperationChoice.Divide:
                        writer.WritePropertyName("Operation");
                        writer.WriteValue("Divide");
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
                                    writer.WritePropertyName("Result");
                                    writer.WriteValue(_Result);
                                    writer.WriteEndObject();
                                }
                                else
                                {
                                    GetValidDivisionNumber();
                                    temp = _Result;
                                    _Result = _Result / _LatestNumber;
                                    UpdateCurrentCalculation(temp, _LatestNumber, "/", _Result);
                                    writer.WritePropertyName("Result");
                                    writer.WriteValue(_Result);
                                    writer.WriteEndObject();
                                }
                            }
                        break;
                    case Enums.OperationChoice.Finish_Calculation:
                        //FinishJsonWriterCalculation();
                        EndCalculating();
                        break;
                default:
                        break;
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
            _LatestNumber = double.NaN;
        }

        internal void DeleteCalculations()
        {
            _Calculations.Clear();
        }

        public void FinishJsonWriterCalculation()
        {
            //Finish current array
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public void FinishJsonWriter()
        {
            // Finish parent object
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
