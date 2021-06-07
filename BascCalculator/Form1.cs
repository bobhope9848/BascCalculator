using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace BascCalculator
{
    /// <summary>
    /// A Very simple Calc
    /// </summary>
    public partial class Form1 : Form
    {


        #region Constructor
        /// <summary>
        /// Default construct
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        #endregion
        private void CEButton_Click(object sender, EventArgs e)
        {
            this.UserInputText.Text = "";
            FocusInputText();

        }

        private void DelButton_Click(object sender, EventArgs e)
        {
            var selectstart = this.UserInputText.SelectionStart;
            if (this.UserInputText.SelectionStart == 0 || (this.UserInputText.SelectionStart > this.UserInputText.Text.Length))
                return;
                this.UserInputText.Text = this.UserInputText.Text.Remove(this.UserInputText.SelectionStart - 1, 1);

            this.UserInputText.SelectionStart = selectstart;

            this.UserInputText.SelectionLength = 0;
            FocusInputText();
        }

        private void PercentButton_Click(object sender, EventArgs e)
        {
            InsertValue("%");
            FocusInputText();
        }

        private void SevenButton_Click(object sender, EventArgs e)
        {
            InsertValue("7");
            FocusInputText();

        }

        private void EightButton_Click(object sender, EventArgs e)
        {

            InsertValue("8");
            FocusInputText();
        }

        private void NineButton_Click(object sender, EventArgs e)
        {
            InsertValue("9");
            FocusInputText();
        }

        private void MultiButton_Click(object sender, EventArgs e)
        {
            InsertValue("*");
            FocusInputText();
        }

        private void FourButton_Click(object sender, EventArgs e)
        {
            InsertValue("4");
            FocusInputText();
        }

        private void FiveButton_Click(object sender, EventArgs e)
        {
            InsertValue("5");
            FocusInputText();
        }

        private void SixButton_Click(object sender, EventArgs e)
        {
            InsertValue("6");
            FocusInputText();
        }

        private void SubtractButton_Click(object sender, EventArgs e)
        {
            InsertValue("-");
            FocusInputText();
        }

        private void OneButton_Click(object sender, EventArgs e)
        {
            InsertValue("1");
            FocusInputText();
        }

        private void TwoButton_Click(object sender, EventArgs e)
        {
            InsertValue("2");
            FocusInputText();
        }

        private void ThreeButton_Click(object sender, EventArgs e)
        {
            InsertValue("3");
            FocusInputText();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            InsertValue("+");
            FocusInputText();
        }

        private void ZeroButton_Click(object sender, EventArgs e)
        {
            InsertValue("0");
            FocusInputText();
        }

        private void DecimalButton_Click(object sender, EventArgs e)
        {
            InsertValue(".");
            FocusInputText();
        }

        private void SumButton_Click(object sender, EventArgs e)
        {
            CalculateSum();
            FocusInputText();
        }

        

        #region Function
        private void FocusInputText()
        {
            this.UserInputText.Focus();
        }
        private void InsertValue(string set)
        {
            var selectstart = this.UserInputText.SelectionStart;
            this.UserInputText.Text = this.UserInputText.Text.Insert(this.UserInputText.SelectionStart, set);

            this.UserInputText.SelectionStart = selectstart + set.Length;

            this.UserInputText.SelectionLength = 0;
        }


        #endregion

        #region Number Method
        private void CalculateSum()
        {
            //var result = Parse();
            this.CalcResultText.Text = Parse();
            FocusInputText();
        }

        private string Parse()
        {
            try
            {
                var Input = this.UserInputText.Text;
                Input = Input.Replace(" ", "");
                var operation = new Operate();
                var leftSide = true;

                for (int i = 0; i < Input.Length; i++)
                {
                    if ("0123456789.".Any(c => Input[i] == c))
                    {
                        if (leftSide)
                        {
                            operation.LeftSide = AddNumber(operation.LeftSide, Input[i]);
                        }
                        else
                        {
                            operation.RightSide = AddNumber(operation.RightSide, Input[i]);
                        }
                    }
                    else if ("+-*/".Any(c => Input[i] == c))
                    {
                        if (!leftSide)
                        {
                            var operatorType = GetOperateType(Input[i]);
                            if (operation.RightSide.Length == 0)
                            {
                                if (operatorType != OpType.Minus)
                                {
                                    throw new InvalidOperationException($" OpType (+ * / or more than one -) specified without right side num");
                                }
                                else
                                {
                                    operation.RightSide += Input[i];
                                }    
                            }
                            else
                            {
                                operation.LeftSide = CalculateOp(operation);

                                operation.OpType = operatorType;

                                operation.RightSide = "";
                            }
                        }
                        else
                        {
                            var operatorType = GetOperateType(Input[i]);

                            if (operation.LeftSide.Length == 0 )
                            {
                                if (operatorType != OpType.Minus)
                                {
                                    throw new InvalidOperationException($" OpType (+ * / or more than one -) specified without left side num");
                                }
                                else
                                {
                                    operation.LeftSide += Input[i];
                                }
                            } 
                            else
                            {
                                operation.OpType = operatorType;
                                leftSide = false;
                            }
                        }
                    }
                }
                return CalculateOp(operation);
            }
            catch(Exception caught)
            {
                return $"Invalid equation. {caught.Message}";
            }
        }

        private string CalculateOp(Operate operation)
        {
            decimal left = 0;
            decimal right = 0;

            if (string.IsNullOrEmpty(operation.LeftSide) || !decimal.TryParse(operation.LeftSide, out left))
            {
                throw new InvalidOperationException($"Left side operation was not valid {operation.LeftSide}");
            }
            if (string.IsNullOrEmpty(operation.RightSide) || !decimal.TryParse(operation.RightSide, out right))
            {
                throw new InvalidOperationException($"Right side operation was not valid {operation.RightSide}");
            }
            try
            {
                switch (operation.OpType)
                {
                    case OpType.Add:
                        return (left + right).ToString();
                    case OpType.Minus:
                        return (left - right).ToString();
                    case OpType.Multiply:
                        return (left * right).ToString();
                    case OpType.Divide:
                        return (left / right).ToString();
                    default:
                        throw new InvalidOperationException($"Invalid operator type when calculating operation. {operation.OpType}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to calculate operation {operation.LeftSide} {operation.OpType} {operation.RightSide}. {ex.Message}");
            }

            return string.Empty;
        }

        private OpType GetOperateType(char character)
        {
            switch (character)
            {
                case '+':
                    return OpType.Add;
                case '-':
                    return OpType.Minus;
                case '*':
                    return OpType.Multiply;
                case '/':
                    return OpType.Divide;
                default:
                    throw new InvalidOperationException($"Invalid operator {character}");

            }
        }
        private string AddNumber(string currentNum, char currentChar)
        {
            if (currentChar == '.' && currentNum.Contains('.'))
            {
                throw new InvalidOperationException($"Number {currentNum} already has a .");
            }
            return currentNum + currentChar;

        }
        #endregion


    }



}
