using Android.App;
using Android.Content;
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
    [Activity(Label = "InvestmentActivity")]
    public class InvestmentActivity : Activity
    {
        EditText startingAmountTxt, yearsTxt, rateTxt, addContributionTxt;
        TextView finalAmountTxt, totalContributionTxt, totalInterestTxt;
        Button calculateButton;
        RadioGroup radioTiming;
        string selectedTiming = "beginning";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.net_layout);
            startingAmountTxt = FindViewById<EditText>(Resource.Id.startingAmountTxt);
            yearsTxt = FindViewById<EditText>(Resource.Id.yearsTxt);
            rateTxt = FindViewById<EditText>(Resource.Id.rateTxt);
            addContributionTxt = FindViewById<EditText>(Resource.Id.addContributionTxt);
            finalAmountTxt = FindViewById<TextView>(Resource.Id.finalAmountTxt);
            totalContributionTxt = FindViewById<TextView>(Resource.Id.totalContributionTxt);
            totalInterestTxt = FindViewById<TextView>(Resource.Id.totalInterestTxt);
            radioTiming = FindViewById<RadioGroup>(Resource.Id.radioTiming);
            radioTiming.CheckedChange += myRadioGroup_CheckedChange;

            calculateButton = FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;


        }

        void myRadioGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            // Get the ID of the checked RadioButton
            int checkedRadioButtonId = radioTiming.CheckedRadioButtonId;

            // Set selectedGender based on the checked RadioButton ID
            switch (checkedRadioButtonId)
            {
                case Resource.Id.beginning:
                    selectedTiming = "beginning";
                    break;
                case Resource.Id.end:
                    selectedTiming = "end";
                    break;
                default:
                    selectedTiming = string.Empty; // No selection
                    break;
            }
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            bool saValid = ValidateInput(startingAmountTxt.Text);
            bool rateValid = ValidateInput(rateTxt.Text);
            bool addValid = ValidateInput(addContributionTxt.Text);
            bool yearsValid = ValidateYearInput(yearsTxt.Text);
            if (saValid && rateValid && addValid)
            {
                if (yearsValid)
                {
                    investCalc.InvestmentCalculator ic = new investCalc.InvestmentCalculator();
                    decimal startingAmount = Decimal.Parse(startingAmountTxt.Text);
                    int years = Int32.Parse(yearsTxt.Text);
                    decimal additionalContribution = Decimal.Parse(addContributionTxt.Text);
                    decimal rate = Decimal.Parse(rateTxt.Text);
                    string finalAmount = ic.calculate_final_amount(startingAmount, years, rate, additionalContribution, selectedTiming);
                    decimal fAmount = Decimal.Parse(finalAmount);
                    decimal totContrib = years * additionalContribution;
                    decimal totInterest = fAmount - totContrib - startingAmount;
                    finalAmountTxt.Text = "Final Amount: ₱" + finalAmount;
                    totalContributionTxt.Text = "Total Contribution: ₱" + totContrib.ToString("F");
                    totalInterestTxt.Text = "Total Interest: ₱" + totInterest.ToString("F");
                }
                else
                {
                    Toast.MakeText(this, "Invalid year. Minimum of 1 year", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Invalid input. Please input a positive number", ToastLength.Long).Show();
            }

        }
        public bool ValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false; // Input is empty or contains only whitespace
            }

            if (!decimal.TryParse(input, out decimal number))
            {
                return false; // Input is not a valid decimal number
            }

            if (number <= 0)
            {
                return false; // Input is zero or a negative number
            }

            return true; // Input is valid (not empty and positive decimal)
        }
        public bool ValidateYearInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false; // Input is empty or contains only whitespace
            }

            if (!int.TryParse(input, out int year))
            {
                return false; // Input is not a valid integer
            }

            if (year < 1)
            {
                return false; // Input is less than 1 year
            }

            return true; // Input is valid (not empty, positive integer, and at least 1 year)
        }
    }
}