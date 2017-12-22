using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic.FileIO;
using System.Collections;

namespace LitSupport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filename;
        string[,] values;

        public MainWindow()
        {
            InitializeComponent();
        }

        private Boolean MultipleEmailsCheck(string s)
        {
            if(s.Contains(";"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string[] SplitDomains(string s)
        {
            string[] splitString = s.Split(';');
            return splitString;
        }

        public char ConvertStringChar(string stringVal)
        {
            char charVal = 'a';

            // A string must be one character long to convert to char.
            try
            {
                charVal = System.Convert.ToChar(stringVal);
                //System.Console.WriteLine("{0} as a char is {1}",
                //    stringVal, charVal);
            }
            catch (System.FormatException)
            {
                System.Console.WriteLine(
                    "The string is longer than one character.");
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("The string is null.");
            }
            return charVal;
        }

        private void openCsvButton_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".csv";
            dlg.Filter = "Comma Separated Values (.csv)|*.csv";
            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
            }
            //read the selected csv into an array or two, depending on whether there's a header row
            ParseCSV(filename);
        }

        // Load a CSV file into an array of rows and columns.
        // Assume there may be blank lines but every line has
        // the same number of fields.
        private string[,] LoadCsv(string filename, char delimiter)
        {
            // Get the file's text.
            string whole_file = System.IO.File.ReadAllText(filename);

            // Split into lines.
            whole_file = whole_file.Replace('\n', '\r');
            whole_file = whole_file.Replace("\"", "");
            string[] lines = whole_file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(delimiter).Length;

            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(delimiter);
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }

            // Return the values.
            return values;
        }

        private ArrayList GetList(string[,] values)
        {
            int docID = 0;
            int to = 0;
            int from = 0;
            int cc = 0;
            int bcc = 0;
            ArrayList list = new ArrayList();
            //find column titles to line up index numbers within the array
            for (int c = 0; c < values.GetUpperBound(1) + 1; c++)
            {
                if (values[0, c].Equals("docid", StringComparison.CurrentCultureIgnoreCase))
                {
                    docID = c;
                }
                else if (values[0, c].Equals("to", StringComparison.CurrentCultureIgnoreCase))
                {
                    to = c;
                }
                else if (values[0, c].Equals("from", StringComparison.CurrentCultureIgnoreCase))
                {
                    from = c;
                }
                else if (values[0, c].Equals("cc", StringComparison.CurrentCultureIgnoreCase))
                {
                    cc = c;
                }
                else if (values[0, c].Equals("bcc", StringComparison.CurrentCultureIgnoreCase))
                {
                    bcc = c;
                }
            }

            for (int r = 1; r < values.GetUpperBound(0) + 1; r++)
                {
                    list.Add(new TableRecord(values[r, docID], values[r, to], values[r, from], values[r, cc], values[r, bcc]));
                }
                         
            return list;
        }

        private void ParseCSV(string filename)
        {
            bool b = (Boolean)hasHeaderRow.IsChecked;
            char delimiterChar = ConvertStringChar(delimiterType.Text);

            // Get the data.
            values = LoadCsv(filename, delimiterChar);
            int num_rows = values.GetUpperBound(0) + 1;
            int num_cols = values.GetUpperBound(1) + 1;

            // Display the data to show we have it.

            ArrayList list = GetList(values);
            dataGrid.ItemsSource = list;

            //read the arrays to search for the domain values within To, CC, and BCC fields should they exist
        }

        private void checkAllDomains(string[,] values)
        { 
            //compare the various fields to see if values other than the requested domain exist
            //values will be separated by a semi-colon if there's more than one entry in a field
            string domain = domainText.Text;
            

        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            checkAllDomains(values);

        }
    }
}
