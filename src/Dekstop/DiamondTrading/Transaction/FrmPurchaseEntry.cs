﻿using DevExpress.Utils;
using DevExpress.XtraEditors;
using EFCore.SQL.Repository;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiamondTrading.Transaction
{
    public partial class FrmPurchaseEntry : DevExpress.XtraEditors.XtraForm
    {
        PurchaseMasterRepository _purchaseMasterRepository;
        PartyMasterRepository _partyMasterRepository;
        private readonly BrokerageMasterRepository _brokerageMasterRepository;
        decimal ItemRunningWeight = 0;
        decimal ItemFinalAmount = 0;
        public FrmPurchaseEntry()
        {
            InitializeComponent();
            _purchaseMasterRepository = new PurchaseMasterRepository();
            _partyMasterRepository = new PartyMasterRepository();
            _brokerageMasterRepository = new BrokerageMasterRepository();
            this.Text = "PURCHASE - " + Common.LoginCompanyName + " - [" + Common.LoginFinancialYearName + "]";
        }

        private void FrmPurchaseEntry_Load(object sender, EventArgs e)
        {
            lblFormTitle.Text = Common.FormTitle;
            SetSelectionBackColor();
            tglSlip.IsOn = Common.PrintPurchaseSlip;
            tglPF.IsOn = Common.PrintPurchasePF;
            dtPayDate.Enabled = Common.AllowToSelectPurchaseDueDate;

            SetThemeColors(Color.FromArgb(250, 243, 197));

            //SetThemeColors(Color.FromArgb(0));

            LoadCompany();
            FillCombos();
            //FillBranches();
            FillCurrency();
            //FillCurrency();
            
        }

        private async void LoadCompany()
        {
            CompanyMasterRepository companyMasterRepository = new CompanyMasterRepository();
            var companies = await companyMasterRepository.GetAllCompanyAsync();
            lueCompany.Properties.DataSource = companies;
            lueCompany.Properties.DisplayMember = "Name";
            lueCompany.Properties.ValueMember = "Id";

            lueCompany.EditValue = Common.LoginCompany;
            LoadBranch(Common.LoginCompany);
        }

        private void SetThemeColors(Color color)
        {
            if (!color.ToArgb().ToString().Equals(Color.FromArgb(0).Name))
            {
                grpGroup1.AppearanceCaption.BorderColor = color;
                grpGroup2.AppearanceCaption.BorderColor = color;
                grpGroup3.AppearanceCaption.BorderColor = color;
                grpGroup4.AppearanceCaption.BorderColor = color;
                grpGroup5.AppearanceCaption.BorderColor = color;
                grpGroup6.AppearanceCaption.BorderColor = color;
                grpGroup7.AppearanceCaption.BorderColor = color;
                grpGroup8.AppearanceCaption.BorderColor = color;
                grpGroup9.AppearanceCaption.BorderColor = color;

                txtCurrencyType.BackColor = color;
                txtBuyerCommisionPer.BackColor = color;
                txtPartyBalance.BackColor = color;
                txtBrokerPer.BackColor = color;
            }
        }

        private async void FillBranches()
        {
            //Branch
            BranchMasterRepository branchMasterRepository = new BranchMasterRepository();
            var branchMaster = await branchMasterRepository.GetAllBranchAsync(Common.LoginCompany);
            lueBranch.Properties.DataSource = branchMaster;
            lueBranch.Properties.DisplayMember = "Name";
            lueBranch.Properties.ValueMember = "Id";
        }

        private async void FillCurrency()
        {
            //Currency
            CurrencyMasterRepository currencyMasterRepository = new CurrencyMasterRepository();
            var currencyMaster = await currencyMasterRepository.GetAllCurrencyAsync();
            lueCurrencyType.Properties.DataSource = currencyMaster;
            lueCurrencyType.Properties.DisplayMember = "Name";
            lueCurrencyType.Properties.ValueMember = "Id";
        }

        private void FillCombos()
        {
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;
            dtPayDate.EditValue = DateTime.Now;

            LoadPurchaseItemDetails();

            //Payment Mode
            luePaymentMode.Properties.DataSource = Common.GetPaymentType;
            luePaymentMode.Properties.DisplayMember = "PTypeName";
            luePaymentMode.Properties.ValueMember = "PTypeID";                        

            //Buyer
            GetBuyerList();

            //Party
            GetPartyList();

            //Broker
            GetBrokerList();
        }

        private void LoadPurchaseItemDetails()
        {
            grdPurchaseDetails.DataSource = GetDTColumnsforPurchaseDetails();            

            //Shape
            GetShapeDetail();

            //Size
            GetSizeDetail();

            //Purity
            GetPurityDetail();

            //Kapan
            GetKapanDetail ();
        }

        private async Task GetBuyerList()
        {
            string companyId = Common.LoginCompany ;
            if(lueCompany.EditValue != null)
            {
                if (lueCompany.EditValue.ToString() != Common.LoginCompany)
                    companyId = lueCompany.EditValue.ToString();
            }
            var BuyerDetailList = await _partyMasterRepository.GetAllPartyAsync(companyId, PartyTypeMaster.Employee, PartyTypeMaster.Buyer);
            lueBuyer.Properties.DataSource = BuyerDetailList;
            lueBuyer.Properties.DisplayMember = "Name";
            lueBuyer.Properties.ValueMember = "Id";
        }

        private async Task GetPartyList()
        {
            string companyId = Common.LoginCompany;
            if (lueCompany.EditValue != null)
            {
                if (lueCompany.EditValue.ToString() != Common.LoginCompany)
                    companyId = lueCompany.EditValue.ToString();
            }
            var PartyDetailList = await _partyMasterRepository.GetAllPartyAsync(companyId, PartyTypeMaster.Party);
            lueParty.Properties.DataSource = PartyDetailList;
            lueParty.Properties.DisplayMember = "Name";
            lueParty.Properties.ValueMember = "Id";
        }

        private async Task GetBrokerList()
        {
            string companyId = Common.LoginCompany;
            if (lueCompany.EditValue != null)
            {
                if (lueCompany.EditValue.ToString() != Common.LoginCompany)
                    companyId = lueCompany.EditValue.ToString();
            }
            var BrokerDetailList = await _partyMasterRepository.GetAllPartyAsync(companyId, PartyTypeMaster.Employee, PartyTypeMaster.Broker);
            lueBroker.Properties.DataSource = BrokerDetailList;
            lueBroker.Properties.DisplayMember = "Name";
            lueBroker.Properties.ValueMember = "Id";
        }
        
        private async void LoadBranch(string companyId)
        {
            BranchMasterRepository branchMasterRepository = new BranchMasterRepository();
            var branches = await branchMasterRepository.GetAllBranchAsync(companyId); //_branchMasterRepository.GetAllBranchAsync();
            lueBranch.Properties.DataSource = branches;
            lueBranch.Properties.DisplayMember = "Name";
            lueBranch.Properties.ValueMember = "Id";
            lueBranch.EditValue = Common.LoginBranch;

            GetPurchaseNo(); //Serial No/Slip No
        }

        private async Task GetShapeDetail()
        {
            ShapeMasterRepository shapeMasterRepository = new ShapeMasterRepository();
            var shapeMaster = await shapeMasterRepository.GetAllShapeAsync();
            repoShape.DataSource = shapeMaster;
            repoShape.DisplayMember = "Name";
            repoShape.ValueMember = "Id";
        }

        private async Task GetSizeDetail()
        {
            SizeMasterRepository sizeMasterRepository = new SizeMasterRepository();
            var sizeMaster = await sizeMasterRepository.GetAllSizeAsync();
            repoSize.DataSource = sizeMaster;
            repoSize.DisplayMember = "Name";
            repoSize.ValueMember = "Id";
        }

        private async Task GetPurityDetail()
        {
            PurityMasterRepository purityMasterRepository = new PurityMasterRepository();
            var purityMaster = await purityMasterRepository.GetAllPurityAsync();
            repoPurity.DataSource = purityMaster;
            repoPurity.DisplayMember = "Name";
            repoPurity.ValueMember = "Id";
        }

        private async Task GetKapanDetail()
        {
            KapanMasterRepository kapanMasterRepository = new KapanMasterRepository();
            var kapanMaster = await kapanMasterRepository.GetAllKapanAsync();
            repoKapan.DataSource = kapanMaster;
            repoKapan.DisplayMember = "Name";
            repoKapan.ValueMember = "Id";
        }

        public async void GetPurchaseNo(bool updateSlip = true)
        {
            try
            {
                var SrNo = await _purchaseMasterRepository.GetMaxSrNo(lueBranch.EditValue.ToString(), Common.LoginFinancialYear);
                txtSerialNo.Text = SrNo.ToString();

                if (updateSlip)
                {
                    var SlipNo = await _purchaseMasterRepository.GetMaxSlipNo(lueCompany.EditValue.ToString(), Common.LoginFinancialYear);
                    txtSlipNo.Text = SlipNo.ToString();
                }
            }
            catch(Exception Ex)
            {
                MessageBox.Show("Error : " + Ex.Message.ToString(), "["+this.Name+"]", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSelectionBackColor()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;

                for (int i = 0; i < ctrl.Controls.Count; i++)
                {
                    ctrl.Controls[i].GotFocus += ctrl_GotFocus;
                    ctrl.Controls[i].LostFocus += ctrl_LostFocus;
                }
            }
            grvPurchaseDetails.Appearance.FocusedCell.BackColor = Color.LightSteelBlue;
        }

        private void ctrl_LostFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl.Tag is Color)
                ctrl.BackColor = (Color)ctrl.Tag;
        }

        private void ctrl_GotFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            ctrl.Tag = ctrl.BackColor;
            ctrl.BackColor = Color.LightSteelBlue;
        }

        private async void NewEntry(object sender, KeyEventArgs e)
        {
            string ControlName = ((DevExpress.XtraEditors.LookUpEdit)sender).Name;
            if (e.Control && e.KeyCode == Keys.N)
            {
                if (ControlName == lueBuyer.Name)
                {
                    Master.FrmPartyMaster frmPartyMaster = new Master.FrmPartyMaster();
                    frmPartyMaster.IsSilentEntry = true;
                    frmPartyMaster.LedgerType = PartyTypeMaster.Buyer;
                    if (frmPartyMaster.ShowDialog() == DialogResult.OK)
                    {
                        await GetBuyerList();
                        lueBuyer.EditValue = frmPartyMaster.CreatedLedgerID;
                    }
                }
                else if (ControlName == lueParty.Name)
                {
                    Master.FrmPartyMaster frmPartyMaster = new Master.FrmPartyMaster();
                    frmPartyMaster.IsSilentEntry = true;
                    frmPartyMaster.LedgerType = PartyTypeMaster.Party;
                    if (frmPartyMaster.ShowDialog() == DialogResult.OK)
                    {
                        await GetPartyList();
                        lueParty.EditValue = frmPartyMaster.CreatedLedgerID;
                    }
                }
                else if (ControlName == lueBroker.Name)
                {
                    Master.FrmPartyMaster frmPartyMaster = new Master.FrmPartyMaster();
                    frmPartyMaster.IsSilentEntry = true;
                    frmPartyMaster.LedgerType = PartyTypeMaster.Broker;
                    if (frmPartyMaster.ShowDialog() == DialogResult.OK)
                    {
                        await GetBrokerList();
                        lueBroker.EditValue = frmPartyMaster.CreatedLedgerID;
                    }
                }
            }
        }

        private void lueCurrencyType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetCurrencyTypeRate();
        }

        private void lueCurrencyType_Leave(object sender, EventArgs e)
        {
            GetCurrencyTypeRate();
        }

        private void GetCurrencyTypeRate()
        {
            try
            {
                if (lueCurrencyType.EditValue != null)
                {
                    txtCurrencyType.Text = lueCurrencyType.GetColumnValue("Value").ToString();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error : " + Ex.Message.ToString(), "[" + this.Name + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPaymentDays_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtPaymentDays.Text.Length > 0)
            {
                dtPayDate.EditValue = CalculateDate(Convert.ToInt32(txtPaymentDays.Text));
            }
        }

        private void txtPaymentDays_Leave(object sender, EventArgs e)
        {
            if(txtPaymentDays.Text.Length > 0)
            {
                dtPayDate.EditValue = CalculateDate(Convert.ToInt32(txtPaymentDays.Text));
            }
        }

        private DateTime CalculateDate(int Days)
        {
            if (Convert.ToDateTime(dtPayDate.EditValue).Date != Convert.ToDateTime(dtDate.EditValue).Date)
                dtPayDate.EditValue = dtDate.EditValue;
            return Convert.ToDateTime(dtPayDate.EditValue).AddDays(Days);
        }

        private void dtPayDate_Validated(object sender, EventArgs e)
        {
            txtPaymentDays.Text = CalculateDays(Convert.ToDateTime(dtPayDate.EditValue)).ToString();
        }

        private int CalculateDays(DateTime Date)
        {
            TimeSpan ts = Date.Subtract(Convert.ToDateTime(dtDate.EditValue));
            return ts.Days;
        }

        private static DataTable GetDTColumnsforPurchaseDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Shape");
            dt.Columns.Add("Size");
            dt.Columns.Add("Purity");
            dt.Columns.Add("Kapan");
            dt.Columns.Add("Carat");
            dt.Columns.Add("Tip");
            dt.Columns.Add("CVD");
            dt.Columns.Add("RejPer");
            dt.Columns.Add("RejCts");
            dt.Columns.Add("LessCts");
            dt.Columns.Add("NetCts");
            dt.Columns.Add("Rate");
            dt.Columns.Add("DiscPer");
            dt.Columns.Add("CVDCharge");
            dt.Columns.Add("Amount");
            dt.Columns.Add("CRate");
            dt.Columns.Add("CAmount");
            dt.Columns.Add("DisAmount");
            dt.Columns.Add("CVDAmount");
            return dt;
        }

        private void labelControl9_Click(object sender, EventArgs e)
        {

        }

        private void FrmPurchaseEntry_KeyDown(object sender, KeyEventArgs e)
        {
            Common.MoveToNextControl(sender, e, this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void repoShape_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                Master.FrmShapeMaster frmShapeMaster = new Master.FrmShapeMaster();
                frmShapeMaster.IsSilentEntry = true;
                if (frmShapeMaster.ShowDialog() == DialogResult.OK)
                {
                    await GetShapeDetail();
                    grvPurchaseDetails.SetFocusedRowCellValue(colShape, frmShapeMaster.CreatedLedgerID.ToString());
                }
            }
        }

        private async void repoSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                Master.FrmSizeMaster frmSizeMaster = new Master.FrmSizeMaster();
                frmSizeMaster.IsSilentEntry = true;
                if (frmSizeMaster.ShowDialog() == DialogResult.OK)
                {
                    await GetSizeDetail();
                    grvPurchaseDetails.SetFocusedRowCellValue(colSize, frmSizeMaster.CreatedLedgerID.ToString());
                }
            }
        }

        private async void repoPurity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                Master.FrmPurityMaster frmPurityMaster = new Master.FrmPurityMaster();
                frmPurityMaster.IsSilentEntry = true;
                if (frmPurityMaster.ShowDialog() == DialogResult.OK)
                {
                    await GetPurityDetail();
                    grvPurchaseDetails.SetFocusedRowCellValue(colPurity, frmPurityMaster.CreatedLedgerID.ToString());
                }
            }
        }

        private async void repoKapan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                Master.FrmKapanMaster frmKapanMaster = new Master.FrmKapanMaster();
                frmKapanMaster.IsSilentEntry = true;
                if (frmKapanMaster.ShowDialog() == DialogResult.OK)
                {
                    await GetKapanDetail();
                    grvPurchaseDetails.SetFocusedRowCellValue(colKapan, frmKapanMaster.CreatedLedgerID.ToString());
                }
            }
        }

        private void grvPurchaseDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colCarat)
                {
                    decimal TipWeight = Convert.ToDecimal(lueBranch.GetColumnValue("TipWeight"));
                    decimal CVDWeight = Convert.ToDecimal(lueBranch.GetColumnValue("CVDWeight"));
                    GetLessWeightDetailBasedOnCity(lueBranch.GetColumnValue("LessWeightId").ToString(), Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCarat)), e.RowHandle, TipWeight, CVDWeight);
                }
                else if (e.Column == colRejPer)
                {
                    CalculateRejectionValue(true, e.RowHandle);
                }
                else if (e.Column == colRejCts)
                {
                    CalculateRejectionValue(false, e.RowHandle);
                }

                FinalCalculation(e.RowHandle);
            }
            catch
            {
            }
        }

        private void grvPurchaseDetails_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetTotal();
        }

        private void grvPurchaseDetails_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            grvPurchaseDetails.SetRowCellValue(e.RowHandle, colShape, Common.DefaultShape);
            grvPurchaseDetails.SetRowCellValue(e.RowHandle, colSize, Common.DefaultSize);
            grvPurchaseDetails.SetRowCellValue(e.RowHandle, colPurity, Common.DefaultPurity);
        }

        private void grvPurchaseDetails_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            if (MessageBox.Show("Do you want add more Items...???", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
            {
                //IsFocusMoveToOutsideGrid = true;
                //grvPurchaseItems.CloseEditor();
                //this.SelectNextControl(grdPurchaseItems, true, true, true, true);


                //this.SelectNextControl(grdPurchaseItems, true, true, true, true);
            }
        }

        private void grvPurchaseDetails_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colShape) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colShape) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colShape).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Shape detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colShape;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colSize) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colSize) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colSize).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Size detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colSize;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colPurity) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colPurity) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colPurity).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Purity detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colPurity;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colKapan) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colKapan) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colKapan).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Kapan detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colKapan;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCarat) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCarat) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCarat).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Carat detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colCarat;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colTipWeight) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colTipWeight) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colTipWeight).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Tip Weight detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colTipWeight;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCVDWeight) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCVDWeight) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCVDWeight).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter CVD Weight detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colCVDWeight;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRejPer) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRejPer) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRejPer).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Rejection (%) detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colRejPer;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRejCts) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRejCts) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRejCts).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Rejection Weight detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colRejCts;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colLessCts) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colLessCts) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colLessCts).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Less Weight detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colLessCts;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colNetCts) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colNetCts) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colNetCts).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Final Carat detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colNetCts;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRate) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRate) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colRate).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter packet Rate detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colRate;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colDisPer) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colDisPer) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colDisPer).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter Disc (%) detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colDisPer;
                e.Valid = false;
            }
            else if (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCVDCharge) == null || (grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCVDCharge) != null && grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCVDCharge).ToString().Trim().Length == 0))
            {
                e.ErrorText = "Please enter CVD charge detail.";
                grvPurchaseDetails.FocusedRowHandle = e.RowHandle;
                grvPurchaseDetails.FocusedColumn = colCVDCharge;
                e.Valid = false;
            }
            else
            {
                //if (MessageBox.Show("Do you want add more Items...???", "confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                //{
                //    if (grvPurchaseItems.ValidateEditor())
                //        grvPurchaseItems.UpdateCurrentRow();
                //    //grvPurchaseItems.ValidateEditor();
                //    grvPurchaseItems.Focus();
                //    txtDueDays.Select();
                //    txtDueDays.Focus();
                //}                    
            }
        }

        private async void GetLessWeightDetailBasedOnCity(string GroupName, decimal Weight, int GridRowIndex, decimal TipWeight, decimal CVDWeight)
        {
            try
            {
                if (!string.IsNullOrEmpty(GroupName) && Weight > 0)
                {
                    LessWeightMasterRepository lessWeightMasterRepository = new LessWeightMasterRepository();
                    LessWeightDetails lessWeightDetails = await lessWeightMasterRepository.GetLessWeightDetailsMasters(GroupName,Weight);
                    
                    if (lessWeightDetails != null)
                    {
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colTipWeight, TipWeight.ToString());
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colCVDCharge, CVDWeight.ToString());
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colLessCts, lessWeightDetails.LessWeight.ToString());
                    }
                    else
                    {
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colTipWeight, "");
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colCVDWeight, "");
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colLessCts, "");
                    }
                }
            }
            catch
            {
            }
        }

        public void CalculateRejectionValue(bool IsCalculateRate, int GridRowIndex)
        {
            try
            {
                this.grvPurchaseDetails.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.grvPurchaseDetails_CellValueChanged);
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejPer).ToString().Trim().Length == 0)
                    grvPurchaseDetails.SetRowCellValue(GridRowIndex, colRejPer, "0");
                decimal RunningWeightBeforeRejection = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCarat)) - Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colTipWeight)) - Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCVDWeight));
                if (IsCalculateRate)
                {
                    try
                    {
                        if (RunningWeightBeforeRejection != 0 && grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejPer).ToString().Trim().Length != 0)
                        {
                            decimal RejectionWeight = RunningWeightBeforeRejection + ((RunningWeightBeforeRejection * Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejPer))) / 100);
                            //txtRejWeight.Text = (RejectionWeight - RunningWeight).ToString("0.000");
                            double multiplier = Math.Pow(10, 2);
                            grvPurchaseDetails.SetRowCellValue(GridRowIndex, colRejCts, (Math.Ceiling((RejectionWeight - RunningWeightBeforeRejection) * (decimal)multiplier) / (decimal)multiplier).ToString());
                            //txtRejWeight.Text = (Math.Ceiling((RejectionWeight - RunningWeightBeforeRejection) * (decimal)multiplier) / (decimal)multiplier).ToString();
                        }
                    }
                    catch
                    {
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colRejCts, "");
                        //txtRejWeight.Text = "";
                    }
                }
                else
                {
                    try
                    {
                        if (RunningWeightBeforeRejection != 0 && grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejCts).ToString().Trim().Length != 0)
                        {
                            decimal RejectionPer = ((Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejCts)) - RunningWeightBeforeRejection) / RunningWeightBeforeRejection) * 100;
                            grvPurchaseDetails.SetRowCellValue(GridRowIndex, colRejPer, (100 - (RejectionPer > 0 ? RejectionPer : (RejectionPer * -1))).ToString("0.00"));
                            //txtRejPer.Text = (100 - (RejectionPer > 0 ? RejectionPer : (RejectionPer * -1))).ToString("0.00");
                        }
                    }
                    catch
                    {
                        grvPurchaseDetails.SetRowCellValue(GridRowIndex, colRejPer, "");
                        //txtRejPer.Text = "";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                this.grvPurchaseDetails.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.grvPurchaseDetails_CellValueChanged);
            }
        }

        private void FinalCalculation(int GridRowIndex)
        {
            try
            {
                this.grvPurchaseDetails.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.grvPurchaseDetails_CellValueChanged);
                decimal Carat = 0;
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCarat).ToString().Length != 0)
                    Carat = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCarat));

                decimal TipCts = 0;
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colTipWeight).ToString().Length != 0)
                    TipCts = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colTipWeight));

                decimal CVDCts = 0;
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCVDWeight).ToString().Length != 0)
                    CVDCts = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCVDWeight));

                decimal RejCts = 0;
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejCts).ToString().Length != 0)
                    RejCts = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRejCts));

                decimal LessCts = 0;
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colLessCts).ToString().Length != 0)
                    LessCts = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colLessCts));

                ItemRunningWeight = Carat - TipCts - CVDCts - RejCts - LessCts;

                grvPurchaseDetails.SetRowCellValue(GridRowIndex, colNetCts, ItemRunningWeight);
                //txtNetWeight.Text = ItemRunningWeight.ToString();

                decimal Amount = 0;
                decimal FinalAmount = 0;
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRate).ToString().Trim().Length > 0)
                {
                    Amount = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colRate));
                    FinalAmount = ItemRunningWeight * Amount;
                }
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colDisPer).ToString().Trim().Length > 0)
                {
                    decimal LessPer = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colDisPer));
                    if (LessPer < 0)
                        LessPer *= -1;
                    decimal LessAmt = (FinalAmount * LessPer) / 100;
                    FinalAmount -= LessAmt;

                    grvPurchaseDetails.SetRowCellValue(GridRowIndex, colDisAmount, LessAmt);
                }
                if (grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCVDCharge).ToString().Trim().Length > 0)
                {
                    decimal CVDCharge = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(GridRowIndex, colCVDCharge));
                    CVDCharge *= Carat;
                    FinalAmount = FinalAmount - CVDCharge;

                    grvPurchaseDetails.SetRowCellValue(GridRowIndex, colCVDAmount, CVDCharge);
                }
                ItemFinalAmount = FinalAmount;
                //txtAmount.Text = ItemFinalAmount.ToString("0.00");
                grvPurchaseDetails.SetRowCellValue(GridRowIndex, colAmount, ItemFinalAmount);

                decimal currRate = 1;
                if (txtCurrencyType.Text.Trim().Length > 0 && Convert.ToDecimal(txtCurrencyType.Text) > 0)
                {
                    currRate = Convert.ToDecimal(txtCurrencyType.Text);
                }
                grvPurchaseDetails.SetRowCellValue(GridRowIndex, colCurrRate, (Amount / currRate).ToString("0.00"));
                grvPurchaseDetails.SetRowCellValue(GridRowIndex, colCurrAmount, (ItemFinalAmount / currRate).ToString("0.00"));
            }
            catch (StackOverflowException ex)
            {
                //txtAmount.Text = "";
            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.grvPurchaseDetails.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.grvPurchaseDetails_CellValueChanged);
            }
        }

        public void GetTotal()
        {
            try
            {
                decimal Total = Convert.ToDecimal(colAmount.SummaryItem.SummaryValue);
                decimal TotalAmount = 0;
                txtAmount.Text = Total.ToString("0.00");
                decimal LastDigit = 0;
                if (Total > 0)
                {
                    int StartIndex = Total.ToString().IndexOf(".") - 1;
                    int EndIndex = Total.ToString().Length - StartIndex;
                    if (StartIndex >= 0)
                        LastDigit = Convert.ToDecimal(Total.ToString().Substring(StartIndex, EndIndex));
                }
                decimal RoundUpAmt = 0;
                if (LastDigit >= 0 && LastDigit <= 5.50m)
                {
                    LastDigit *= -1;
                    RoundUpAmt = LastDigit;
                }
                else
                {
                    RoundUpAmt = 10 - LastDigit;
                }

                Total += RoundUpAmt;
                txtNetAmount.Text = Total.ToString("0.00");
                txtCurrencyAmount.Text = GetCurrencyTotalAmount(Total).ToString("0.00");
                txtRoundAmount.Text = RoundUpAmt.ToString();

                CalculateBrokerageRate(true);
                CalculateCommisionRate(true);
            }
            catch
            {
            }
        }

        private decimal GetCurrencyTotalAmount(decimal Amt)
        {
            try
            {
                if (Amt != 0)
                {
                    if (txtCurrencyType.Text.ToString().Length == 0)
                        txtCurrencyType.Text = "0";
                }
                return (Amt / Convert.ToDecimal(txtCurrencyType.Text));
            }
            catch
            {
                return 0;
            }
        }

        public void CalculateBrokerageRate(bool IsCalculateRate)
        {
            try
            {
                if (txtBrokerPer.Text.ToString().Trim().Length == 0)
                    txtBrokerPer.Text = "0";
                if (IsCalculateRate)
                {
                    try
                    {
                        if (Convert.ToDecimal(txtNetAmount.Text) != 0 && txtBrokerPer.Text.Trim().Length != 0)
                        {
                            decimal BrokerageAmount = Convert.ToDecimal(txtNetAmount.Text) + ((Convert.ToDecimal(txtNetAmount.Text) * Convert.ToDecimal(txtBrokerPer.Text)) / 100);
                            double multiplier = Math.Pow(10, 2);
                            txtBrokerageAmount.Text = (Math.Ceiling((BrokerageAmount - Convert.ToDecimal(txtNetAmount.Text)) * (decimal)multiplier) / (decimal)multiplier).ToString();
                        }
                    }
                    catch
                    {
                        txtBrokerageAmount.Text = "";
                    }
                }
                else
                {
                    try
                    {
                        if (Convert.ToDecimal(txtNetAmount.Text) != 0 && txtBrokerageAmount.Text.Trim().Length != 0)
                        {
                            decimal BrokeragePer = ((Convert.ToDecimal(txtBrokerageAmount.Text) - Convert.ToDecimal(txtNetAmount.Text)) / Convert.ToDecimal(txtNetAmount.Text)) * 100;
                            txtBrokerPer.Text = (100 - (BrokeragePer > 0 ? BrokeragePer : (BrokeragePer * -1))).ToString("0.00");
                        }
                    }
                    catch
                    {
                        txtBrokerPer.Text = "";
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        public void CalculateCommisionRate(bool IsCalculateRate)
        {
            try
            {
                if (txtBuyerCommisionPer.Text.ToString().Trim().Length == 0)
                    txtBuyerCommisionPer.Text = "0";
                if (IsCalculateRate)
                {
                    try
                    {
                        if (Convert.ToDecimal(txtNetAmount.Text) != 0 && txtBuyerCommisionPer.Text.Trim().Length != 0)
                        {
                            decimal CommisionAmount = Convert.ToDecimal(txtNetAmount.Text) + ((Convert.ToDecimal(txtNetAmount.Text) * Convert.ToDecimal(txtBuyerCommisionPer.Text)) / 100);
                            double multiplier = Math.Pow(10, 2);
                            txtCommisionAmount.Text = (Math.Ceiling((CommisionAmount - Convert.ToDecimal(txtNetAmount.Text)) * (decimal)multiplier) / (decimal)multiplier).ToString();
                        }
                    }
                    catch
                    {
                        txtCommisionAmount.Text = "";
                    }
                }
                else
                {
                    try
                    {
                        if (Convert.ToDecimal(txtNetAmount.Text) != 0 && txtCommisionAmount.Text.Trim().Length != 0)
                        {
                            decimal CommisionPer = ((Convert.ToDecimal(txtCommisionAmount.Text) - Convert.ToDecimal(txtNetAmount.Text)) / Convert.ToDecimal(txtNetAmount.Text)) * 100;
                            txtBuyerCommisionPer.Text = (100 - (CommisionPer > 0 ? CommisionPer : (CommisionPer * -1))).ToString("0.00");
                        }
                    }
                    catch
                    {
                        txtBuyerCommisionPer.Text = "";
                    }
                }
            }
            catch
            {
            }
            finally
            {
            }
        }

        private void txtBuyerCommisionPer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalculateCommisionRate(true);
            }
        }

        private void txtBuyerCommisionPer_Leave(object sender, EventArgs e)
        {
            CalculateCommisionRate(true);
        }

        private void txtCommisionAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //CalculateCommisionRate(false);
            }
        }

        private void txtCommisionAmount_Leave(object sender, EventArgs e)
        {
            //CalculateCommisionRate(false);
        }

        private void txtBrokerPer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalculateBrokerageRate(true);
            }
        }

        private void txtBrokerPer_Leave(object sender, EventArgs e)
        {
            CalculateBrokerageRate(true);
        }

        private void txtBrokerageAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //CalculateBrokerageRate(false);
            }
        }

        private void txtBrokerageAmount_Leave(object sender, EventArgs e)
        {
            //CalculateBrokerageRate(false);
        }

        private Image LoadImage()
        {
            Image newimage=null;
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.JPEG;*.PNG)|*.BMP;*.JPG;*.JPEG;*.PNG";
            openFileDialog1.FileName = string.Empty;
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                if (openFileDialog1.FileName != string.Empty)
                {
                    try
                    {
                        Byte[] logo = null;
                        logo = File.ReadAllBytes(openFileDialog1.FileName);
                        MemoryStream ms = new MemoryStream(logo);
                        newimage = Image.FromStream(ms);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("CM01:" + ex.Message, "AD InfoTech", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            return newimage;
        }

        private void BrowseImage(int SelectedImage)
        {
            Transaction.FrmTakePicture fpc = new Transaction.FrmTakePicture();
            fpc.Image1.Image = Image1.Image;
            fpc.Image2.Image = Image2.Image;
            fpc.Image3.Image = Image3.Image;
            fpc.SelectedImage = SelectedImage;
            if (fpc.ShowDialog() == DialogResult.OK)
            {
                Image1.Image = fpc.Image1.Image;
                Image2.Image = fpc.Image2.Image;
                Image3.Image = fpc.Image3.Image;

                Image1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                Image2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
                Image3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            }
        }

        private void Image1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Image1.Image = LoadImage();
            //Image1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            BrowseImage(0);
        }

        private void Image2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Image2.Image = LoadImage();
            //Image2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            BrowseImage(1);
        }

        private void Image3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Image3.Image = LoadImage();
            //Image3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            BrowseImage(2);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!CheckValidation())
                    return;

                string PurchaseId = Guid.NewGuid().ToString();
                //string PurchaseDetailsId = Guid.NewGuid().ToString();

                List<PurchaseDetails> purchaseDetailsList = new List<PurchaseDetails>();
                PurchaseDetails purchaseDetails = new PurchaseDetails();
                for (int i = 0; i < grvPurchaseDetails.RowCount; i++)
                {
                    purchaseDetails = new PurchaseDetails();
                    purchaseDetails.Id = Guid.NewGuid().ToString();
                    purchaseDetails.PurchaseId = PurchaseId;
                    purchaseDetails.KapanId = grvPurchaseDetails.GetRowCellValue(i, colKapan).ToString();
                    purchaseDetails.ShapeId = grvPurchaseDetails.GetRowCellValue(i, colShape).ToString();
                    purchaseDetails.SizeId = grvPurchaseDetails.GetRowCellValue(i, colSize).ToString();
                    purchaseDetails.PurityId = grvPurchaseDetails.GetRowCellValue(i, colPurity).ToString();

                    purchaseDetails.Weight = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colCarat).ToString());
                    purchaseDetails.TIPWeight = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colTipWeight).ToString());
                    purchaseDetails.CVDWeight = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colCVDWeight).ToString());
                    purchaseDetails.RejectedPercentage = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colRejPer).ToString());
                    purchaseDetails.RejectedWeight = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colRejCts).ToString());
                    purchaseDetails.LessWeight = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colLessCts).ToString());
                    purchaseDetails.LessDiscountPercentage = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colDisPer).ToString());
                    purchaseDetails.LessWeightDiscount= Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colDisAmount).ToString());
                    purchaseDetails.NetWeight = Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(i, colNetCts).ToString());
                    purchaseDetails.BuyingRate = float.Parse(grvPurchaseDetails.GetRowCellValue(i, colRate).ToString());
                    purchaseDetails.CVDCharge = float.Parse(grvPurchaseDetails.GetRowCellValue(i, colCVDCharge).ToString());
                    purchaseDetails.CVDAmount = float.Parse(grvPurchaseDetails.GetRowCellValue(i, colCVDAmount).ToString());
                    purchaseDetails.Amount = float.Parse(grvPurchaseDetails.GetRowCellValue(i, colAmount).ToString());
                    purchaseDetails.CurrencyRate = float.Parse(grvPurchaseDetails.GetRowCellValue(i, colCurrRate).ToString());
                    purchaseDetails.CurrencyAmount = float.Parse(grvPurchaseDetails.GetRowCellValue(i, colCurrAmount).ToString());
                    purchaseDetails.IsTransfer = false;
                    purchaseDetails.TransferParentId = null;
                    purchaseDetails.CreatedDate = DateTime.Now;
                    purchaseDetails.CreatedBy = Common.LoginUserID;
                    purchaseDetails.UpdatedDate = DateTime.Now;
                    purchaseDetails.UpdatedBy = Common.LoginUserID;

                    purchaseDetailsList.Insert(i, purchaseDetails);
                }

                PurchaseMaster purchaseMaster = new PurchaseMaster();
                purchaseMaster.Id = PurchaseId;
                purchaseMaster.CompanyId = lueCompany.GetColumnValue("Id").ToString();
                purchaseMaster.BranchId = lueBranch.GetColumnValue("Id").ToString();
                purchaseMaster.PartyId = lueParty.GetColumnValue("Id").ToString();
                purchaseMaster.ByuerId = lueBuyer.GetColumnValue("Id").ToString();
                purchaseMaster.CurrencyId = lueCurrencyType.GetColumnValue("Id").ToString();
                purchaseMaster.FinancialYearId = Common.LoginFinancialYear;
                purchaseMaster.BrokerageId = lueBroker.GetColumnValue("Id").ToString();
                purchaseMaster.CurrencyRate = Convert.ToDecimal(txtCurrencyType.Text);
                purchaseMaster.PurchaseBillNo = Convert.ToInt32(txtSerialNo.Text);
                purchaseMaster.SlipNo = Convert.ToInt32(txtSlipNo.Text);
                purchaseMaster.TransactionType = Convert.ToInt32(luePaymentMode.GetColumnValue("Id"));
                purchaseMaster.Date = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                purchaseMaster.Time = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                purchaseMaster.DayName = Convert.ToDateTime(dtDate.EditValue).DayOfWeek.ToString();
                purchaseMaster.PartyLastBalanceWhilePurchase = float.Parse(txtPartyBalance.Text);
                purchaseMaster.BrokerPercentage = Convert.ToDecimal(txtBrokerPer.Text);
                purchaseMaster.BrokerAmount = float.Parse(txtBrokerageAmount.Text);
                purchaseMaster.RoundUpAmount = float.Parse(txtRoundAmount.Text);
                purchaseMaster.Total = float.Parse(txtAmount.Text);
                purchaseMaster.GrossTotal = float.Parse(txtNetAmount.Text);
                purchaseMaster.DueDays = Convert.ToInt32(txtDays.Text);
                purchaseMaster.DueDate = Convert.ToDateTime(dtDate.Text).AddDays(Convert.ToInt32(txtDays.Text));
                purchaseMaster.PaymentDays = Convert.ToInt32(txtDays.Text);
                purchaseMaster.PaymentDueDate = Convert.ToDateTime(dtPayDate.Text);
                purchaseMaster.IsSlip = tglSlip.IsOn;
                purchaseMaster.IsPF = tglPF.IsOn;
                purchaseMaster.CommissionPercentage = Convert.ToDecimal(txtBuyerCommisionPer.Text);
                purchaseMaster.CommissionAmount = float.Parse(txtCommisionAmount.Text);
                if (Image1.Image != null)
                    purchaseMaster.Image1 = ImageToByteArray(Image1.Image);
                if (Image2.Image != null)
                    purchaseMaster.Image2 = ImageToByteArray(Image2.Image);
                if (Image3.Image != null)
                    purchaseMaster.Image3 = ImageToByteArray(Image3.Image);
                purchaseMaster.AllowSlipPrint = tglSlip.IsOn ? true : false;
                purchaseMaster.IsTransfer = false;
                purchaseMaster.TransferParentId = null;
                purchaseMaster.IsDelete = false;
                purchaseMaster.Remarks = txtRemark.Text;
                purchaseMaster.CreatedDate = DateTime.Now;
                purchaseMaster.CreatedBy = Common.LoginUserID;
                purchaseMaster.UpdatedDate = DateTime.Now;
                purchaseMaster.UpdatedBy = Common.LoginUserID;
                purchaseMaster.PurchaseDetails = purchaseDetailsList;

                PurchaseMasterRepository purchaseMasterRepository = new PurchaseMasterRepository();
                var Result = await purchaseMasterRepository.AddPurchaseAsync(purchaseMaster);

                if (Result != null)
                {
                    Reset();
                    MessageBox.Show(AppMessages.GetString(AppMessageID.SaveSuccessfully), "[" + this.Text + "]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error : " + Ex.Message.ToString(), "[" + this.Text + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private bool CheckValidation()
        {
            if (lueCompany.EditValue == null)
            {
                MessageBox.Show("Please select Company", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueCompany.Focus();
                return false;
            }
            else if (txtSlipNo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter Slip No", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSlipNo.Focus();
                return false;
            }
            else if (luePaymentMode.EditValue == null)
            {
                MessageBox.Show("Please select Payment Mode", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                luePaymentMode.Focus();
                return false;
            }
            else if (lueCurrencyType.EditValue == null)
            {
                MessageBox.Show("Please select Currency Type", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueCurrencyType.Focus();
                return false;
            }
            else if (lueBranch.EditValue == null)
            {
                MessageBox.Show("Please select Branch", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueBranch.Focus();
                return false;
            }
            else if (lueBuyer.EditValue == null)
            {
                MessageBox.Show("Please select buyer name", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueBuyer.Focus();
                return false;
            }
            else if (lueParty.EditValue == null)
            {
                MessageBox.Show("Please select Party name", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueParty.Focus();
                return false;
            }
            else if (lueBroker.EditValue == null)
            {
                MessageBox.Show("Please select broker name", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueBroker.Focus();
                return false;
            }
            else if (txtDays.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter bill due days", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDays.Focus();
                return false;
            }
            else if (txtPaymentDays.Text.Trim().Length == 0)
            {
                MessageBox.Show("Please enter bill payment due days", "Purchase Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPaymentDays.Focus();
                return false;
            }

            if (txtCurrencyAmount.Text.ToString().Length == 0)
                txtCurrencyAmount.Text = "1";

            return true;
        }

        private void Reset()
        {
            grdPurchaseDetails.DataSource = null;
            FillCombos();
            //FillBranches();
            FillCurrency();
            txtRemark.Text = "";
            txtDays.Text = "";
            txtPaymentDays.Text = "";
            dtPayDate.EditValue = DateTime.Today;
            Image1.Image = null;
            Image2.Image = null;
            Image3.Image = null;
            txtBuyerCommisionPer.Text = "0";
            txtPartyBalance.Text = "0";
            txtBrokerPer.Text = "0";
            txtBrokerageAmount.Text = "0";
            txtCommisionAmount.Text = "0";
            txtAmount.Text = "0";
            txtRoundAmount.Text = "0";
            txtNetAmount.Text = "0";
            txtCurrencyAmount.Text = "0";
            tglSlip.IsOn = Common.PrintPurchaseSlip;
            tglPF.IsOn = Common.PrintPurchasePF;
            GetPurchaseNo();
            txtSlipNo.Focus();
        }

        private async void lueBroker_EditValueChanged(object sender, EventArgs e)
        {
            var selectedBoker = (PartyMaster)lueBroker.GetSelectedDataRow();
            var brokerageDetail  = await _brokerageMasterRepository.GetBrokerageAsync(selectedBoker.BrokerageId);
            txtBrokerPer.Text = brokerageDetail != null ? brokerageDetail.Percentage.ToString() : "0";
        }

        private async void lueBuyer_EditValueChanged(object sender, EventArgs e)
        {
            var selectedBuyer = (PartyMaster)lueBuyer.GetSelectedDataRow();
            var brokerageDetail = await _brokerageMasterRepository.GetBrokerageAsync(selectedBuyer.BrokerageId);
            txtBuyerCommisionPer.Text =brokerageDetail != null ?  brokerageDetail.Percentage.ToString() : "0";
        }

        private void lueParty_EditValueChanged(object sender, EventArgs e)
        {
            var selectedParty = (PartyMaster)lueParty.GetSelectedDataRow();
            txtPartyBalance.Text = selectedParty.OpeningBalance.ToString();
        }

        private void lueBranch_EditValueChanged(object sender, EventArgs e)
        {
            GetPurchaseNo(false);
        }

        private void lueCompany_EditValueChanged(object sender, EventArgs e)
        {
            this.Text = "PURCHASE - " + lueCompany.Text + " - [" + Common.LoginFinancialYearName + "]";

            LoadBranch(lueCompany.EditValue.ToString());

            FillCombos();
        }

        private void grpGroup9_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}