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
using AlertDialog = Android.App.AlertDialog;

namespace FinancialCalculator
{
    [Activity(Label = "Return of Investment Calculator")]
    public class RoiActivity : Activity
    {
        private EditText editTextInvestment;
        private EditText editTextRevenue;
        private EditText editTextYears;
        private Button buttonCalculate;
        private Button buttonBack;
        private TextView textViewResult;


        private TextView investmentGainValue;
        private TextView roiValue;
        private TextView annualRoiValue;
        private TextView investmentLengthValue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            SetContentView(Resource.Layout.roi_layout);

            editTextInvestment = FindViewById<EditText>(Resource.Id.editTextInvestment);
            editTextRevenue = FindViewById<EditText>(Resource.Id.editTextRevenue);
            editTextYears = FindViewById<EditText>(Resource.Id.editTextYears);
            buttonCalculate = FindViewById<Button>(Resource.Id.buttonCalculate);
            buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
            textViewResult = FindViewById<TextView>(Resource.Id.textViewResult);

            var tableLayout = FindViewById<TableLayout>(Resource.Id.tableLayout);

            investmentGainValue = FindViewById<TextView>(Resource.Id.investmentGainValue);
            roiValue = FindViewById<TextView>(Resource.Id.roiValue);
            annualRoiValue = FindViewById<TextView>(Resource.Id.annualRoiValue);
            investmentLengthValue = FindViewById<TextView>(Resource.Id.investmentLengthValue);

            buttonCalculate.Click += ButtonCalculate_Click;
            buttonBack.Click += ButtonBack_Click;
        }

        private void ButtonCalculate_Click(object sender, System.EventArgs e)
        {


            if ((string.IsNullOrEmpty(editTextRevenue.Text) && (string.IsNullOrEmpty(editTextInvestment.Text)) && (string.IsNullOrEmpty(editTextYears.Text))))
            {
                ShowInvalidInputMessageBox("Values cannot be null. Enter a value.");
            }
            else if (string.IsNullOrEmpty(editTextInvestment.Text))
            {
                ShowInvalidInputMessageBox("Investment value cannot be null. Input a value.\n\nCorrect values: 25000 or 157000.25");
            }

            else if (string.IsNullOrEmpty(editTextRevenue.Text))
            {
                ShowInvalidInputMessageBox("Revenue value cannot be null. Input a value.\n\nCorrect values: 25000 or 157000.25");
            }
            else if (string.IsNullOrEmpty(editTextYears.Text))
            {
                ShowInvalidInputMessageBox("Number of Years cannot be empty.\n\n Correct values: 5 or 10");

            }

            else
            {
                roiCalc1.SoapServiceTest ws = new roiCalc1.SoapServiceTest();


                string gotValue = ws.get_count(editTextRevenue.Text, editTextInvestment.Text, editTextYears.Text);
                //val/val/val

                string[] tokens = gotValue.Split("/");



                investmentGainValue.Text = tokens[0] + " Pesos";
                roiValue.Text = tokens[1] + "%";
                annualRoiValue.Text = tokens[2] + "% per year";
                investmentLengthValue.Text = editTextYears.Text + " years";


            }

        }



        public void ShowInvalidInputMessageBox(string message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Invalid Input");
            builder.SetMessage(message);
            builder.SetPositiveButton("Ok", (sender, args) => { /* Ok button clicked */ });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }


        private void ButtonBack_Click(object sender, System.EventArgs e)
        {

            //back to Homepage


        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}