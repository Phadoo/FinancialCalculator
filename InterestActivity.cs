using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialCalculator
{
    [Activity(Label = "InterestActivity")]
    public class InterestActivity : Activity
    {
        TextView txt, txt1, txt2, txt3, txt4, txt5, txt6;
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
            txt4 = FindViewById<TextView>(Resource.Id.textView5);
            txt5 = FindViewById<TextView>(Resource.Id.textView6);
            txt6 = FindViewById<TextView>(Resource.Id.textView7);
            //rad = FindViewById<RadioButton>(Resource.Id.radioButton1);
            //rad1 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            //time = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            spin = FindViewById<Spinner>(Resource.Id.spinner1);
            spin1 = FindViewById<Spinner>(Resource.Id.spinner2);
            timeRange = FindViewById<SeekBar>(Resource.Id.seekBar1);
            calculate = FindViewById<Button>(Resource.Id.button1);

            txt.Text = "Initial Investment";
            txt1.Text = "Interest Rate";
            txt2.Text = "Compound Interval";
            txt3.Text = "Time Horizon (YEARS)";
            txt4.Text = "Future investment value: ";
            txt5.Text = "Total interest earned: ";
            txt6.Text = "Initial balance: ";
            //rad.Text = "Years";
            //rad1.Text = "Months";
            calculate.Text = "CALCULATE";

            calculate.Click += Calculate_Click;

            // for Interest Rate
            List<string> spinnerValues = new List<string>
            {
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

            // Set input filters for editText1 and editText2
            edit.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter() });
            edit1.SetFilters(new IInputFilter[] { new DecimalDigitsInputFilter() });

        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            /*
            decimal initial = Decimal.Parse(edit.Text);
            decimal interest = Decimal.Parse(edit1.Text);
            decimal time = Decimal.Parse(edit2.Text);

            interestCalc.FinancialCalculator ws = new interestCalc.FinancialCalculator();

            string appliedInterest = spin1.SelectedItem.ToString();
            string ratePeriod = spin.SelectedItem.ToString();
            string compute = ws.compoundInterest(initial, interest, appliedInterest, time, ratePeriod).ToString();

            txt4.Text = "Compound Interest: " + compute;
            */


            // Validate and parse input values
            if (decimal.TryParse(edit.Text, out decimal initial) &&
                decimal.TryParse(edit1.Text, out decimal interest) &&
                decimal.TryParse(edit2.Text, out decimal time))
            {
                // Validate selected items
                if (spin1.SelectedItem != null && spin.SelectedItem != null)
                {
                    string appliedInterest = spin1.SelectedItem.ToString();
                    string ratePeriod = spin.SelectedItem.ToString();

                    // Call the web service method
                    interestCalc.FinancialCalculator ws = new interestCalc.FinancialCalculator();
                    string compound = ws.compoundInterest(initial, interest, appliedInterest, time, ratePeriod);
                    string earned = (float.Parse(compound) - float.Parse(edit.Text)).ToString("N2");

                    txt4.Text = "Future investment value: ₱ " + compound.ToString();
                    txt5.Text = "Total interest earned: ₱ " + earned;
                    txt6.Text = "Initial balance: ₱ " + initial.ToString("N2");

                    InputMethodManager imm = (InputMethodManager)GetSystemService(InputMethodService);
                    imm.HideSoftInputFromWindow(edit.WindowToken, 0);
                }
                else
                {
                    // Show error message for missing selected items
                    Toast.MakeText(this, "Please select interest type and rate period.", ToastLength.Short).Show();
                }
            }
            else
            {
                // Show error message for invalid input values
                Toast.MakeText(this, "Please enter valid numeric values for initial balance, interest rate, and time.", ToastLength.Short).Show();
            }
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

        public class DecimalDigitsInputFilter : Java.Lang.Object, IInputFilter
        {
            public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
            {
                string filtered = "";
                for (int i = start; i < end; i++)
                {
                    char character = source.CharAt(i);
                    if (char.IsDigit(character) || character == '.')
                    {
                        filtered += character;
                    }
                }
                return new Java.Lang.String(filtered);
            }
        }
    }
}