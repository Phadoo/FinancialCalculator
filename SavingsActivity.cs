using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialCalculator
{
    [Activity(Label = "ConverterActivity")]
    public class SavingsActivity : Activity
    {
        private EditText savingsGoal;
        private EditText startingBalance;
        private EditText growthTime;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private EditText interestRate;
        private TextView textView6;
        private Button calculateButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.savings_layout);

            savingsGoal = FindViewById<EditText>(Resource.Id.savingsGoal);
            startingBalance = FindViewById<EditText>(Resource.Id.startingBalance);
            growthTime = FindViewById<EditText>(Resource.Id.growthTime);
            radioButton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            radioButton2 = FindViewById<RadioButton>(Resource.Id.radioButton2);
            interestRate = FindViewById<EditText>(Resource.Id.interestRate);
            textView6 = FindViewById<TextView>(Resource.Id.textView6);
            calculateButton = FindViewById<Button>(Resource.Id.calculateButton);

            savingsGoal.TextChanged += HandleEmpty;
            startingBalance.TextChanged += HandleEmpty;
            growthTime.TextChanged += HandleEmpty;
            interestRate.TextChanged += HandleEmpty;

            calculateButton.Click += CalculateButton_Click;
            calculateButton.Enabled = false;
        }

        private void HandleEmpty(object sender, TextChangedEventArgs e)
        {
            bool anyFieldEmpty = string.IsNullOrEmpty(savingsGoal.Text) || string.IsNullOrEmpty(startingBalance.Text) || string.IsNullOrEmpty(growthTime.Text) || string.IsNullOrEmpty(interestRate.Text);

            // Disable the button if any field is empty
            calculateButton.Enabled = !anyFieldEmpty;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            savingsCalc.SoapServiceTest savCalc = new savingsCalc.SoapServiceTest();
            double savings = Convert.ToDouble(savingsGoal.Text);
            double interest = Convert.ToDouble(interestRate.Text);
            int growth = Convert.ToInt32(growthTime.Text);
            double starting = Convert.ToDouble(startingBalance.Text);

            if (radioButton1.Checked)
            {
                string monthlyContrib = savCalc.calculateMonthlyContribution(savings, starting, growth, interest);
                textView6.Text = monthlyContrib;
            }

            else if (radioButton2.Checked)
            {
                growth = growth * 12;
                string monthlyContrib = savCalc.calculateMonthlyContribution(savings, starting, growth, interest);
                textView6.Text = monthlyContrib;
            }
        }
    }
}
