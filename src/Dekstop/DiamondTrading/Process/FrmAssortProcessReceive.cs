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
    public partial class FrmAssortProcessReceive : DevExpress.XtraEditors.XtraForm
    {
        AccountToAssortMasterRepository _accountToAssortMasterRepository;
        PartyMasterRepository _partyMasterRepository;
        List<CharniProcessSend> ListCharniProcessSend;
        List<GalaProcessSend> ListGalaProcessSend;

        public FrmAssortProcessReceive()
        {
            InitializeComponent();
            _accountToAssortMasterRepository = new AccountToAssortMasterRepository();
            _partyMasterRepository = new PartyMasterRepository();
        }

        private void SetThemeColors(Color color)
        {
            if (!color.ToArgb().ToString().Equals(Color.FromArgb(0).Name))
            {
                grpGroup1.AppearanceCaption.BorderColor = color;
                grpGroup2.AppearanceCaption.BorderColor = color;
            }
        }

        private async void FrmAssortProcessSend_Load(object sender, EventArgs e)
        {
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;

            SetThemeColors(Color.FromArgb(215, 246, 214));

            await GetMaxSrNo();
            GetDepartmentList();
            await GetEmployeeList();
            await GetKapanDetail();
        }

        private void GetDepartmentList()
        {
            var Department = DepartmentMaster1.GetAllDepartment();

            if (Department != null)
            {
                lueDepartment.Properties.DataSource = Department;
                lueDepartment.Properties.DisplayMember = "Name";
                lueDepartment.Properties.ValueMember = "Id";
            }
        }

        private async Task GetMaxSrNo()
        {
            if (lueDepartment.EditValue != null)
            {
                if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Boil)
                {
                    BoilMasterRepository boilMasterRepository = new BoilMasterRepository();
                    var SrNo = await boilMasterRepository.GetMaxSrNoAsync(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear, 2);
                    txtSerialNo.Text = SrNo.ToString();
                }
                else if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Charni)
                {
                    CharniProcessMasterRepository charniProcessMasterRepository = new CharniProcessMasterRepository();
                    var SrNo = await charniProcessMasterRepository.GetMaxSrNoAsync(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear, 2);
                    txtSerialNo.Text = SrNo.ToString();
                }
            }
            //var SrNo = await _accountToAssortMasterRepository.GetMaxSrNoAsync(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear);
            //txtSerialNo.Text = SrNo.ToString();
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
            var kapanMaster = await kapanMasterRepository.GetAllKapanAsync();
            lueKapan.Properties.DataSource = kapanMaster;
            lueKapan.Properties.DisplayMember = "Name";
            lueKapan.Properties.ValueMember = "Id";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void lueDepartment_EditValueChanged(object sender, EventArgs e)
        {
            if (lueDepartment.EditValue != null)
            {
                if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Boil)
                {
                    await GetMaxSrNo();
                    await GetBoilProcessReceivedDetail();
                    repoSlipNo.Columns["BoilNo"].Visible = true;
                    repoSlipNo.Columns["CharniSize"].Visible = false;
                    colCharniSize.Visible = false;
                }
                else if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Charni)
                {
                    await GetMaxSrNo();
                    await GetCharniProcessReceiveDetail();
                    repoSlipNo.Columns["BoilNo"].Visible = false;
                    repoSlipNo.Columns["CharniSize"].Visible = true;
                    colCharniSize.Visible = true;
                }

            }
        }

        private async Task GetBoilProcessReceivedDetail()
        {
            CharniProcessMasterRepository charniProcessMasterRepository = new CharniProcessMasterRepository();
            grdParticularsDetails.DataSource = GetDTColumnsforParticularDetails();
            ListCharniProcessSend = await charniProcessMasterRepository.GetCharniSendToDetails(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear.ToString());

            lueKapan.Properties.DataSource = ListCharniProcessSend.Select(x => new { x.KapanId, x.Kapan }).Distinct().ToList();
            lueKapan.Properties.DisplayMember = "Kapan";
            lueKapan.Properties.ValueMember = "KapanId";
        }

        private async Task GetCharniProcessReceiveDetail()
        {
            GalaProcessMasterRepository galaProcessMasterRepository = new GalaProcessMasterRepository();
            grdParticularsDetails.DataSource = GetDTColumnsforParticularDetails();
            ListGalaProcessSend = await galaProcessMasterRepository.GetGalaSendToDetails(Common.LoginCompany.ToString(), Common.LoginBranch.ToString(), Common.LoginFinancialYear.ToString());

            lueKapan.Properties.DataSource = ListGalaProcessSend.Select(x => new { x.KapanId, x.Kapan }).Distinct().ToList();
            lueKapan.Properties.DisplayMember = "Kapan";
            lueKapan.Properties.ValueMember = "KapanId";
        }

        private static DataTable GetDTColumnsforParticularDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BoilNo");
            dt.Columns.Add("SlipNo");
            dt.Columns.Add("Size");
            dt.Columns.Add("AvailableWeight");
            dt.Columns.Add("CharniCarat");
            dt.Columns.Add("SizeId");
            dt.Columns.Add("ShapeId");
            dt.Columns.Add("PurityId");
            dt.Columns.Add("BoilNo1");

            dt.Columns.Add("GalaCarat");
            dt.Columns.Add("SlipNo1");
            dt.Columns.Add("CharniSize");
            dt.Columns.Add("CharniSizeId");
            return dt;
        }

        private void lueKapan_EditValueChanged(object sender, EventArgs e)
        {
            if (lueDepartment.EditValue != null)
            {
                if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Boil && ListCharniProcessSend != null)
                {
                    repoSlipNo.DataSource = ListCharniProcessSend.Where(x => x.KapanId == lueKapan.EditValue.ToString()).ToList();
                    repoSlipNo.DisplayMember = "BoilNo";
                    repoSlipNo.ValueMember = "Id";

                    repoSlipNo.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                    repoSlipNo.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
                }
                else if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Charni && ListGalaProcessSend != null)
                {
                    repoSlipNo.DataSource = ListGalaProcessSend.Where(x => x.KapanId == lueKapan.EditValue.ToString()).ToList();
                    repoSlipNo.DisplayMember = "SlipNo";
                    repoSlipNo.ValueMember = "Id";

                    repoSlipNo.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                    repoSlipNo.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
                }
            }
        }

        private void grvParticularsDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colSrNo)
                {
                    if (lueDepartment.EditValue != null)
                    {
                        if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Boil)
                        {
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSlipNo, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colACarat, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colBoilNo1, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).BoilNo);
                        }
                        else if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Charni)
                        {
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSlipNo1, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSlipNo, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colACarat, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colCharniSize, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).CharniSize);
                            grvParticularsDetails.SetRowCellValue(e.RowHandle, colCharniSizeId, ((Repository.Entities.Models.GalaProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).CharniSizeId);
                        }
                        //grvPurchaseItems.FocusedRowHandle = e.RowHandle;
                        //grvPurchaseItems.FocusedColumn = colBoilCarat;
                    }
                }
            }
            catch
            {

            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!CheckValidation())
                    return;

                if (lueDepartment.EditValue != null)
                {
                    if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Boil)
                    {
                        string Cts = "0";

                        BoilMasterRepository boilMasterRepository = new BoilMasterRepository();
                        BoilProcessMaster boilProcessMaster = new BoilProcessMaster();
                        bool IsSuccess = false;
                        try
                        {
                            for (int i = 0; i < grvParticularsDetails.RowCount; i++)
                            {
                                Cts = grvParticularsDetails.GetRowCellValue(i, colCharniCarat).ToString();
                                boilProcessMaster = new BoilProcessMaster();
                                boilProcessMaster.Id = Guid.NewGuid().ToString();
                                boilProcessMaster.BoilNo = Convert.ToInt32(grvParticularsDetails.GetRowCellValue(i, colBoilNo1));
                                boilProcessMaster.JangadNo = Convert.ToInt32(txtSerialNo.Text);
                                boilProcessMaster.CompanyId = Common.LoginCompany;
                                boilProcessMaster.BranchId = Common.LoginBranch;
                                boilProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                                boilProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                                boilProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                                boilProcessMaster.BoilType = Convert.ToInt32(ProcessType.Return);
                                boilProcessMaster.KapanId = lueKapan.EditValue.ToString();
                                boilProcessMaster.ShapeId = grvParticularsDetails.GetRowCellValue(i, colShapeId).ToString();
                                boilProcessMaster.SizeId = grvParticularsDetails.GetRowCellValue(i, colSizeId).ToString();
                                boilProcessMaster.PurityId = grvParticularsDetails.GetRowCellValue(i, colPurityId).ToString();
                                boilProcessMaster.Weight = Convert.ToDecimal(Cts);
                                boilProcessMaster.LossWeight = 0;
                                boilProcessMaster.RejectionWeight = 0;
                                boilProcessMaster.HandOverById = lueReceiveFrom.EditValue.ToString();
                                boilProcessMaster.HandOverToId = lueSendto.EditValue.ToString();
                                boilProcessMaster.SlipNo = grvParticularsDetails.GetRowCellValue(i, colSlipNo).ToString();
                                //boilProcessMaster.BoilCategoy = Convert.ToInt32(grvParticularsDetails.GetRowCellValue(i, colCategory));
                                boilProcessMaster.Remarks = txtRemark.Text;
                                boilProcessMaster.IsDelete = false;
                                boilProcessMaster.CreatedDate = DateTime.Now;
                                boilProcessMaster.CreatedBy = Common.LoginUserID;
                                boilProcessMaster.UpdatedDate = DateTime.Now;
                                boilProcessMaster.UpdatedBy = Common.LoginUserID;

                                var Result = await boilMasterRepository.AddBoilAsync(boilProcessMaster);
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
                            MessageBox.Show(AppMessages.GetString(AppMessageID.SaveSuccessfully), "[" + this.Text + "}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else if (Convert.ToInt32(lueDepartment.EditValue) == DepartmentMaster1.Charni)
                    {
                        string Cts = "0";

                        CharniProcessMasterRepository charniProcessMasterRepository = new CharniProcessMasterRepository();
                        CharniProcessMaster charniProcessMaster = new CharniProcessMaster();
                        bool IsSuccess = false;
                        try
                        {
                            for (int i = 0; i < grvParticularsDetails.RowCount; i++)
                            {
                                Cts = "0";
                                Cts = grvParticularsDetails.GetRowCellValue(i, colCharniCarat).ToString();
                                charniProcessMaster = new CharniProcessMaster();
                                charniProcessMaster.Id = Guid.NewGuid().ToString();
                                //charniProcessMaster.CharniNo = Convert.ToInt32(lueKapan.GetColumnValue("CharniNo").ToString());
                                charniProcessMaster.JangadNo = Convert.ToInt32(txtSerialNo.Text);
                                //charniProcessMaster.BoilJangadNo = Convert.ToInt32(lueKapan.GetColumnValue("BoilJangadNo").ToString());
                                charniProcessMaster.CompanyId = Common.LoginCompany;
                                charniProcessMaster.BranchId = Common.LoginBranch;
                                charniProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                                charniProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                                charniProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                                charniProcessMaster.CharniType = Convert.ToInt32(ProcessType.Return);
                                charniProcessMaster.KapanId = lueKapan.GetColumnValue("KapanId").ToString();
                                charniProcessMaster.ShapeId = grvParticularsDetails.GetRowCellValue(i, colShapeId).ToString();
                                charniProcessMaster.SizeId = grvParticularsDetails.GetRowCellValue(i, colSizeId).ToString();
                                charniProcessMaster.PurityId = grvParticularsDetails.GetRowCellValue(i, colPurityId).ToString();
                                charniProcessMaster.CharniSizeId = grvParticularsDetails.GetRowCellValue(i, colCharniSizeId).ToString();
                                charniProcessMaster.Weight = 0;//Convert.ToDecimal(txtACarat.Text);
                                charniProcessMaster.CharniWeight = Convert.ToDecimal(Cts);
                                charniProcessMaster.LossWeight = 0;
                                charniProcessMaster.RejectionWeight = 0;
                                charniProcessMaster.HandOverById = lueReceiveFrom.EditValue.ToString();
                                charniProcessMaster.HandOverToId = lueSendto.EditValue.ToString();
                                charniProcessMaster.SlipNo = grvParticularsDetails.GetRowCellValue(i, colSlipNo1).ToString();
                                //charniProcessMaster.CharniCategoy = Convert.ToInt32(grvParticularsDetails.GetRowCellValue(i, colCategory));
                                charniProcessMaster.Remarks = txtRemark.Text;
                                charniProcessMaster.IsDelete = false;
                                charniProcessMaster.CreatedDate = DateTime.Now;
                                charniProcessMaster.CreatedBy = Common.LoginUserID;
                                charniProcessMaster.UpdatedDate = DateTime.Now;
                                charniProcessMaster.UpdatedBy = Common.LoginUserID;

                                var Result = await charniProcessMasterRepository.AddCharniProcessAsync(charniProcessMaster);
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
                            MessageBox.Show(AppMessages.GetString(AppMessageID.SaveSuccessfully), "[" + this.Text + "}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
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

        private async void Reset()
        {
            grdParticularsDetails.DataSource = null;
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;
            txtRemark.Text = "";
            lueReceiveFrom.EditValue = null;
            lueSendto.EditValue = null;
            lueDepartment.EditValue = null;
            lueKapan.EditValue = null;
            repoSlipNo.DataSource = null;

            await GetMaxSrNo();
            await GetEmployeeList();
            lueReceiveFrom.Select();
            lueReceiveFrom.Focus();
        }
    }
}