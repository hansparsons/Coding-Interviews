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
            WebClient wc = new WebClient();
            string stockSymbol = textBox1.Text;
            string urlString = ("http://www.streetinsider.com/ec_earnings.php?q=" + stockSymbol);
            string stockFileName = "stocks\\" + stockSymbol + ".csv";

            //clear out the listViewBox
            listBox1.Items.Clear();

            // spaw a webclient scrap of the site and store it in a string
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

            // using yahoo finance create a data file that contains the stock history
            PullStockInfoIntoFile(stockSymbol, stockFileName);

            // variable declarations
            DateTime today = DateTime.Today;

            // Add the label titles to the listbox
            string labelString = "Date" + "\t" + "\t" + "Open" + "\t" + "High" + "\t" + "Low" + "\t" + "Close";
            listBox1.Items.Add(labelString);

            // we need to grab the stock price data associated with each of the earnings dates
            foreach (string earningsDate in listDates)
            {
                
                if (Convert.ToDateTime(earningsDate) < today) // check to make sure date is not later then today
                {


                    FileStream dataFile = File.Open(@stockFileName, FileMode.Open);
                    StreamReader dataStream = new StreamReader(dataFile);

                    string tempString;
                    string[] myArray;

                    while ((tempString = dataStream.ReadLine()) != null)
                    {

                        myArray = tempString.Split(',');

                        if (tempString.Substring(0, 4) == "Date")
                        {
                            //temp hack we need to skip the first row of lables
                        }
                        else if ((Convert.ToDateTime(myArray[0]) == Convert.ToDateTime(earningsDate)))
                        {
                            // we have a match
                            string listString = Convert.ToDateTime(myArray[0]).ToShortDateString() + "\t" + myArray[1].ToString() + "\t" + myArray[2].ToString() + "\t" + myArray[3].ToString() + "\t" + myArray[4].ToString();
                            listBox1.Items.Add(listString);

                        }
                    }
                    dataStream.Close();
                    dataFile.Close();
                }
            }

        }

        public void PullStockInfoIntoFile(string stockSymbol, string stockFileName)
        {
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
