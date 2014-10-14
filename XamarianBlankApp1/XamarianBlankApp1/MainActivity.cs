using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;


namespace XamarianBlankApp1
{
    [Activity(Label = "XamarianBlankApp1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            GetData();
            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
        }

        public void GetData()
        {

            System.Net.WebClient wc = new System.Net.WebClient();
            string urlString = ("http://www.streetinsider.com/ec_earnings.php?q=AAPL");
            byte[] raw = wc.DownloadData(urlString);

            string input = System.Text.Encoding.UTF8.GetString(raw);
            MatchCollection matchesDates = Regex.Matches(input, @"<td>(.*?)/></td>");


                    foreach (Match match in matchesDates)
                    {

                    }


                    //Console.WriteLine("Matches found: {0}", matchesDates.Count);

                    var listDates = matchesDates.Cast<Match>().Select(match => match.Value).ToList();
                    //var listStocks = matchesStocks.Cast<Match>().Select(match => match.Value).ToList();

            

        }
    }
}

