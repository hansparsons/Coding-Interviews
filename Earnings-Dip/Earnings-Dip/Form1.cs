using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Earnings_Dip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // variable declarations

            string stockSymbol = textBox1.Text;
            string stockSymbol2 = textBox2.Text;
            string urlString = ("http://www.streetinsider.com/ec_earnings.php?q=" + stockSymbol);
            string urlString2 = ("http://www.streetinsider.com/ec_earnings.php?q=" + stockSymbol2);
            string stockFilePath = "stocks";
            string stockFileName = stockFilePath + "\\" + stockSymbol + ".csv";
            string stockFileName2 = stockFilePath + "\\" + stockSymbol2 + ".csv";

            //clear out the listBoxText
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            // using yahoo finance create a data file that contains the stock history
            PullStockInfoIntoFile(stockSymbol, stockFilePath, stockFileName);
            PullStockInfoIntoFile(stockSymbol2, stockFilePath, stockFileName2);

            List<string> listDates = BuildListOfDates(urlString);
            PopulateListBox(listDates, stockFileName,listBox1);

            List<string> listDates2 = BuildListOfDates(urlString2);
            PopulateListBox(listDates2, stockFileName2,listBox2);
        }

        public void PopulateListBox(List<string> listDates, string stockFileName, ListBox myListBox)
        {
            // variable declarations
            DateTime today = DateTime.Today;

            // Add the label titles to the listbox
            string labelString = "Date" + "\t" + "\t" + "Open" + "\t" + "High" + "\t" + "Low" + "\t" + "Close" + "\t" + "Close%";
            myListBox.Items.Add(labelString);

            // we need to grab the stock price data associated with each of the earnings dates
            foreach (string earningsDate in listDates)
            {

                if (Convert.ToDateTime(earningsDate) < today) // check to make sure date is not later then today
                {


                    FileStream dataFile = File.Open(@stockFileName, FileMode.Open);
                    StreamReader dataStream = new StreamReader(dataFile);

                    string tempString;
                    string[] myArray;
                    string listString;
                    string previousString;
                    string[] previousArray;


                    previousString = "";

                    while ((tempString = dataStream.ReadLine()) != null)
                    {

                        myArray = tempString.Split(',');

                        if (tempString.Substring(0, 4) == "Date")
                        {
                            //temp hack we need to skip the first row of lables
                        }
                        else if ((Convert.ToDateTime(myArray[0]) == Convert.ToDateTime(earningsDate)))
                        {

                            previousArray = previousString.Split(',');
                            // calculate close %
                            decimal closePercent = (((Convert.ToDecimal(previousArray[4])) - (Convert.ToDecimal(myArray[4]))) / (Convert.ToDecimal(previousArray[4]))) * 100;
                            closePercent = Math.Round(closePercent,2);

                            // we have a match - list the day off earning day stock info
                            listString = Convert.ToDateTime(previousArray[0]).ToShortDateString() + "\t" + previousArray[1].ToString() + "\t" + previousArray[2].ToString() + "\t" + previousArray[3].ToString() + "\t" + previousArray[4].ToString() + "\t" + closePercent.ToString();
                            myListBox.Items.Add(listString);

                            // we have a match - list the day off earning day stock info
                            listString = Convert.ToDateTime(myArray[0]).ToShortDateString() + "\t" + myArray[1].ToString() + "\t" + myArray[2].ToString() + "\t" + myArray[3].ToString() + "\t" + myArray[4].ToString();
                            myListBox.Items.Add(listString);

                        }

                        previousString = tempString;
                    }
                    dataStream.Close();
                    dataFile.Close();
                }
            }
        }
        public void PullStockInfoIntoFile(string stockSymbol, string stockFilePath, string stockFileName)
        {
            //ensure the stocks file path exists
            try
            {
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(stockFilePath))
                {
                    Directory.CreateDirectory(stockFilePath);
                }
            }
            catch (Exception)
            {
                // Fail silently
            }

            // kick off a webclient that returns all the stock info into a file
            WebClient webClient = new WebClient();
            
            string uriString = "http://ichart.yahoo.com/table.csv?s=" + stockSymbol;
            Uri url = new Uri(@uriString);

            try
            {
                webClient.DownloadFile(url, @stockFileName);
                // To copy a file to another location and overwrite the destination file if it already exists.
                System.IO.File.Copy(@stockFileName, stockSymbol + ".csv", true);
            }
            catch (WebException ex)
            {
                //something went wrong
                MessageBox.Show(ex.ToString());
            }
        }
 
        public List<string> BuildListOfDates(string urlString)
        {
            WebClient wc = new WebClient();

            // spawn a webclient scrape of the site and store it in a string
            byte[] raw = wc.DownloadData(urlString);
            string input = System.Text.Encoding.UTF8.GetString(raw);

            // using regex pair down the into the areas where the dates are located    
            MatchCollection matchesDates = Regex.Matches(input, @"<td>(.*?)/></td>");
            
            // build a list that contains all the dates         
            List<string> listDates = new List<string>();
                 
            foreach (Match match in matchesDates)
            {
                if (((match.Value.Substring(0, 4)) == "<td>")) //if true i'm pretty sure a valid date is next
                     {
                         listDates.Add (Regex.Match(match.Value, @"<td>(.*?)<img src").Groups[1].Value);
                     }  
            }
            return listDates;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
