using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace FinancialCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        ImageButton intBtn, roiBtn, curBtn, netBtn, morBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.main_layout);

            intBtn = FindViewById<ImageButton>(Resource.Id.imageButton1);
            roiBtn = FindViewById<ImageButton>(Resource.Id.imageButton2);
            curBtn = FindViewById<ImageButton>(Resource.Id.imageButton3);
            netBtn = FindViewById<ImageButton>(Resource.Id.imageButton4);
            morBtn = FindViewById<ImageButton>(Resource.Id.imageButton5);

            intBtn.Click += intBtn_Click;
            roiBtn.Click += roiBtn_Click;
            curBtn.Click += curBtn_Click;
            netBtn.Click += netBtn_Click;
            morBtn.Click += morBtn_Click;

        }

        private void morBtn_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(MortgageActivity));
            StartActivity(i);
        }

        private void netBtn_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(InvestmentActivity));
            StartActivity(i);
        }

        private void curBtn_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(SavingsActivity));
            StartActivity(i);
        }

        private void roiBtn_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(RoiActivity));
            StartActivity(i);
        }

        private void intBtn_Click(object sender, System.EventArgs e)
        {
            Intent i = new Intent(this, typeof(InterestActivity));
            StartActivity(i);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}