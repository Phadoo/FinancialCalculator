using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Description;

namespace FinancialCalculator
{
    [Activity(Label = "MortgageActivity")]
    public class MortgageActivity : Activity
    {
        private EditText loanInput;
        private EditText interestInput;
        private EditText loanTermInput;
        private EditText taxesInput;
        private EditText insuranceInput;
        private TextView monthlyPaymentResult;
        private TextView totalInterestResult;
        private LinearLayout amortizationItemsLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mortgage_layout);

            // Find UI elements by their IDs
            loanInput = FindViewById<EditText>(Resource.Id.loanInput);
            interestInput = FindViewById<EditText>(Resource.Id.interestInput);
            loanTermInput = FindViewById<EditText>(Resource.Id.loanTermInput);
            taxesInput = FindViewById<EditText>(Resource.Id.taxesInput);
            insuranceInput = FindViewById<EditText>(Resource.Id.insuranceInput);
            monthlyPaymentResult = FindViewById<TextView>(Resource.Id.monthlyPaymentResult);
            totalInterestResult = FindViewById<TextView>(Resource.Id.totalInterestResult);
            amortizationItemsLayout = FindViewById<LinearLayout>(Resource.Id.amortizationItemsLayout);

            // Hook up the click event for the Calculate button
            Button calculateButton = FindViewById<Button>(Resource.Id.calculateButton);
            calculateButton.Click += CalculateButton_Click;
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            // Get user inputs

            double loanAmount = Convert.ToDouble(loanInput.Text);
            double interestRate = Convert.ToDouble(interestInput.Text) / 100; // Convert interest rate to decimal
            int loanTerm = Convert.ToInt32(loanTermInput.Text);

            mortgageCalculation.MortgageCalculator mortgageCalculate = new mortgageCalculation.MortgageCalculator();

            double monthlyPayment = mortgageCalculate.get_mortgage(loanAmount, interestRate, loanTerm);

            double totalInterest = mortgageCalculate.get_totalinterest(loanAmount, interestRate, loanTerm);

            // Display results
            monthlyPaymentResult.Text = monthlyPayment.ToString("C2");
            totalInterestResult.Text = totalInterest.ToString("C2");

            // Calculate and display loan amortization schedule
            CalculateAmortizationSchedule(loanAmount, interestRate, loanTerm, monthlyPayment);
        }



        private void CalculateAmortizationSchedule(double loanAmount, double interestRate, int loanTerm, double monthlyPayment)
        {
            amortizationItemsLayout.RemoveAllViews();

            // Create an instance of the service class
            var mortgageCalculate = new mortgageCalculation.MortgageCalculator();

            // Call the create_amortizationschedule method
            var amortizationSchedule = mortgageCalculate.create_amortizationschedule(loanAmount, interestRate, loanTerm, monthlyPayment);
            foreach (var scheduleItem in amortizationSchedule)
            {
                // Create a new row in the amortization schedule layout
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                LinearLayout rowLayout = new LinearLayout(this);
                rowLayout.Orientation = Orientation.Horizontal;
                rowLayout.LayoutParameters = layoutParams;

                // Create and add TextViews for payment number, interest, principal, and remaining balance
                TextView paymentNumberText = new TextView(this);
                paymentNumberText.Text = scheduleItem.PaymentNumber.ToString();
                rowLayout.AddView(paymentNumberText);

                TextView interestText = new TextView(this);
                interestText.Text = scheduleItem.InterestPayment.ToString("C2");
                rowLayout.AddView(interestText);

                TextView principalText = new TextView(this);
                principalText.Text = scheduleItem.PrincipalPayment.ToString("C2");
                rowLayout.AddView(principalText);

                TextView totalAmountText = new TextView(this);
                totalAmountText.Text = monthlyPayment.ToString("C2");
                rowLayout.AddView(totalAmountText);

                TextView balanceText = new TextView(this);
                balanceText.Text = scheduleItem.RemainingBalance.ToString("C2");
                rowLayout.AddView(balanceText);

                // Add spacing between TextViews
                int spacing = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 8, Resources.DisplayMetrics);
                LinearLayout.LayoutParams textViewParams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1);
                textViewParams.SetMargins(spacing, 0, spacing, 0);
                paymentNumberText.LayoutParameters = textViewParams;
                principalText.LayoutParameters = textViewParams;
                interestText.LayoutParameters = textViewParams;
                totalAmountText.LayoutParameters = textViewParams;
                balanceText.LayoutParameters = textViewParams;

                // Add the row to the amortization schedule layout
                amortizationItemsLayout.AddView(rowLayout);
            }
        }
    }
}