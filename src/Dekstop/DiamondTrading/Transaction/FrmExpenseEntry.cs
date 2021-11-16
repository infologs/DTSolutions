﻿using DevExpress.XtraEditors;
using EFCore.SQL.Repository;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiamondTrading.Transaction
{
    public partial class FrmExpenseEntry : DevExpress.XtraEditors.XtraForm
    {
        private readonly CompanyMasterRepository _companyMasterRepository;
        private readonly BranchMasterRepository _branchMasterRepository;
        private readonly PartyMasterRepository _partyMasterRepository;
        private ExpenseMasterRepository _expenseMaterRepository;
        int _paymentType = 0;

        public FrmExpenseEntry()
        {
            InitializeComponent();

            _companyMasterRepository = new CompanyMasterRepository();
            _branchMasterRepository = new BranchMasterRepository();
            _partyMasterRepository = new PartyMasterRepository();
            _expenseMaterRepository = new ExpenseMasterRepository();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblFormTitle_Click(object sender, EventArgs e)
        {

        }

        private async void FrmPaymentEntry_Load(object sender, EventArgs e)
        {
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;

            await GetMaxSrNo();
            await LoadCompany();
            LoadLedgers(lueCompany.EditValue.ToString());
        }

        private static DataTable GetDTColumnsForPaymentDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Party");
            dt.Columns.Add("Amount");
            return dt;
        }

        private async Task GetMaxSrNo()
        {
            var SrNo = await _expenseMaterRepository.GetMaxSrNoAsync(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear);
            txtSerialNo.Text = SrNo.ToString();
        }

        private void SetThemeColors(Color color)
        {
            if (!color.ToArgb().ToString().Equals(Color.FromArgb(0).Name))
            {
                grpGroup1.AppearanceCaption.BorderColor = color;
                grpGroup2.AppearanceCaption.BorderColor = color;

                //txtLedgerBalance.BackColor = color;
            }
        }

        private async Task LoadCompany()
        {
            var result = await _companyMasterRepository.GetAllCompanyAsync();
            lueCompany.Properties.DataSource = result;
            lueCompany.Properties.DisplayMember = "Name";
            lueCompany.Properties.ValueMember = "Id";
            lueCompany.EditValue = Common.LoginCompany;

            await LoadBranch(lueCompany.EditValue.ToString());
        }

        private async Task LoadBranch(string CompanyId)
        {
            var result = await _branchMasterRepository.GetAllBranchAsync(CompanyId);
            lueBranch.Properties.DataSource = result;
            lueBranch.Properties.DisplayMember = "Name";
            lueBranch.Properties.ValueMember = "Id";
            lueBranch.EditValue = Common.LoginBranch;
        }

        private async void LoadLedgers(string companyId)
        {
            grdPaymentDetails.DataSource = GetDTColumnsForPaymentDetails();
            var result = await _partyMasterRepository.GetAllPartyAsync(companyId, PartyTypeMaster.Expense);

            repoParty.DataSource = result;
            repoParty.DisplayMember = "Name";
            repoParty.ValueMember = "Id";
        }

        private void grvPurchaseDetails_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            //string ControlName = ((DevExpress.XtraEditors.LookUpEdit)sender).Name;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool IsSucess = false;
                try
                {
                    for (int i = 0; i < grvPaymentDetails.RowCount; i++)
                    {
                        ExpenseDetails expenseDetails = new ExpenseDetails
                        {
                            Id = Guid.NewGuid().ToString(),
                            SrNo = Convert.ToInt32(txtSerialNo.Text),
                            BranchId = Common.LoginBranch,
                            CompanyId = lueCompany.EditValue.ToString(),
                            FinancialYearId = Common.LoginFinancialYear,
                            PartyId = grvPaymentDetails.GetRowCellValue(i, colParty).ToString(),
                            Amount = float.Parse(grvPaymentDetails.GetRowCellValue(i, colAmount).ToString()),
                            IsDelete = false,
                            Remarks = txtRemark.Text,
                            CreatedBy = Common.LoginUserID,
                            CreatedDate = DateTime.Now,
                            UpdatedBy = Common.LoginUserID,
                            UpdatedDate = DateTime.Now,
                        };

                        var result = await _expenseMaterRepository.AddExpenseAsync(expenseDetails);
                        IsSucess = true;
                    }
                }
                catch(Exception Ex)
                {
                    IsSucess = false;
                }

                if (IsSucess)
                {
                    Reset();
                    MessageBox.Show(AppMessages.GetString(AppMessageID.SaveSuccessfully), "[" + this.Text + "}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error : " + Ex.Message.ToString(), "[" + this.Text + "}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private async void Reset()
        {
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;
            grdPaymentDetails.DataSource = null;
            txtRemark.Text = "";
            lueBranch.EditValue = null;
            await GetMaxSrNo();
            LoadLedgers(lueCompany.EditValue.ToString());
            lueCompany.Focus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void lueCompany_EditValueChanged(object sender, EventArgs e)
        {
            if (lueCompany.EditValue != null)
                LoadLedgers(lueCompany.EditValue.ToString());
        }

        private void FrmPaymentEntry_KeyDown(object sender, KeyEventArgs e)
        {
            Common.MoveToNextControl(sender, e, this);
        }
    }
}