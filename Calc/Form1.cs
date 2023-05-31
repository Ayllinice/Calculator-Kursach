using System.Globalization;

namespace Calc
{
    public partial class Calculator : Form
    {
        private string currentNumber = "";
        private string operation = "";
        private decimal result = 0;
        private bool isDecimal = false;

        public Calculator()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text == ".")
            {
                if (!isDecimal)
                {
                    isDecimal = true;
                    currentNumber += button.Text;
                    richTextBox1.Text += button.Text;
                }
            }
            else
            {
                currentNumber += button.Text;
                richTextBox1.Text += button.Text;
            }
        }

        private void OperatorButton_Click(object sender, EventArgs e)
        {

            //Button button = (Button)sender;
            //operation = button.Text;
            //result = decimal.Parse(currentNumber);
            //currentNumber = "";
            //richTextBox1.Text = richTextBox1.Text.TrimEnd() + " " + button.Text;
            Button button = (Button)sender;
            operation = button.Text;

            if (isDecimal)
            {
                decimal.TryParse(currentNumber, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
                isDecimal = false;
            }
            else
            {
                decimal.TryParse(currentNumber, out result);
            }

            currentNumber = "";
            richTextBox1.Text += button.Text;
        }

        private void EqualsButton_Click(object sender, EventArgs e)
        {
            decimal secondNumber;
            if (!decimal.TryParse(currentNumber, NumberStyles.Float, CultureInfo.InvariantCulture, out secondNumber))
            {
                MessageBox.Show("Invalid input!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string protocolEntry = $"{result} {operation} {secondNumber} = {result}\n";
            
            try
            {
                File.AppendAllText("protocol.txt", protocolEntry);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to write to protocol file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            switch (operation)
            {
                case "+":
                    result += secondNumber;
                    break;
                case "-":
                    result -= secondNumber;
                    break;
                case "*":
                    result *= secondNumber;
                    break;
                case "/":
                    if (secondNumber != 0)
                    {
                        result /= secondNumber;
                    }
                    else
                    {
                        MessageBox.Show("Cannot divide by zero!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        try
                        {
                            protocolEntry = $"↑ Cannot divide by zero!\n";
                            File.AppendAllText("protocol.txt", protocolEntry);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Failed to write to protocol file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }
                    break;
            }

            richTextBox1.Text = result.ToString(CultureInfo.InvariantCulture);
            currentNumber = result.ToString(CultureInfo.InvariantCulture);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.ClearUndo();
            result = 0;
            currentNumber = "";
            operation = "";
            try
            {
                if (File.Exists("protocol.txt"))
                {
                    File.Delete("protocol.txt");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete protocol file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void protocolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string protocolContent = File.ReadAllText("protocol.txt");
                MessageBox.Show(protocolContent, "Protocol", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open protocol file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.ClearUndo();
            result = 0;
            currentNumber = "";
            operation = "";
        }
    }
}