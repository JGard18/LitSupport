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

namespace LitSupport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filename;
        private int fieldCount = 0;
        private int recordCount = 0;

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

        private Boolean ContainsEmailDomain(string s, List<string> l)
        {
            if(l.IndexOf(s) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            ParseCSV();
        }

        private void ParseCSV()
        {
            int to;
            int cc;
            int bcc;
            int from;

            using (TextFieldParser csvParser = new TextFieldParser(filename))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                bool? b = hasHeaderRow.IsChecked;
                List<string> fieldNames = new List<string>();
                List<List<string>> fieldInfo = new List<List<string>>();
                if (b == true)
                {
                    //read first line to get variable names and then read rest into array
                    string[] fields = csvParser.ReadFields();
                    
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fieldNames.Add(fields[i]);
                    }
                    //then read the rest of the CSV file into its own array
                    while (!csvParser.EndOfData)
                    {
                        int j = 0;
                        // Read current line fields, pointer moves to the next line.
                        fields = csvParser.ReadFields();
                        //load into 2D array
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fieldInfo[j].Add(fields[i]);
                        }
                        j++;
                    }
                }
                else
                {
                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();
                        //fieldCount = fields.Length;
                        //counter for while loop and loading into array
                        int j = 0;
                        //load into 2D array
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fieldInfo[j].Add(fields[i]);
                        }
                        j++;
                    }
                }
                
                //read the arrays to search for the domain values within To, CC, and BCC fields should they exist
                
                if (fieldNames != null)
                {
                    to = fieldNames.IndexOf("to");
                    cc = fieldNames.IndexOf("cc");
                    bcc = fieldNames.IndexOf("bcc");
                    from = fieldNames.IndexOf("from");
                }
                else
                {
                    to = -1;
                    cc = -1;
                    bcc = -1;
                    from = -1;
                }
                //compare the various fields to see if values other than the requested domain exist
                //values will be separated by a semi-colon if there's more than one entry in a field
                string domain = domainText.Text;
                List<List<string>> tempArray = new List<List<string>>();
                List<List<string>> finalArray = new List<List<string>>();
                int recordCount = fieldInfo[0].Count;
                fieldCount = fieldInfo.Count;

                //iterate through the field info array, searching for the desired domain name within the to/from/cc field
                //if they're known.  otherwise search the whole thing
                for (int record = 0; record < recordCount; record++)
                {
                    for(int field = 0; field < fieldCount; field++)
                    {
                        if (ContainsEmailDomain(domain, fieldInfo[record]))
                        {
                            tempArray[record][field] = fieldInfo[record][field];
                        }
                        //now we have new, smaller, array for items that have the domain in question present somewhere

                        //all fields exist in the header so we can target search the fields for the domain where it must be in at least
                        //from and one of the other fields
                        if (to >= 0 && cc >= 0 && bcc >= 0 && from >= 0)
                        {
                            //check that the domain is in the from field
                            if(tempArray[record][from].Contains(domain))
                            {
                                //if found, then check To, since we want inter-company email, or it can be empty
                                if (tempArray[record][to].Contains(domain))
                                {
                                    //if found, then move to CC field
                                    if (tempArray[record][cc].Contains(domain))
                                    {
                                        //if found, then continue to BCC field
                                        if (tempArray[record][bcc].Contains(domain))
                                        {
                                            //check for multiple address in this field
                                            if (MultipleEmailsCheck(tempArray[record][bcc]))
                                            {
                                                string[] tempList = SplitDomains(tempArray[record][bcc]);
                                                //need to now search this array for values not matching the domain requested
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //bcc is missing from the header
                        else if (to >= 0 && cc >= 0 && bcc == -1 && from >= 0)
                        {

                        }
                        //cc and bcc are missing from the header
                        else if (to >= 0 && cc == -1 && bcc == -1 && from >= 0)
                        {

                        }
                        //the rest of the potential combinations, involving searching the entire table
                        else
                        {

                        }
                    }
                    
                }





            }
        }
    }
}
