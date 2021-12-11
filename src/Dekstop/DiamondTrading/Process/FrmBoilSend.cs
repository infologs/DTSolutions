﻿using DevExpress.XtraEditors;
using EFCore.SQL.Repository;
using Repository.Entities;
using Repository.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiamondTrading.Process
{
    public partial class FrmBoilSend : DevExpress.XtraEditors.XtraForm
    {
        BoilMasterRepository _boilMasterRepository;
        PartyMasterRepository _partyMasterRepository;
        public FrmBoilSend()
        {
            InitializeComponent();
            _boilMasterRepository = new BoilMasterRepository();
            _partyMasterRepository = new PartyMasterRepository();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetThemeColors(Color color)
        {
            if (!color.ToArgb().ToString().Equals(Color.FromArgb(0).Name))
            {
                grpGroup1.AppearanceCaption.BorderColor = color;
                grpGroup2.AppearanceCaption.BorderColor = color;
            }
        }

        private async void FrmBoilSend_Load(object sender, EventArgs e)
        {
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;

            SetThemeColors(Color.FromArgb(250, 243, 197));

            await GetMaxSrNo();
            await GetEmployeeList();
            await GetKapanDetail();
        }

        private async Task GetMaxSrNo()
        {
            var SrNo = await _boilMasterRepository.GetMaxSrNoAsync(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear,0);
            txtSerialNo.Text = SrNo.ToString();
        }

        private async Task GetEmployeeList()
        {
            var EmployeeDetailList = await _partyMasterRepository.GetAllPartyAsync(Common.LoginCompany.ToString(), PartyTypeMaster.Employee, PartyTypeMaster.Other);
            lueReceiveFrom.Properties.DataSource = EmployeeDetailList;
            lueReceiveFrom.Properties.DisplayMember = "Name";
            lueReceiveFrom.Properties.ValueMember = "Id";

            lueSendto.Properties.DataSource = EmployeeDetailList;
            lueSendto.Properties.DisplayMember = "Name";
            lueSendto.Properties.ValueMember = "Id";
        }

        private async Task GetKapanDetail()
        {
            KapanMasterRepository kapanMasterRepository = new KapanMasterRepository();
            var kapanMaster = await kapanMasterRepository.GetAssortProcessKapanDetails(Common.LoginCompany,Common.LoginBranch);
            lueKapan.Properties.DataSource = kapanMaster;
            lueKapan.Properties.DisplayMember = "Name";
            lueKapan.Properties.ValueMember = "Id";
        }

        private async Task GetBoilProcessSendDetail()
        {
            if (lueKapan.EditValue != null)
            {
                grdParticularsDetails.DataSource = GetDTColumnsforParticularDetails();
                List<BoilProcessSend> ListAssortmentProcessSend = await _boilMasterRepository.GetBoilSendToDetails(lueKapan.EditValue.ToString(),Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear.ToString());
                repoSlipNo.DataSource = ListAssortmentProcessSend;
                repoSlipNo.DisplayMember = "SlipNo";
                repoSlipNo.ValueMember = "Id";

                repoSlipNo.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                repoSlipNo.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            }
            else
            {
                grdParticularsDetails.DataSource = null;
            }
        }

        private static DataTable GetDTColumnsforParticularDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlipNo");
            dt.Columns.Add("Size");
            dt.Columns.Add("AvailableWeight");
            dt.Columns.Add("BoilCarat");
            dt.Columns.Add("SizeId");
            dt.Columns.Add("ShapeId");
            dt.Columns.Add("PurityId");
            dt.Columns.Add("PurchaseDetailsId");
            dt.Columns.Add("SlipNo1");
            return dt;
        }

        private async void lueKapan_EditValueChanged(object sender, EventArgs e)
        {
            await GetBoilProcessSendDetail();
        }

        private void grvParticularsDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colSlipNo)
                {
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).Size);
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colACarat, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colSlipNo1, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                    grvParticularsDetails.SetRowCellValue(e.RowHandle, colPurchaseDetailsId, ((Repository.Entities.Models.BoilProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).PurchaseDetailsId);
                    //grvPurchaseItems.FocusedRowHandle = e.RowHandle;
                    //grvPurchaseItems.FocusedColumn = colBoilCarat;
                }
            }
            catch
            {

            }
        }

        private void FrmBoilSend_KeyDown(object sender, KeyEventArgs e)
        {
            Common.MoveToNextControl(sender, e, this);
        }

        private bool CheckValidation()
        {
            if (lueReceiveFrom.EditValue == null)
            {
                MessageBox.Show("Please select Receive from name", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueReceiveFrom.Focus();
                return false;
            }
            else if (lueSendto.EditValue == null)
            {
                MessageBox.Show("Please select Send to name", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueSendto.Focus();
                return false;
            }
            if (lueKapan.EditValue == null)
            {
                MessageBox.Show("Please select Kapan", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueKapan.Focus();
                return false;
            }
            else if (grvParticularsDetails.RowCount == 0)
            {
                MessageBox.Show("Please select Particulars Details", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                grvParticularsDetails.Focus();
                return false;
            }
            return true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
  
            {
                this.Cursor = Cursors.WaitCursor;

                if (!CheckValidation())
                    return;

                BoilProcessMaster boilProcessMaster = new BoilProcessMaster();
                bool IsSuccess = false;
                try
                {
                    for (int i = 0; i < grvParticularsDetails.RowCount; i++)
                    {
                        boilProcessMaster = new BoilProcessMaster();
                        boilProcessMaster.Id = Guid.NewGuid().ToString();
                        boilProcessMaster.PurchaseDetailsId = grvParticularsDetails.GetRowCellValue(i, colPurchaseDetailsId).ToString();
                        boilProcessMaster.BoilNo = Convert.ToInt32(txtSerialNo.Text);
                        boilProcessMaster.JangadNo = Convert.ToInt32(txtSerialNo.Text);
                        boilProcessMaster.CompanyId = Common.LoginCompany;
                        boilProcessMaster.BranchId = Common.LoginBranch;
                        boilProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        boilProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        boilProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        boilProcessMaster.BoilType = Convert.ToInt32(ProcessType.Send);
                        boilProcessMaster.KapanId = lueKapan.EditValue.ToString();
                        boilProcessMaster.ShapeId = grvParticularsDetails.GetRowCellValue(i, colShapeId).ToString();
                        boilProcessMaster.SizeId = grvParticularsDetails.GetRowCellValue(i, colSizeId).ToString();
                        boilProcessMaster.PurityId = grvParticularsDetails.GetRowCellValue(i, colPurityId).ToString();
                        boilProcessMaster.Weight = Convert.ToDecimal(grvParticularsDetails.GetRowCellValue(i, colBoilCarat).ToString());
                        boilProcessMaster.LossWeight = 0;
                        boilProcessMaster.RejectionWeight = 0;
                        boilProcessMaster.HandOverById = lueReceiveFrom.EditValue.ToString();
                        boilProcessMaster.HandOverToId = lueSendto.EditValue.ToString();
                        boilProcessMaster.SlipNo = grvParticularsDetails.GetRowCellValue(i, colSlipNo1).ToString();
                        boilProcessMaster.BoilCategoy = 0;
                        boilProcessMaster.Remarks = txtRemark.Text;
                        boilProcessMaster.IsDelete = false;
                        boilProcessMaster.CreatedDate = DateTime.Now;
                        boilProcessMaster.CreatedBy = Common.LoginUserID;
                        boilProcessMaster.UpdatedDate = DateTime.Now;
                        boilProcessMaster.UpdatedBy = Common.LoginUserID;

                        var Result = await _boilMasterRepository.AddBoilAsync(boilProcessMaster);
                        IsSuccess = true;
                    }
                }
                catch
                {
                    IsSuccess = false;
                }

                if (IsSuccess)
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

        private async void Reset()
        {
            grdParticularsDetails.DataSource = null;
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;
            txtRemark.Text = "";
            lueReceiveFrom.EditValue = null;
            lueSendto.EditValue = null;
            lueKapan.EditValue = null;
            repoSlipNo.DataSource = null;

            await GetMaxSrNo();
            await GetEmployeeList();
            lueReceiveFrom.Select();
            lueReceiveFrom.Focus();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}