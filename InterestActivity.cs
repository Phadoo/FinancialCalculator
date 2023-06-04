using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialCalculator
{
    [Activity(Label = "InterestActivity")]
    public class InterestActivity : Activity
    {
        TextView txt, txt1, txt2, txt3;
        EditText edit, edit1, edit2;
        Spinner spin, spin1;
        RadioGroup time;
        RadioButton rad, rad1;
        SeekBar timeRange;
        Button calculate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.interest_layout);

            edit = FindViewById<EditText>(Resource.Id.editText1);
            edit1 = FindViewById<EditText>(Resource.Id.editText2);
            edit2 = FindViewById<EditText>(Resource.Id.editText3);
            txt = FindViewById<TextView>(Resource.Id.textView1);
            txt1 = FindViewById<TextView>(Resource.Id.textView2);
            txt2 = FindViewById<TextView>(Resource.Id.textView3);
            txt3 = FindViewById<TextView>(Resource.Id.textView4);
            rad = FindViewById<RadioButton>(Resource.Id.radioButton1);
            rad1 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            time = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            spin = FindViewById<Spinner>(Resource.Id.spinner1);
            spin1 = FindViewById<Spinner>(Resource.Id.spinner2);
            timeRange = FindViewById<SeekBar>(Resource.Id.seekBar1);
            calculate = FindViewById<Button>(Resource.Id.button1);

            txt.Text = "Initial Investment";
            txt1.Text = "Interest Rate";
            txt2.Text = "Compound Interval";
            txt3.Text = "Time Horizon";
            rad.Text = "Years";
            rad1.Text = "Months";
            calculate.Text = "CALCULATE";

            // for Interest Rate
            List<string> spinnerValues = new List<string>
            {
                "Daily",
                "Weekly",
                "Monthly",
                "Quarterly",
                "Yearly"
            };
            
            ArrayAdapter<string> spinAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spinnerValues);
            spinAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = spinAdapter;

            // for Compound Interval
            List<string> spinnerValues1 = new List<string>
            {
                "Monthly",
                "Quarterly",
                "Semi-annually",
                "Yearly"
            };
            
            ArrayAdapter<string> spinAdapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, spinnerValues1);
            spinAdapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin1.Adapter = spinAdapter1;

            // for Seekbar
            // Set up listener for SeekBar value changes
            timeRange.ProgressChanged += TimeRange_ProgressChanged;
            // Set the initial value of EditText based on SeekBar's initial value
            edit2.Text = timeRange.Progress.ToString();
            // Set the range of SeekBar
            timeRange.Max = 59;
            // Set the OnTouchListener for the EditText to disable manual input
            edit2.SetOnTouchListener(new EditTextTouchListener());

        }

        private void TimeRange_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            // Update the EditText value with the SeekBar's current value
            edit2.Text = (e.Progress + 1).ToString();
        }

        private class EditTextTouchListener : Java.Lang.Object, View.IOnTouchListener
        {
            public bool OnTouch(View v, MotionEvent e)
            {
                // Disable manual input by intercepting touch events
                return true;
            }
        }
    }
}