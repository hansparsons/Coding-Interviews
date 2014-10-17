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
using System.Windows.Forms.DataVisualization.Charting;

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

            //clear the data in the listBoxes
            listBox1.Items.Clear();
            listBox2.Items.Clear();

            // clear the data in the DataGridViews
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            // clear the data in the charts
            chart1.Series["Close %"].Points.Clear();
            chart1.Series["AHM %"].Points.Clear();
            chart1.Series["Max %"].Points.Clear();
            chart1.Refresh();

            // clear the data in the charts
            chart2.Series["Close %"].Points.Clear();
            chart2.Series["AHM %"].Points.Clear();
            chart2.Series["Max %"].Points.Clear();
            chart2.Refresh();

            // using yahoo finance create a data file that contains the stock history
            PullStockInfoIntoFile(stockSymbol, stockFilePath, stockFileName);
            PullStockInfoIntoFile(stockSymbol2, stockFilePath, stockFileName2);

            List<string> listDates = BuildListOfDates(urlString);
            PopulateListBox(listDates, stockFileName,listBox1, chart1, dataGridView1);
            PopulateSevenDayChart( listDates, stockFileName, chart3);


            List<string> listDates2 = BuildListOfDates(urlString2);
            PopulateListBox(listDates2, stockFileName2,listBox2, chart2, dataGridView2);
            PopulateSevenDayChart(listDates2, stockFileName2, chart4);
        }

        public void PopulateSevenDayChart(List<string> listDates, string stockFileName, Chart myChart)
        {
            // variable declarations
            DateTime today = DateTime.Today;
            DateTime latestEarningsDate = DateTime.Today;
            int i = 0;
            string tempString;
            string[] tempArray;
            
            // find the latest valid earnings date
            do
            {
                latestEarningsDate = Convert.ToDateTime(listDates[i]);
                i++;
            } while (Convert.ToDateTime(listDates[i]) > today);

            // open the stock file for read
            FileStream dataFile = File.Open(@stockFileName, FileMode.Open);
            StreamReader dataStream = new StreamReader(dataFile);

            // create a historical DataTable of all the stock close price
            DataTable historicalClosePrice = new DataTable("ClosePrice");
            historicalClosePrice.Columns.Add("Date", typeof(DateTime));
            historicalClosePrice.Columns.Add("HighPrice", typeof(decimal));
            historicalClosePrice.Columns.Add("LowPrice", typeof(decimal));
            historicalClosePrice.Columns.Add("ClosePrice", typeof(decimal));

            //fill the historicalDataTable with close stock price
            while ((tempString = dataStream.ReadLine()) != null)
            {
                tempArray = tempString.Split(',');

                if (tempString.Substring(0, 4) == "Date")
                        {
                    //temp hack we need to skip the first row of lables
                }
                else 
                {

                    historicalClosePrice.Rows.Add(Convert.ToDateTime(tempArray[0]).ToShortDateString(), (Convert.ToDecimal(tempArray[2])), (Convert.ToDecimal(tempArray[3])), (Convert.ToDecimal(tempArray[4])));
                }

            }

            //close the stock file
            dataStream.Close();
            dataFile.Close();

            int datesMatch = -1;
            int indexOfEarningsDate = 0;

            DateTime foo3 = DateTime.Today;

             // we now need to find the index of the lastEarningsDate in the historicalClosePrice table
            foreach (DataRow row in historicalClosePrice.Rows)
            {
                foo3 = (Convert.ToDateTime(row.ItemArray[0]));
                datesMatch = DateTime.Compare(foo3, latestEarningsDate);

                if ( datesMatch == 0)
                {
                    
                    indexOfEarningsDate = historicalClosePrice.Rows.IndexOf(row);
                }
            }

            // create a DataTable that contains the pre and post 7 day stock prices
            // create a historical DataTable of all the stock close price
            DataTable sevenDayClosePrice = new DataTable("SevenDayClosePrice");
            sevenDayClosePrice.Columns.Add("Date", typeof(DateTime));
            sevenDayClosePrice.Columns.Add("HighPrice", typeof(decimal));
            sevenDayClosePrice.Columns.Add("LowPrice", typeof(decimal));
            sevenDayClosePrice.Columns.Add("ClosePrice", typeof(decimal));

            for (int sevenday = 7; sevenday > 0; sevenday--)
            {
                sevenDayClosePrice.Rows.Add(historicalClosePrice.Rows[58 - sevenday]["Date"], historicalClosePrice.Rows[58 - sevenday]["HighPrice"].ToString(), historicalClosePrice.Rows[58 - sevenday]["LowPrice"].ToString(), historicalClosePrice.Rows[58 - sevenday]["ClosePrice"].ToString()); 
            }
            sevenDayClosePrice.Rows.Add(historicalClosePrice.Rows[58]["Date"], historicalClosePrice.Rows[58]["HighPrice"].ToString(), historicalClosePrice.Rows[58]["LowPrice"].ToString(), historicalClosePrice.Rows[58]["ClosePrice"].ToString());

            for (int sevenday = 1; sevenday < 8; sevenday++)
            {
                sevenDayClosePrice.Rows.Add(historicalClosePrice.Rows[58 + sevenday]["Date"], historicalClosePrice.Rows[58 + sevenday]["HighPrice"].ToString(), historicalClosePrice.Rows[58 + sevenday]["LowPrice"].ToString(), historicalClosePrice.Rows[58 + sevenday]["ClosePrice"].ToString());
            }

            // bind the sevenDayClosePrice DataTable to chart3
            
            myChart.Series["High"].XValueMember = "Date";
            myChart.Series["High"].YValueMembers = "HighPrice";

            myChart.Series["Low"].XValueMember = "Date";
            myChart.Series["Low"].YValueMembers = "LowPrice";

            myChart.Series["Close"].XValueMember = "Date";
            myChart.Series["Close"].YValueMembers = "ClosePrice";

            myChart.DataSource = sevenDayClosePrice;
            myChart.DataBind();

           
        }

        public void PopulateListBox(List<string> listDates, string stockFileName, ListBox myListBox, Chart myChart, DataGridView myDataGridView)
        {
            // variable declarations
            DateTime today = DateTime.Today;

            // Add the label titles to the listbox
            string labelString = "Date" + "\t" + "\t" + "Open" + "\t" + "High" + "\t" + "Low" + "\t" + "Close" + "\t" + "Close%" + "\t" + "AHM%" + "\t" + "Max%";
            myListBox.Items.Add(labelString);

            // create the chart data table
            DataTable chartTable = new DataTable();
            chartTable.Columns.Add("Date", typeof(DateTime));
            chartTable.Columns.Add("Close", typeof(double));
            chartTable.Columns.Add("AHM", typeof(double));
            chartTable.Columns.Add("Max", typeof(double));

            // we need to grab the stock price data associated with each of the earnings dates
            foreach (string earningsDate in listDates)
            {

                //System.DateTime x = new System.DateTime(2008, 11, 21);
                //myChart.Series[0].Points.AddXY(Convert.ToDateTime(previousArray[0]), (Convert.ToDouble(closePercent)), (Convert.ToDouble(AHM)), (Convert.ToDouble(Max)));


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
                            //decimal closePercent = (((Convert.ToDecimal(previousArray[4])) - (Convert.ToDecimal(myArray[4]))) / (Convert.ToDecimal(previousArray[4]))) * 100;
                            decimal closePercent = (((Convert.ToDecimal(previousArray[4])) / (Convert.ToDecimal(myArray[4]))) -1 ) * 100;
                            closePercent = Math.Round(closePercent,2);

                            // calculate the After Hours Move
                            decimal AHM = (((Convert.ToDecimal(previousArray[1])) / (Convert.ToDecimal(myArray[4]))) - 1 ) * 100 ;
                            AHM = Math.Round(AHM, 2);

                            // calculate the Max Percentage
                            decimal Max = ((Convert.ToDecimal(previousArray[2])) / (Convert.ToDecimal(myArray[4])) -1 ) * 100;
                            Max = Math.Round(Max, 2);

                            // Here we add five DataRows.
                            chartTable.Rows.Add(Convert.ToDateTime(earningsDate), (Convert.ToDouble(closePercent)), (Convert.ToDouble(AHM)), (Convert.ToDouble(Max)));

                            //myChart.Series["Close %"].Points.Add(Convert.ToDouble(closePercent));
                            //myChart.Series["AHM %"].Points.Add(Convert.ToDouble(AHM));
                            //myChart.Series["Max %"].Points.Add(Convert.ToDouble(Max));

                            // we have a match - list the day off earning day stock info
                            listString = Convert.ToDateTime(previousArray[0]).ToShortDateString() + "\t" + previousArray[1].ToString() + "\t" + previousArray[2].ToString() + "\t" + previousArray[3].ToString() + "\t" + previousArray[4].ToString() + "\t" + closePercent.ToString() + "\t" + AHM.ToString() + "\t" + Max.ToString();
                            myListBox.Items.Add(listString);

                            // we have a match - list the day off earning day stock info
                            listString = Convert.ToDateTime(myArray[0]).ToShortDateString() + "\t" + myArray[1].ToString() + "\t" + myArray[2].ToString() + "\t" + myArray[3].ToString() + "\t" + myArray[4].ToString();
                            myListBox.Items.Add(listString);

                            // add a row of data to the grid view
                            int n = myDataGridView.Rows.Add();

                            myDataGridView.Rows[n].Cells[0].Value = Convert.ToDateTime(previousArray[0]).ToShortDateString();
                            myDataGridView.Rows[n].Cells[1].Value = previousArray[1];
                            myDataGridView.Rows[n].Cells[2].Value = previousArray[2];
                            myDataGridView.Rows[n].Cells[3].Value = previousArray[3];
                            myDataGridView.Rows[n].Cells[4].Value = previousArray[4];

                            myDataGridView.Rows[n].Cells[5].Value = closePercent;
                            if (closePercent < 0)
                            { myDataGridView.Rows[n].Cells[5].Style.ForeColor = Color.Red; }
                            else { myDataGridView.Rows[n].Cells[5].Style.ForeColor = Color.Green; }

                            myDataGridView.Rows[n].Cells[6].Value = AHM.ToString();
                            if (AHM < 0)
                            { myDataGridView.Rows[n].Cells[6].Style.ForeColor = Color.Red; }
                            else { myDataGridView.Rows[n].Cells[6].Style.ForeColor = Color.Green; }

                            myDataGridView.Rows[n].Cells[7].Value = Max.ToString();
                            if (Max < 0)
                            { myDataGridView.Rows[n].Cells[7].Style.ForeColor = Color.Red; }
                            else { myDataGridView.Rows[n].Cells[7].Style.ForeColor = Color.Green; }

                            // add a row of data to the grid view
                            int x = myDataGridView.Rows.Add();
                            myDataGridView.Rows[x].Cells[0].Value = Convert.ToDateTime(myArray[0]).ToShortDateString();
                            myDataGridView.Rows[x].Cells[1].Value = myArray[1];
                            myDataGridView.Rows[x].Cells[2].Value = myArray[2];
                            myDataGridView.Rows[x].Cells[3].Value = myArray[3];
                            myDataGridView.Rows[x].Cells[4].Value = myArray[4];

                        }

                        previousString = tempString;
                    }
                    dataStream.Close();
                    dataFile.Close();

                }
            }
            myChart.Series[0].XValueType = ChartValueType.DateTime;
            myChart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            myChart.ChartAreas[0].AxisX.Interval = 3;
            myChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            myChart.ChartAreas[0].AxisX.IntervalOffset = 1;
            
            DateTime minDate = Convert.ToDateTime(listDates.Last());
            DateTime maxDate = Convert.ToDateTime(listDates.First());
            myChart.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
            myChart.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();
            

            myChart.DataSource = chartTable;
            myChart.Series["Close %"].XValueMember = "Date";
            myChart.Series["AHM %"].XValueMember = "Date";
            myChart.Series["Max %"].XValueMember = "Date";
            myChart.Series["Close %"].YValueMembers = "Close";
            myChart.Series["AHM %"].YValueMembers = "AHM";
            myChart.Series["Max %"].YValueMembers = "Max";
            myChart.DataBind();

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

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
