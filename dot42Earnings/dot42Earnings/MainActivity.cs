using System;
using Android.App;
using Android.Os;
using Android.Widget;
using Dot42;
using Dot42.Manifest;

[assembly: Application("dot42Earnings")]

namespace dot42Earnings
{
    [Activity]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstance)
        {
            base.OnCreate(savedInstance);
            SetContentView(R.Layouts.MainLayout);
        }
    }
}
