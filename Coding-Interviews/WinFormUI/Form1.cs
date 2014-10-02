using System;
using System.Windows.Forms;
using Coding_Interviews_Lib;

namespace WinFormUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ReverseStringWithoutCollectionsButtonClick(object sender, EventArgs e)
        {
            OutputTextBox.AppendText("\n");
            OutputTextBox.AppendText(Answers.ReverseStringWithoutCollections(Input1.Text));

        }

        private void ReverseStringButtonClick(object sender, EventArgs e)
        {
            OutputTextBox.AppendText("\n");
            OutputTextBox.AppendText(Answers.ReverseString(Input1.Text));
        }

        private void MultiplyButtonClick(object sender, EventArgs e)
        {
            Int32 input1;   //multiplicand
            Int32 input2;   //multiplier
            if (!Int32.TryParse(Input1.Text, out input1))
                MessageBox.Show("Invalid First Number Input", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (!Int32.TryParse(Input2.Text, out input2))
                MessageBox.Show("Invalid Second Number Input", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            OutputTextBox.AppendText("\n");
            OutputTextBox.AppendText(Answers.MultiplyBinary(input1, input2).ToString());
        }

        private void Input1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
