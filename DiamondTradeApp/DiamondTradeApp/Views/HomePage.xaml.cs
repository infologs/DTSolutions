﻿using DiamondTradeApp.Services;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DiamondTradeApp.Views
{
    public partial class HomePage : ContentPage
    {
        ObservableCollection<ReportMaster> reportMastersList = new ObservableCollection<ReportMaster>();
        public ObservableCollection<ReportMaster> ReportMastersList { get { return reportMastersList; } }

        private readonly ReportMasterRepository _reportMasterRepository;

        public HomePage()
        {
            InitializeComponent();
            _reportMasterRepository = new ReportMasterRepository();

            FillBox();
            onPurchaseClick();
        }

        protected override async void OnAppearing()
        {
            var remember = Preferences.Get("rememberMe_Key", null);
            var userId = Preferences.Get("userId_key", null);
            if (remember == "false" || userId == null)
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage(), true);
            }
            base.OnAppearing();
        }

        private void FillBox()
        {
            var reportList = _reportMasterRepository.GetDashboardReports("", "", 0);
            ReportMaster reportMaster = null;
            if (reportList != null)
            {
                reportMaster = new ReportMaster
                {
                    Asset = reportList.Rows[0][2].ToString(),
                    CashInHand = reportList.Rows[1][2].ToString(),
                    Deposit = reportList.Rows[2][2].ToString(),
                    Investment = reportList.Rows[3][2].ToString(),
                    OpeningStock = reportList.Rows[4][2].ToString(),
                    Sales = reportList.Rows[5][2].ToString(),
                    UchinaPlusEmployee = reportList.Rows[6][2].ToString(),
                    CapitalAccount = reportList.Rows[7][2].ToString(),
                    Loan = reportList.Rows[8][2].ToString(),
                    Purchase = reportList.Rows[9][2].ToString()
                };

                lblAssets.Text = reportMaster.Asset;
                lblCashInHand.Text = reportMaster.CashInHand;
                lblCapitalAct.Text = reportMaster.CapitalAccount;
                lblDeposit.Text = reportMaster.Deposit;
                lblInvestment.Text = reportMaster.Investment;
                lblLoan.Text = reportMaster.Loan;
                lblOpeningStock.Text = reportMaster.OpeningStock;
                lblPurchase.Text = reportMaster.Purchase;
                lblSales.Text = reportMaster.Sales;
                lblUchina.Text = reportMaster.UchinaPlusEmployee;
            }
        }

        void onPurchaseClick()
        {
            boxPurchase.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () =>
                {
                    await Shell.Current.GoToAsync("//ReportsPage");
                })
            });
        }
    }

    public class ReportMaster
    {
        public string Type { get; set; }
        public string Component { get; set; }
        public string Value { get; set; }

        public string Asset { get; set; }
        public string CashInHand { get; set; }
        public string Deposit { get; set; }
        public string Investment { get; set; }
        public string OpeningStock { get; set; }
        public string Sales { get; set; }
        public string UchinaPlusEmployee { get; set; }
        public string CapitalAccount { get; set; }
        public string Loan { get; set; }
        public string Purchase { get; set; }
    }
}