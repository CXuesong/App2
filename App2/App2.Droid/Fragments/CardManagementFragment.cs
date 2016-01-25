using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace App2.Droid.Fragments
{
    public class CardManagementFragment : Fragment
    {
        private TextView balanceTextView;
        private EditText transferAmountEditText;
        private Button refreshButton, transferButton;


        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

        public override async void OnStart()
        {
            base.OnStart();
            GlobalServices.XjtuSite.Card.Updated += Card_Updated;
        }

        private static CultureInfo zhCN = CultureInfo.GetCultureInfo("zh-CN");

        private void Card_Updated(object sender, EventArgs e)
        {
            if (balanceTextView != null)
            {
                balanceTextView.Text = GlobalServices.XjtuSite.Card.Balance?.ToString("C", zhCN);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.CardManagement, container, false);
            balanceTextView = view.FindViewById<TextView>(Resource.Id.balanceTextView);
            transferAmountEditText = view.FindViewById<EditText>(Resource.Id.transferAmountEditText);
            transferButton = view.FindViewById<Button>(Resource.Id.transferButton);
            refreshButton = view.FindViewById<Button>(Resource.Id.refreshButton);
            transferAmountEditText.TextChanged += TransferAmountEditText_TextChanged;
            transferButton.Click += TransferButton_Click;
            refreshButton.Click += RefreshButton_Click;
            if (GlobalServices.XjtuSite.Card.IsInvalidated) UpdateCardManager();
            return view;
        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            await UpdateCardManager();
        }

        private async void TransferButton_Click(object sender, EventArgs e)
        {
            transferButton.Enabled = false;
            try
            {
                await GlobalServices.XjtuSite.Card.Transfer(Convert.ToDecimal(transferAmountEditText.Text));
            }
            catch (Exception ex)
            {
                DroidUtility.ReportException(this.Activity, ex);
            }
            finally
            {
                transferButton.Enabled = true;
            }
        }

        private async Task UpdateCardManager()
        {
            refreshButton.Enabled = false;
            try
            {
                await GlobalServices.XjtuSite.Card.UpdateAsync();
            }
            finally
            {
                refreshButton.Enabled = true;
            }
        }

        private void TransferAmountEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            double amount;
            if (double.TryParse(transferAmountEditText.Text, out amount))
                transferButton.Enabled = amount >= 0.01 && amount <= 500;
            else
                transferButton.Enabled = false;
        }
    }
}