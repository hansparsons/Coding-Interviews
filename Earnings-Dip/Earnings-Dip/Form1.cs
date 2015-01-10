using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;
using System.Data.SqlClient;

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

            // clear the data in the charts
            chart3.Series["Close"].Points.Clear();
            chart3.Series["High"].Points.Clear();
            chart3.Series["Low"].Points.Clear();

            chart3.Refresh();

            // clear the data in the charts
            chart4.Series["Close"].Points.Clear();
            chart4.Series["High"].Points.Clear();
            chart4.Series["Low"].Points.Clear();
            chart4.Refresh();

            // using yahoo finance create a data file that contains the stock history
            PullStockInfoIntoFile(stockSymbol, stockFilePath, stockFileName);
            PullStockInfoIntoFile(stockSymbol2, stockFilePath, stockFileName2);

            List<string> listDates = BuildListOfDates(urlString, stockSymbol);
            if (listDates.Count() != 0)
            {
                PopulateListBox(listDates, stockFileName, listBox1, chart1, dataGridView1);
                PopulateSevenDayChart(listDates, stockFileName, chart3);
            }

            List<string> listDates2 = BuildListOfDates(urlString2, stockSymbol2);
            if (listDates2.Count() != 0)
            {
                PopulateListBox(listDates2, stockFileName2, listBox2, chart2, dataGridView2);
                PopulateSevenDayChart(listDates2, stockFileName2, chart4);
            }

            ImportPriceDataIntoSQL(stockSymbol);
            CalculateMovingAverages();
            PoplulateTwoHundredDayChartFromSQL();
            PoplulateFiftyDayChartFromSQL();
            DateTime latestEarningsDate = ReturnLastValidEarningsDate(listDates);
        }

        public void CalculateMovingAverages()
        {

            int y;

            for (int x = 0; x < 201; x++)
            {
                y = x + 199;
                string sqlstring = "UPDATE [Table] SET MovingDayAveTwo=(SELECT AVG(Adj_Close) AS MovingAverage FROM [Table] WHERE StockQuote_Id BETWEEN " + Convert.ToString(x) + " AND " + Convert.ToString(y) + ") WHERE StockQuote_Id=" + Convert.ToString(x);

                using (SqlConnection connection = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString))

                using (SqlCommand command = new SqlCommand(sqlstring, connection))
                {
                    connection.Open();
                    // Todo: figure out how to fill out rows accurately
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            for (int x = 0; x < 51; x++)
            {
                y = x + 49;
                string sqlstring = "UPDATE [Table] SET MovingDayAveFifty=(SELECT AVG(Adj_Close) AS MovingAverage FROM [Table] WHERE StockQuote_Id BETWEEN " + Convert.ToString(x) + " AND " + Convert.ToString(y) + ") WHERE StockQuote_Id=" + Convert.ToString(x);

                using (SqlConnection connection = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString))

                using (SqlCommand command = new SqlCommand(sqlstring, connection))
                {
                    connection.Open();
                    // Todo: figure out how to fill out rows accurately
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

        }

        public void ImportPriceDataIntoSQL(string stockSymbol)
        {
            SqlConnection conn = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString);
            string sqlTrunc = "TRUNCATE TABLE [Table]";
            SqlCommand cmd = new SqlCommand(sqlTrunc, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            string stockTempString = String.Empty;
            string[] stockTempArray;

            // open the stock file for read
            FileStream dataFile = File.Open(stockSymbol + ".csv", FileMode.Open);
            StreamReader dataStream = new StreamReader(dataFile);
            SqlConnection cn = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString);
            int x = -1;

            try
            {
                cn.Open();
                //fill the historicalDataTable with close stock price
                while ((stockTempString = dataStream.ReadLine()) != null)
                {
                    x += 1;

                    stockTempArray = stockTempString.Split(',');

                    if (stockTempString.Substring(0, 4) == "Date")
                    {
                        //temp hack we need to skip the first row of lables
                    }
                    else
                    {
                        string sql = "INSERT INTO [Table] (StockQuote_Id, StockDate, OpenPrice, HighPrice, LowPrice, ClosePrice, Volume, Adj_Close, StockQuote) VALUES(@stockName_Id, @stockDate , @openPrice ,@highPrice, @lowPrice ,@closePrice, @volume, @adj_Close, @stockQuote)";

                        SqlCommand exeSql = new SqlCommand(sql, cn);

                        exeSql.Parameters.AddWithValue("@stockName_Id", x);
                        exeSql.Parameters.AddWithValue("@stockDate", Convert.ToDateTime(stockTempArray[0]).ToShortDateString());
                        exeSql.Parameters.AddWithValue("@openPrice", (Convert.ToDecimal(stockTempArray[1])));
                        exeSql.Parameters.AddWithValue("@highPrice", (Convert.ToDecimal(stockTempArray[2])));
                        exeSql.Parameters.AddWithValue("@lowPrice", (Convert.ToDecimal(stockTempArray[3])));
                        exeSql.Parameters.AddWithValue("@closePrice", (Convert.ToDecimal(stockTempArray[4])));
                        exeSql.Parameters.AddWithValue("@volume", (Convert.ToDecimal(stockTempArray[5])));
                        exeSql.Parameters.AddWithValue("@adj_Close", (Convert.ToDecimal(stockTempArray[6])));
                        exeSql.Parameters.AddWithValue("@stockQuote", stockSymbol);

                        exeSql.ExecuteNonQuery();
                    }

                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {


                // TODO: This line of code loads data into the 'stockPriceDBDataSet.Table' table. You can move, or remove it, as needed.
                this.tableTableAdapter.Fill(this.stockPriceDBDataSet.Table);
                //this.tableTableAdapter.Update(StockPriceDBDataSet.TableDataTable )



                // close the sql db
                cn.Close();

                //close the stock file
                dataStream.Close();
                dataFile.Close();
            }

        }

        public DateTime ReturnLastValidEarningsDate(List<string> listDates)
        {
            DateTime today = DateTime.Today;

            foreach (string date in listDates)
            {
                if (Convert.ToDateTime(date) < today)
                {
                    return Convert.ToDateTime(date);
                }

            }

            return today; // todo: this is an error case that i need to handle
        }
        public void PoplulateLastEarningsChartFromSQL( DateTime lastEarningsDate)
        {
            // Create a connection to the "pubs" SQL database located on the 

            // local computer.
            SqlConnection myConnection = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString);

            //SqlConnection myConnection = new SqlConnection("server=localhost;" +                    "database=pubs;Trusted_Connection=Yes");

            // Connect to the SQL database using a SQL SELECT query to get all 
            // the data from the "Authors" table.

            //string sqlStr = "SELECT StockDate, ClosePrice FROM [Table] WHERE StockDate >= DateAdd(month, -6, getdate())";
            string sqlString = "SELECT StockQuote_Id, StockDate, ClosePrice, HighPrice, LowPrice, MovingDayAveFifty FROM [Table] WHERE StockQuote_Id <= 50";
            SqlDataAdapter myCommand = new SqlDataAdapter(sqlString, myConnection);

            // Create and fill a DataSet.
            DataSet ds = new DataSet();
            myCommand.Fill(ds);

            DataView source = new DataView(ds.Tables[0]);
            FiftyDay.DataSource = source;

            // set series members names for the X and Y values 
            FiftyDay.Series[0].XValueMember = "StockDate";
            FiftyDay.Series[0].YValueMembers = "ClosePrice";
            FiftyDay.Series[1].XValueMember = "StockDate";
            FiftyDay.Series[1].YValueMembers = "HighPrice";
            FiftyDay.Series[2].XValueMember = "StockDate";
            FiftyDay.Series[2].YValueMembers = "LowPrice";
            FiftyDay.Series[3].XValueMember = "StockDate";
            FiftyDay.Series[3].YValueMembers = "MovingDayAveFifty";

            // Populate the chart with data from the data source
            FiftyDay.DataBind();

        }     
        public void PoplulateFiftyDayChartFromSQL()
        {
            // Create a connection to the "pubs" SQL database located on the 

            // local computer.
            SqlConnection myConnection = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString);

            //SqlConnection myConnection = new SqlConnection("server=localhost;" +                    "database=pubs;Trusted_Connection=Yes");

            // Connect to the SQL database using a SQL SELECT query to get all 
            // the data from the "Authors" table.

            //string sqlStr = "SELECT StockDate, ClosePrice FROM [Table] WHERE StockDate >= DateAdd(month, -6, getdate())";
            string sqlString = "SELECT StockQuote_Id, StockDate, ClosePrice, HighPrice, LowPrice, MovingDayAveFifty FROM [Table] WHERE StockQuote_Id <= 50";
            SqlDataAdapter myCommand = new SqlDataAdapter(sqlString, myConnection);

            // Create and fill a DataSet.
            DataSet ds = new DataSet();
            myCommand.Fill(ds);

            DataView source = new DataView(ds.Tables[0]);
            FiftyDay.DataSource = source;

            // set series members names for the X and Y values 
            FiftyDay.Series[0].XValueMember = "StockDate";
            FiftyDay.Series[0].YValueMembers = "ClosePrice";
            FiftyDay.Series[1].XValueMember = "StockDate";
            FiftyDay.Series[1].YValueMembers = "HighPrice";
            FiftyDay.Series[2].XValueMember = "StockDate";
            FiftyDay.Series[2].YValueMembers = "LowPrice";
            FiftyDay.Series[3].XValueMember = "StockDate";
            FiftyDay.Series[3].YValueMembers = "MovingDayAveFifty";

            // Populate the chart with data from the data source
            FiftyDay.DataBind();

        }

        public void PoplulateTwoHundredDayChartFromSQL()
        {
            // Create a connection to the "pubs" SQL database located on the 

            // local computer.
            SqlConnection myConnection = new SqlConnection(global::Earnings_Dip.Properties.Settings.Default.StockPriceDBConnectionString);

            //SqlConnection myConnection = new SqlConnection("server=localhost;" +                    "database=pubs;Trusted_Connection=Yes");

            // Connect to the SQL database using a SQL SELECT query to get all 
            // the data from the "Authors" table.

            //string sqlStr = "SELECT StockDate, ClosePrice FROM [Table] WHERE StockDate >= DateAdd(month, -6, getdate())";
            string sqlString = "SELECT StockQuote_Id, StockDate, ClosePrice, HighPrice, LowPrice, MovingDayAveTwo FROM [Table] WHERE StockQuote_Id <= 200";
            SqlDataAdapter myCommand = new SqlDataAdapter(sqlString, myConnection);

            // Create and fill a DataSet.
            DataSet ds = new DataSet();
            myCommand.Fill(ds);

            DataView source = new DataView(ds.Tables[0]);
            TwoHundredDay.DataSource = source;

            // set series members names for the X and Y values 
            TwoHundredDay.Series[0].XValueMember = "StockDate";
            TwoHundredDay.Series[0].YValueMembers = "ClosePrice";
            TwoHundredDay.Series[1].XValueMember = "StockDate";
            TwoHundredDay.Series[1].YValueMembers = "HighPrice";
            TwoHundredDay.Series[2].XValueMember = "StockDate";
            TwoHundredDay.Series[2].YValueMembers = "LowPrice";
            TwoHundredDay.Series[3].XValueMember = "StockDate";
            TwoHundredDay.Series[3].YValueMembers = "MovingDayAveTwo";

            // Populate the chart with data from the data source
            TwoHundredDay.DataBind();

        }
        public void PopulateSevenDayChart(List<string> listDates, string stockFileName, Chart myChart)
        {
            // variable declarations

            string tempString;
            string[] tempArray;

            DateTime latestEarningsDate = ReturnLastValidEarningsDate(listDates);

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

                if (datesMatch == 0)
                {

                    indexOfEarningsDate = historicalClosePrice.Rows.IndexOf(row);
                    //need to add a break here
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
                sevenDayClosePrice.Rows.Add(historicalClosePrice.Rows[indexOfEarningsDate + sevenday]["Date"], historicalClosePrice.Rows[indexOfEarningsDate + sevenday]["HighPrice"].ToString(), historicalClosePrice.Rows[indexOfEarningsDate + sevenday]["LowPrice"].ToString(), historicalClosePrice.Rows[indexOfEarningsDate + sevenday]["ClosePrice"].ToString());
            }
            sevenDayClosePrice.Rows.Add(historicalClosePrice.Rows[indexOfEarningsDate]["Date"], historicalClosePrice.Rows[indexOfEarningsDate]["HighPrice"].ToString(), historicalClosePrice.Rows[indexOfEarningsDate]["LowPrice"].ToString(), historicalClosePrice.Rows[indexOfEarningsDate]["ClosePrice"].ToString());

            for (int sevenday = 1; sevenday < 8; sevenday++)
            {
                try
                {
                    sevenDayClosePrice.Rows.Add(historicalClosePrice.Rows[indexOfEarningsDate - sevenday]["Date"], historicalClosePrice.Rows[indexOfEarningsDate - sevenday]["HighPrice"].ToString(), historicalClosePrice.Rows[indexOfEarningsDate - sevenday]["LowPrice"].ToString(), historicalClosePrice.Rows[indexOfEarningsDate - sevenday]["ClosePrice"].ToString());
                }
                catch
                {
                    //catch if we dont' have more than 14 days of stock info just don't fill up the sevenDayClosePrice data
                }
            }

            // bind the sevenDayClosePrice DataTable to chart3

            myChart.Titles.Clear();
            myChart.Titles.Add("7 day pre and post trends for " + latestEarningsDate.ToShortDateString());
            myChart.Series["High"].XValueMember = "Date";
            myChart.Series["High"].YValueMembers = "HighPrice";

            myChart.Series["Low"].XValueMember = "Date";
            myChart.Series["Low"].YValueMembers = "LowPrice";

            myChart.Series["Close"].XValueMember = "Date";
            myChart.Series["Close"].YValueMembers = "ClosePrice";

            myChart.DataSource = sevenDayClosePrice;
            myChart.DataBind();

            myChart.ChartAreas[0].AxisX.StripLines.Clear();
            myChart.ChartAreas[0].AxisY.StripLines.Clear();


            // instantiate new strip line
            StripLine stripLine1 = new StripLine();
            stripLine1.Interval = 0;
            stripLine1.IntervalOffset = myChart.DataManipulator.Statistics.Mean(myChart.Series[0].Name);
            stripLine1.BorderColor = Color.Black;
            stripLine1.StripWidth = 0.0;
            stripLine1.Text = "Mean";

            // add the strip line to the chart
            myChart.ChartAreas[0].AxisY.StripLines.Add(stripLine1);

            // instantiate new strip line
            StripLine stripLine2 = new StripLine();
            stripLine2.Interval = 0;
            stripLine2.IntervalOffset = latestEarningsDate.ToOADate();
            stripLine2.BorderColor = Color.Black;
            stripLine2.StripWidth = 0.0;
            stripLine2.Text = "Earnings";

            // add the strip line to the chart
            myChart.ChartAreas[0].AxisX.StripLines.Add(stripLine2);

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

                        if ((tempString.Substring(0, 4) == "Date") || (previousString.Substring(0, 4) == "Date"))
                        {
                            //temp hack we need to skip the first row of lables
                        }
                        else if ((Convert.ToDateTime(myArray[0]) == Convert.ToDateTime(earningsDate)))
                        {

                            previousArray = previousString.Split(',');
                            // calculate close %
                            //decimal closePercent = (((Convert.ToDecimal(previousArray[4])) - (Convert.ToDecimal(myArray[4]))) / (Convert.ToDecimal(previousArray[4]))) * 100;
                            decimal closePercent = (((Convert.ToDecimal(previousArray[4])) / (Convert.ToDecimal(myArray[4]))) - 1) * 100;
                            closePercent = Math.Round(closePercent, 2);

                            // calculate the After Hours Move
                            decimal AHM = (((Convert.ToDecimal(previousArray[1])) / (Convert.ToDecimal(myArray[4]))) - 1) * 100;
                            AHM = Math.Round(AHM, 2);

                            // calculate the Max Percentage
                            decimal Max = ((Convert.ToDecimal(previousArray[2])) / (Convert.ToDecimal(myArray[4])) - 1) * 100;
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
            //myChart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            myChart.ChartAreas[0].AxisX.Interval = 3;
            myChart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            myChart.ChartAreas[0].AxisX.IntervalOffset = 1;

            DateTime minDate = Convert.ToDateTime(listDates.Last());
            DateTime maxDate = Convert.ToDateTime(listDates.First());
            //myChart.ChartAreas[0].AxisX.Minimum = minDate.ToOADate();
            //myChart.ChartAreas[0].AxisX.Maximum = maxDate.ToOADate();


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

        public List<string> BuildListOfDates(string urlString, string stockSymbol)
        {


            //SourceTest = webpage.DownloadString(source_url);
            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
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
                    listDates.Add(Regex.Match(match.Value, @"<td>(.*?)<img src").Groups[1].Value);
                }
            }
            if (listDates.Count == 0)
            {

                MessageBox.Show("There was no infomration about " + stockSymbol + "'s earnings date history.");
                return listDates;
            }
            else
            {
                return listDates;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'stockPriceDBDataSet2.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter2.Fill(this.stockPriceDBDataSet2.Table);
            // TODO: This line of code loads data into the 'stockPriceDBDataSet1.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter1.Fill(this.stockPriceDBDataSet1.Table);
            // TODO: This line of code loads data into the 'stockPriceDBDataSet.Table' table. You can move, or remove it, as needed.
            this.tableTableAdapter.Fill(this.stockPriceDBDataSet.Table);

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart5_Click(object sender, EventArgs e)
        {

        }

        private void TwoHundredDay_Click(object sender, EventArgs e)
        {

        }
    }
}
