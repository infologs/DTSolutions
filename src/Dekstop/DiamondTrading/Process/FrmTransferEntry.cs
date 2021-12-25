﻿using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using EFCore.SQL.Repository;
using Newtonsoft.Json;
using Repository.Entities;
using Repository.Entities.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiamondTrading.Process
{
    public partial class FrmTransferEntry : DevExpress.XtraEditors.XtraForm
    {
        TransferMasterRepository _transferMasterRepository;
        SalesMasterRepository _salesMasterRepository;
        SalesItemObj _salesItemObj;
        List<CaratCategoryType> _caratCategoryTypes;

        public FrmTransferEntry()
        {
            InitializeComponent();
            _transferMasterRepository = new TransferMasterRepository();
            _salesMasterRepository = new SalesMasterRepository();
            _salesItemObj = new SalesItemObj();
        }

        private void FrmTransferEntry_Load(object sender, EventArgs e)
        {
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;

            LoadTransferItemDetails();
        }

        private static DataTable GetDTColumnsforPurchaseDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Category");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Shape");
            dt.Columns.Add("Size");
            dt.Columns.Add("Purity");
            dt.Columns.Add("Kapan");
            dt.Columns.Add("Carat");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Amount");
            dt.Columns.Add("CaratCategory");
            dt.Columns.Add("Type");
            dt.Columns.Add("CharniSize");
            dt.Columns.Add("GalaSize");
            dt.Columns.Add("NumberSize");
            dt.Columns.Add("CategoryT");
            dt.Columns.Add("BranchT");
            dt.Columns.Add("ShapeT");
            dt.Columns.Add("SizeT");
            dt.Columns.Add("PurityT");
            dt.Columns.Add("KapanT");
            dt.Columns.Add("CaratT");
            dt.Columns.Add("CaratCategoryT");
            dt.Columns.Add("TypeT");
            dt.Columns.Add("RateT");
            dt.Columns.Add("AmountT");

            dt.Columns.Add("BoilNo");
            dt.Columns.Add("SlipNo");
            dt.Columns.Add("ShapeId");
            dt.Columns.Add("SizeId");
            dt.Columns.Add("PurityId");
            dt.Columns.Add("KapanId");
            dt.Columns.Add("TypeId");
            dt.Columns.Add("TypeIdT");
            return dt;
        }

        private async Task GetMaxSrNo()
        {
            var SrNo = await _transferMasterRepository.GetMaxSrNoAsync(Common.LoginCompany.ToString(), Common.LoginFinancialYear);
            txtSerialNo.Text = SrNo.ToString();
        }

        private void LoadTransferItemDetails()
        {
            GetMaxSrNo();

            grdTransferItemDetails.DataSource = GetDTColumnsforPurchaseDetails();

            //Company
            LoadCompany();

            //Branch
            GetBrancheDetail();
            
            //Employee
            GetEmployeeList();

            //Category
            GetCategoryDetail();
            
            //Size
            GetSizeDetail();

            //Shape
            GetShapeDetail();

            //Purity
            GetPurityDetail();

            //Kapan
            GetKapanDetail();

            GetCaratCategoryDetail();

            grvTransferItemDetails.BestFitColumns();
        }

        private async void LoadCompany()
        {
            CompanyMasterRepository companyMasterRepository = new CompanyMasterRepository();
            var result = await companyMasterRepository.GetAllCompanyAsync();
            lueCompany.Properties.DataSource = result;
            lueCompany.Properties.DisplayMember = "Name";
            lueCompany.Properties.ValueMember = "Id";
            lueCompany.EditValue = Common.LoginCompany;
        }

        private async void GetBrancheDetail()
        {
            if (lueCompany.EditValue != null)
            {
                BranchMasterRepository branchMasterRepository = new BranchMasterRepository();
                var branchMaster = await branchMasterRepository.GetAllBranchAsync(lueCompany.EditValue.ToString());
                repoBranch.DataSource = branchMaster;
                repoBranch.DisplayMember = "Name";
                repoBranch.ValueMember = "Id";

                repoBranchT.DataSource = branchMaster;
                repoBranchT.DisplayMember = "Name";
                repoBranchT.ValueMember = "Id";
            }
            else
            {
                repoBranch.DataSource = null;
                repoBranchT.DataSource = null;
            }
        }

        private async Task GetEmployeeList()
        {
            PartyMasterRepository partyMasterRepository = new PartyMasterRepository();
            var EmployeeDetailList = await partyMasterRepository.GetAllPartyAsync(Common.LoginCompany.ToString(), PartyTypeMaster.Employee, PartyTypeMaster.Other);
            lueTransferBy.Properties.DataSource = EmployeeDetailList;
            lueTransferBy.Properties.DisplayMember = "Name";
            lueTransferBy.Properties.ValueMember = "Id";
        }
        private async void GetCategoryDetail()
        {
            var Category = CategoryMaster.GetAllCategory();

            if (Category != null)
            {
                repoCategory.DataSource = Category;
                repoCategory.DisplayMember = "Name";
                repoCategory.ValueMember = "Id";

                repoCategoryT.DataSource = Category;
                repoCategoryT.DisplayMember = "Name";
                repoCategoryT.ValueMember = "Id";
            }
        }

        private async void GetCaratCategoryDetail()
        {
            var Category = CaratCategoryMaster.GetAllCaratCategory();

            if (Category != null)
            {
                repoCaratCategory.DataSource = Category;
                repoCaratCategory.DisplayMember = "Name";
                repoCaratCategory.ValueMember = "Id";

                //repoCategoryT.DataSource = Category;
                //repoCategoryT.DisplayMember = "Name";
                //repoCategoryT.ValueMember = "Id";
            }
        }

        private async void GetSizeDetail()
        {
            SizeMasterRepository sizeMasterRepository = new SizeMasterRepository();
            var sizeMaster = await sizeMasterRepository.GetAllSizeAsync();

            repoSizeT.DataSource = sizeMaster;
            repoSizeT.DisplayMember = "Name";
            repoSizeT.ValueMember = "Id";
        }

        private async void GetShapeDetail()
        {
            ShapeMasterRepository shapeMasterRepository = new ShapeMasterRepository();
            var shapeMaster = await shapeMasterRepository.GetAllShapeAsync();

            repoShapeT.DataSource = shapeMaster;
            repoShapeT.DisplayMember = "Name";
            repoShapeT.ValueMember = "Id";
        }

        private async void GetPurityDetail()
        {
            PurityMasterRepository purityMasterRepository = new PurityMasterRepository();
            var purityMaster = await purityMasterRepository.GetAllPurityAsync();

            repoPurityT.DataSource = purityMaster;
            repoPurityT.DisplayMember = "Name";
            repoPurityT.ValueMember = "Id";
        }

        private async void GetKapanDetail()
        {
            KapanMasterRepository kapanMasterRepository = new KapanMasterRepository();
            var kapanMaster = await kapanMasterRepository.GetAllKapanAsync();

            repoKapanT.DataSource = kapanMaster;
            repoKapanT.DisplayMember = "Name";
            repoKapanT.ValueMember = "Id";
        }

        private void lueCompany_EditValueChanged(object sender, EventArgs e)
        {
            GetBrancheDetail();
        }

        private async void grvTransferItemDetails_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Column == colCategory || e.Column == colBranch)
                {
                    if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Charni.ToString())
                    {
                        grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, CaratCategoryMaster.CharniCarat);

                        if (!string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString()))
                        {
                            GalaProcessMasterRepository galaProcessMasterRepository = new GalaProcessMasterRepository();
                            var ListGalaProcessSend = await galaProcessMasterRepository.GetGalaSendToDetails(lueCompany.EditValue.ToString(), grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString(), Common.LoginFinancialYear.ToString());

                            repoShape.DataSource = ListGalaProcessSend;
                            repoShape.DisplayMember = "Shape";
                            repoShape.ValueMember = "ShapeId";

                            repoShape.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                            repoShape.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

                            repoShape.Columns["BoilNo"].Visible = false;
                            repoShape.Columns["CharniSize"].Visible = true;
                            repoShape.Columns["GalaNumber"].Visible = false;
                            repoShape.Columns["Number"].Visible = false;
                        }
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Number.ToString())
                    {
                        grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, CaratCategoryMaster.NumberCarat);

                        if (!string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString()))
                        {
                            NumberProcessMasterRepository numberProcessMasterRepository = new NumberProcessMasterRepository();
                            var ListNumberProcessReturn = await numberProcessMasterRepository.GetNumberReturnDetails(lueCompany.EditValue.ToString(), grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString(), Common.LoginFinancialYear.ToString());

                            repoShape.DataSource = ListNumberProcessReturn;
                            repoShape.DisplayMember = "Shape";
                            repoShape.ValueMember = "ShapeId";

                            repoShape.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                            repoShape.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

                            repoShape.Columns["BoilNo"].Visible = false;
                            repoShape.Columns["CharniSize"].Visible = false;
                            repoShape.Columns["GalaNumber"].Visible = false;
                            repoShape.Columns["Number"].Visible = true;
                        }
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Gala.ToString())
                    {
                        grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, CaratCategoryMaster.GalaCarat);

                        if (!string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString()))
                        {
                            NumberProcessMasterRepository numberProcessMasterRepository = new NumberProcessMasterRepository();
                            var ListNumberProcessSend = await numberProcessMasterRepository.GetNumberSendToDetails(lueCompany.EditValue.ToString(), grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString(), Common.LoginFinancialYear.ToString());

                            repoShape.DataSource = ListNumberProcessSend;
                            repoShape.DisplayMember = "Shape";
                            repoShape.ValueMember = "ShapeId";

                            repoShape.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                            repoShape.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

                            repoShape.Columns["BoilNo"].Visible = false;
                            repoShape.Columns["CharniSize"].Visible = false;
                            repoShape.Columns["GalaNumber"].Visible = true;
                            repoShape.Columns["Number"].Visible = false;
                        }
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Boil.ToString())
                    {
                        grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, CaratCategoryMaster.None);

                        if (!string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString()))
                        {
                            CharniProcessMasterRepository charniProcessMasterRepository = new CharniProcessMasterRepository();
                            var ListCharniProcessSend = await charniProcessMasterRepository.GetCharniSendToDetails(lueCompany.EditValue.ToString(), grvTransferItemDetails.GetRowCellValue(e.RowHandle, colBranch).ToString(), Common.LoginFinancialYear.ToString());

                            repoShape.DataSource = ListCharniProcessSend;
                            repoShape.DisplayMember = "Shape";
                            repoShape.ValueMember = "ShapeId";

                            repoShape.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
                            repoShape.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;

                            repoShape.Columns["BoilNo"].Visible = true;
                            repoShape.Columns["CharniSize"].Visible = false;
                            repoShape.Columns["GalaNumber"].Visible = false;
                            repoShape.Columns["Number"].Visible = false;
                        }
                    }
                }
                else if (e.Column == colTypeT)
                {
                    try
                    {
                        string Id = e.Value.ToString();
                        grvTransferItemDetails.SetRowCellValue(e.RowHandle, colTypeIdT, Id);
                        var result = _caratCategoryTypes.Where(x => x.Id.Equals(Id));
                        grvTransferItemDetails.SetRowCellValue(e.RowHandle, colTypeT, result.FirstOrDefault().Name);
                    }
                    catch
                    {

                    }
                    //if (!string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT).ToString()))
                    //{
                    //    if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.None)
                    //    {

                    //    }
                    //    else if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.CharniCarat)
                    //    {
                    //        if (_caratCategoryTypes == null)
                    //            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                    //        repoTypeT.DataSource = _caratCategoryTypes;
                    //        repoTypeT.DisplayMember = "Name";
                    //        repoTypeT.ValueMember = "Id";
                    //    }
                    //    else if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.GalaCarat)
                    //    {
                    //        if (_caratCategoryTypes == null)
                    //            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                    //        repoTypeT.DataSource = _caratCategoryTypes;
                    //        repoTypeT.DisplayMember = "Name";
                    //        repoTypeT.ValueMember = "Id";
                    //    }
                    //    else if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.NumberCarat)
                    //    {
                    //        if (_caratCategoryTypes == null)
                    //            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                    //        repoTypeT.DataSource = _caratCategoryTypes;
                    //        repoTypeT.DisplayMember = "Name";
                    //        repoTypeT.ValueMember = "Id";
                    //    }                        
                    //}
                }
                else if (e.Column == colShape)
                {
                    try
                    {
                        if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Boil.ToString())
                        {
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurity, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Purity);
                            //grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colType, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapan, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Kapan);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCarat, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);

                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colBoilNo, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).BoilNo);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSlipNo, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapanId, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).KapanId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colTypeId, ((Repository.Entities.Models.CharniProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            //grvPurchaseItems.FocusedRowHandle = e.RowHandle;
                            //grvPurchaseItems.FocusedColumn = colBoilCarat;
                        }
                        else if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Charni.ToString())
                        {
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurity, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Purity);
                            //grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colType, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).CharniSize);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapan, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Kapan);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCarat, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);

                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSlipNo, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapanId, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).KapanId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colTypeId, ((Repository.Entities.Models.GalaProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).CharniSizeId);
                            //grvPurchaseItems.FocusedRowHandle = e.RowHandle;
                            //grvPurchaseItems.FocusedColumn = colBoilCarat;
                        }
                        else if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Gala.ToString())
                        {
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurity, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Purity);
                            //grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colType, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).GalaNumber);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapan, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).Kapan);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCarat, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);

                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSlipNo, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapanId, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).KapanId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colTypeId, ((Repository.Entities.Models.NumberProcessSend)repoShape.GetDataSourceRowByKeyValue(e.Value)).GalaNumberId);
                            //grvPurchaseItems.FocusedRowHandle = e.RowHandle;
                            //grvPurchaseItems.FocusedColumn = colBoilCarat;
                        }
                        else if (grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCategory).ToString() == CategoryMaster.Number.ToString())
                        {
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSize, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).Size);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurity, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).Purity);
                            //grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCaratCategory, ((Repository.Entities.Models.CharniProcessSend)repoSlipNo.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colType, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).Number);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapan, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).Kapan);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colCarat, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).AvailableWeight);

                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSlipNo, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).SlipNo);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colShapeId, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).ShapeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colSizeId, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).SizeId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colPurityId, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).PurityId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colKapanId, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).KapanId);
                            grvTransferItemDetails.SetRowCellValue(e.RowHandle, colTypeId, ((Repository.Entities.Models.NumberProcessReturn)repoShape.GetDataSourceRowByKeyValue(e.Value)).NumberId);
                            //grvPurchaseItems.FocusedRowHandle = e.RowHandle;
                            //grvPurchaseItems.FocusedColumn = colBoilCarat;
                        }
                    }
                    catch
                    {

                    }
                }
                else if (e.Column == colCarat)
                {
                    //decimal TipWeight = Convert.ToDecimal(lueBranch.GetColumnValue("TipWeight"));
                    //decimal CVDWeight = Convert.ToDecimal(lueBranch.GetColumnValue("CVDWeight"));
                    //GetLessWeightDetailBasedOnCity(lueBranch.GetColumnValue("LessWeightId").ToString(), Convert.ToDecimal(grvPurchaseDetails.GetRowCellValue(e.RowHandle, colCarat)), e.RowHandle, TipWeight, CVDWeight);
                }
                else if (e.Column == colRate)
                {
                    decimal Carat = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCarat).ToString());
                    decimal Rate = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colRate).ToString());

                    grvTransferItemDetails.SetRowCellValue(e.RowHandle, colAmount, (Carat*Rate).ToString());
                }
                else if (e.Column == colRateT)
                {
                    decimal Carat = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratT).ToString());
                    decimal Rate = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colRateT).ToString());

                    grvTransferItemDetails.SetRowCellValue(e.RowHandle, colAmountT, (Carat * Rate).ToString());
                }
                else if (e.Column == colCaratT)
                {
                    //decimal Carat = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCarat).ToString());
                    //decimal CaratT = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratT).ToString());
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private async void grvTransferItemDetails_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            
        }

        private async void grvTransferItemDetails_ShowingEditor(object sender, CancelEventArgs e)
        {
            ColumnView view = (ColumnView)sender;
            //if (view.FocusedColumn == colTypeT && !string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(view.FocusedRowHandle, colCaratCategoryT).ToString()))
            {
                //grvTransferItemDetails_CustomRowCellEdit(null,null);
            }
            
        }

        private async void grvTransferItemDetails_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            try
            {
                if (e.Column == colTypeT)
                {
                    if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.None)
                    {
                        if (_caratCategoryTypes == null)
                            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                        repoTypeT.DataSource = _caratCategoryTypes.Where(x => x.Type.Equals(CaratCategoryMaster.CharniCarat));
                        repoTypeT.DisplayMember = "Name";
                        repoTypeT.ValueMember = "Id";

                        e.RepositoryItem = repoTypeT;
                    }
                    else if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.CharniCarat)
                    {
                        if (_caratCategoryTypes == null)
                            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                        repoTypeT.DataSource = _caratCategoryTypes.Where(x => x.Type.Equals(CaratCategoryMaster.CharniCarat));
                        repoTypeT.DisplayMember = "Name";
                        repoTypeT.ValueMember = "Id";

                        e.RepositoryItem = repoTypeT;
                    }
                    else if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.GalaCarat)
                    {
                        if (_caratCategoryTypes == null)
                            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                        repoTypeT.DataSource = _caratCategoryTypes.Where(x => x.Type.Equals(CaratCategoryMaster.GalaCarat));
                        repoTypeT.DisplayMember = "Name";
                        repoTypeT.ValueMember = "Id";

                        e.RepositoryItem = repoTypeT;
                    }
                    else if (Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(e.RowHandle, colCaratCategoryT)) == CaratCategoryMaster.NumberCarat)
                    {
                        if (_caratCategoryTypes == null)
                            _caratCategoryTypes = await _salesMasterRepository.GetCaratCategoryDetails();

                        repoTypeT.DataSource = _caratCategoryTypes.Where(x => x.Type.Equals(CaratCategoryMaster.NumberCarat));
                        repoTypeT.DisplayMember = "Name";
                        repoTypeT.ValueMember = "Id";

                        e.RepositoryItem = repoTypeT;
                    }
                }
            }
            catch
            {

            }
        }

        private void Image1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Image1.Image = LoadImage();
            Image1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
        }

        private void Image2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Image2.Image = LoadImage();
            Image2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
        }

        private void Image3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Image3.Image = LoadImage();
            Image3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
        }

        private Image LoadImage()
        {
            Image newimage = null;
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

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (!CheckValidation())
                    return;

                string TransferId = Guid.NewGuid().ToString();

                BoilMasterRepository boilMasterRepository;
                CharniProcessMasterRepository charniProcessMasterRepository;
                GalaProcessMasterRepository galaProcessMasterRepository;
                NumberProcessMasterRepository numberProcessMasterRepository;
                BoilProcessMaster boilProcessMaster;
                CharniProcessMaster charniProcessMaster;
                GalaProcessMaster galaProcessMaster;
                NumberProcessMaster numberProcessMaster;

                for (int i = 0; i < grvTransferItemDetails.RowCount; i++)
                {
                    string TransferEntryId = Guid.NewGuid().ToString();
                    string TransferType = grvTransferItemDetails.GetRowCellValue(i, colCategory).ToString() + "-" + grvTransferItemDetails.GetRowCellValue(i, colCategoryT).ToString();
                    //Transfer From
                    if (grvTransferItemDetails.GetRowCellValue(i, colCategory).ToString() == CategoryMaster.Boil.ToString())
                    {
                        boilProcessMaster = new BoilProcessMaster();
                        boilProcessMaster.Id = Guid.NewGuid().ToString();
                        boilProcessMaster.BoilNo = Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(i, colBoilNo).ToString());
                        boilProcessMaster.JangadNo = 0;
                        boilProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        boilProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranch).ToString();
                        boilProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        boilProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        boilProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        boilProcessMaster.BoilType = Convert.ToInt32(ProcessType.Receive);
                        boilProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanId).ToString();
                        boilProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeId).ToString();
                        boilProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeId).ToString();
                        boilProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityId).ToString();
                        boilProcessMaster.Weight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString()) * -1;
                        boilProcessMaster.LossWeight = 0;
                        boilProcessMaster.RejectionWeight = 0;
                        boilProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        boilProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        boilProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString(); ;
                        boilProcessMaster.BoilCategoy = 0;
                        boilProcessMaster.Remarks = txtRemark.Text;

                        boilProcessMaster.TransferId = TransferId;
                        boilProcessMaster.TransferType = TransferType;
                        boilProcessMaster.TransferEntryId = TransferEntryId;
                        boilProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRate).ToString());

                        boilProcessMaster.IsDelete = false;
                        boilProcessMaster.CreatedDate = DateTime.Now;
                        boilProcessMaster.CreatedBy = Common.LoginUserID;
                        boilProcessMaster.UpdatedDate = DateTime.Now;
                        boilProcessMaster.UpdatedBy = Common.LoginUserID;

                        boilMasterRepository = new BoilMasterRepository();
                        var Result1 = await boilMasterRepository.AddBoilAsync(boilProcessMaster);
                        boilMasterRepository = null;
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(i, colCategory).ToString() == CategoryMaster.Charni.ToString())
                    {
                        charniProcessMaster = new CharniProcessMaster();
                        charniProcessMaster.Id = Guid.NewGuid().ToString();
                        charniProcessMaster.CharniNo = 0;
                        charniProcessMaster.JangadNo = 0;
                        charniProcessMaster.BoilJangadNo = 0;
                        charniProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        charniProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranch).ToString();
                        charniProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        charniProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        charniProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        charniProcessMaster.CharniType = Convert.ToInt32(ProcessType.Receive);
                        charniProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanId).ToString();
                        charniProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeId).ToString();
                        charniProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeId).ToString();
                        charniProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityId).ToString();
                        charniProcessMaster.Weight = 0;
                        charniProcessMaster.CharniSizeId = grvTransferItemDetails.GetRowCellValue(i, colTypeId).ToString();
                        charniProcessMaster.CharniWeight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString()) * -1;
                        charniProcessMaster.LossWeight = 0;
                        charniProcessMaster.RejectionWeight = 0;
                        charniProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        charniProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        charniProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString();
                        charniProcessMaster.CharniCategoy = 0;
                        charniProcessMaster.Remarks = txtRemark.Text;

                        charniProcessMaster.TransferId = TransferId;
                        charniProcessMaster.TransferType = TransferType;
                        charniProcessMaster.TransferEntryId = TransferEntryId;
                        charniProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRate).ToString());

                        charniProcessMaster.IsDelete = false;
                        charniProcessMaster.CreatedDate = DateTime.Now;
                        charniProcessMaster.CreatedBy = Common.LoginUserID;
                        charniProcessMaster.UpdatedDate = DateTime.Now;
                        charniProcessMaster.UpdatedBy = Common.LoginUserID;

                        charniProcessMasterRepository = new CharniProcessMasterRepository();
                        var Result1 = await charniProcessMasterRepository.AddCharniProcessAsync(charniProcessMaster);
                        charniProcessMasterRepository = null;
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(i, colCategory).ToString() == CategoryMaster.Gala.ToString())
                    {
                        galaProcessMaster = new GalaProcessMaster();
                        galaProcessMaster.Id = Guid.NewGuid().ToString();
                        galaProcessMaster.GalaNo = 0;
                        galaProcessMaster.JangadNo = 0;
                        //galaProcessMaster.BoilJangadNo = Convert.ToInt32(lueKapan.GetColumnValue("BoilJangadNo").ToString());
                        galaProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        galaProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranch).ToString();
                        galaProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        galaProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        galaProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        galaProcessMaster.GalaProcessType = Convert.ToInt32(ProcessType.Receive);
                        galaProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanId).ToString();
                        galaProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeId).ToString();
                        galaProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeId).ToString();
                        galaProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityId).ToString();
                        galaProcessMaster.Weight = 0;
                        galaProcessMaster.GalaNumberId = grvTransferItemDetails.GetRowCellValue(i, colTypeId).ToString();
                        galaProcessMaster.GalaWeight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString()) * -1;
                        galaProcessMaster.LossWeight = 0;
                        galaProcessMaster.RejectionWeight = 0;
                        galaProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        galaProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        galaProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString();
                        galaProcessMaster.GalaCategoy = 0;
                        galaProcessMaster.Remarks = txtRemark.Text;

                        galaProcessMaster.TransferId = TransferId;
                        galaProcessMaster.TransferType = TransferType;
                        galaProcessMaster.TransferEntryId = TransferEntryId;
                        galaProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRate).ToString());

                        galaProcessMaster.IsDelete = false;
                        galaProcessMaster.CreatedDate = DateTime.Now;
                        galaProcessMaster.CreatedBy = Common.LoginUserID;
                        galaProcessMaster.UpdatedDate = DateTime.Now;
                        galaProcessMaster.UpdatedBy = Common.LoginUserID;

                        galaProcessMasterRepository = new GalaProcessMasterRepository();
                        var Result1 = await galaProcessMasterRepository.AddGalaProcessAsync(galaProcessMaster);
                        galaProcessMasterRepository = null;
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(i, colCategory).ToString() == CategoryMaster.Number.ToString())
                    {
                        numberProcessMaster = new NumberProcessMaster();
                        numberProcessMaster.Id = Guid.NewGuid().ToString();
                        numberProcessMaster.NumberNo = 0;
                        numberProcessMaster.JangadNo = 0;
                        //galaProcessMaster.BoilJangadNo = Convert.ToInt32(lueKapan.GetColumnValue("BoilJangadNo").ToString());
                        numberProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        numberProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranch).ToString();
                        numberProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        numberProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        numberProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        numberProcessMaster.NumberProcessType = Convert.ToInt32(ProcessType.Receive);
                        numberProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanId).ToString();
                        numberProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeId).ToString();
                        numberProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeId).ToString();
                        numberProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityId).ToString();
                        numberProcessMaster.Weight = 0;
                        //numberProcessMaster.GalaNumberId = grvTransferItemDetails.GetRowCellValue(i, colTypeId).ToString();
                        numberProcessMaster.NumberId = grvTransferItemDetails.GetRowCellValue(i, colTypeId).ToString();
                        numberProcessMaster.NumberWeight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString()) * -1;
                        numberProcessMaster.LossWeight = 0;
                        numberProcessMaster.RejectionWeight = 0;
                        numberProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        numberProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        numberProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString();
                        numberProcessMaster.NumberCategoy = 0;
                        numberProcessMaster.Remarks = txtRemark.Text;

                        numberProcessMaster.TransferId = TransferId;
                        numberProcessMaster.TransferType = TransferType;
                        numberProcessMaster.TransferEntryId = TransferEntryId;
                        numberProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRate).ToString());

                        numberProcessMaster.IsDelete = false;
                        numberProcessMaster.CreatedDate = DateTime.Now;
                        numberProcessMaster.CreatedBy = Common.LoginUserID;
                        numberProcessMaster.UpdatedDate = DateTime.Now;
                        numberProcessMaster.UpdatedBy = Common.LoginUserID;

                        numberProcessMasterRepository = new NumberProcessMasterRepository();
                        var Result1 = await numberProcessMasterRepository.AddNumberProcessAsync(numberProcessMaster);
                        numberProcessMasterRepository = null;
                    }

                    //Transfer To
                    if (grvTransferItemDetails.GetRowCellValue(i, colCategoryT).ToString() == CategoryMaster.Boil.ToString())
                    {
                        boilProcessMaster = new BoilProcessMaster();
                        boilProcessMaster.Id = Guid.NewGuid().ToString();
                        //boilProcessMaster.PurchaseDetailsId = grvParticularsDetails.GetRowCellValue(i, colPurchaseDetailsId).ToString();
                        if(!string.IsNullOrEmpty(grvTransferItemDetails.GetRowCellValue(i, colBoilNo).ToString()))
                            boilProcessMaster.BoilNo = Convert.ToInt32(grvTransferItemDetails.GetRowCellValue(i, colBoilNo).ToString());
                        else
                            boilProcessMaster.BoilNo = 0;
                        boilProcessMaster.JangadNo = 0;
                        boilProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        boilProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranchT).ToString();
                        boilProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        boilProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        boilProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        boilProcessMaster.BoilType = Convert.ToInt32(ProcessType.Receive);
                        boilProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanT).ToString();
                        boilProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeT).ToString();
                        boilProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeT).ToString();
                        boilProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityT).ToString();
                        boilProcessMaster.Weight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString());
                        boilProcessMaster.LossWeight = 0;
                        boilProcessMaster.RejectionWeight = 0;
                        boilProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        boilProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        boilProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString(); ;
                        boilProcessMaster.BoilCategoy = 0;
                        boilProcessMaster.Remarks = txtRemark.Text;

                        boilProcessMaster.TransferId = TransferId;
                        boilProcessMaster.TransferType = TransferType;
                        boilProcessMaster.TransferEntryId = TransferEntryId;
                        boilProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRateT).ToString());

                        boilProcessMaster.IsDelete = false;
                        boilProcessMaster.CreatedDate = DateTime.Now;
                        boilProcessMaster.CreatedBy = Common.LoginUserID;
                        boilProcessMaster.UpdatedDate = DateTime.Now;
                        boilProcessMaster.UpdatedBy = Common.LoginUserID;

                        boilMasterRepository = new BoilMasterRepository();
                        var Result1 = await boilMasterRepository.AddBoilAsync(boilProcessMaster);
                        boilMasterRepository = null;
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(i, colCategoryT).ToString() == CategoryMaster.Charni.ToString())
                    {
                        charniProcessMaster = new CharniProcessMaster();
                        charniProcessMaster.Id = Guid.NewGuid().ToString();
                        charniProcessMaster.CharniNo = 0;
                        charniProcessMaster.JangadNo = 0;
                        charniProcessMaster.BoilJangadNo = 0;
                        charniProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        charniProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranchT).ToString();
                        charniProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        charniProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        charniProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        charniProcessMaster.CharniType = Convert.ToInt32(ProcessType.Receive);
                        charniProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanT).ToString();
                        charniProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeT).ToString();
                        charniProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeT).ToString();
                        charniProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityT).ToString();
                        charniProcessMaster.Weight = 0;
                        charniProcessMaster.CharniSizeId = grvTransferItemDetails.GetRowCellValue(i, colTypeIdT).ToString();
                        charniProcessMaster.CharniWeight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString());
                        charniProcessMaster.LossWeight = 0;
                        charniProcessMaster.RejectionWeight = 0;
                        charniProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        charniProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        charniProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString();
                        charniProcessMaster.CharniCategoy = 0;
                        charniProcessMaster.Remarks = txtRemark.Text;

                        charniProcessMaster.TransferId = TransferId;
                        charniProcessMaster.TransferType = TransferType;
                        charniProcessMaster.TransferEntryId = TransferEntryId;
                        charniProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRateT).ToString());

                        charniProcessMaster.IsDelete = false;
                        charniProcessMaster.CreatedDate = DateTime.Now;
                        charniProcessMaster.CreatedBy = Common.LoginUserID;
                        charniProcessMaster.UpdatedDate = DateTime.Now;
                        charniProcessMaster.UpdatedBy = Common.LoginUserID;

                        charniProcessMasterRepository = new CharniProcessMasterRepository();
                        var Result1 = await charniProcessMasterRepository.AddCharniProcessAsync(charniProcessMaster);
                        charniProcessMasterRepository = null;
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(i, colCategoryT).ToString() == CategoryMaster.Gala.ToString())
                    {
                        galaProcessMaster = new GalaProcessMaster();
                        galaProcessMaster.Id = Guid.NewGuid().ToString();
                        galaProcessMaster.GalaNo = 0;
                        galaProcessMaster.JangadNo = 0;
                        //galaProcessMaster.BoilJangadNo = Convert.ToInt32(lueKapan.GetColumnValue("BoilJangadNo").ToString());
                        galaProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        galaProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranchT).ToString();
                        galaProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        galaProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        galaProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        galaProcessMaster.GalaProcessType = Convert.ToInt32(ProcessType.Receive);
                        galaProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanT).ToString();
                        galaProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeT).ToString();
                        galaProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeT).ToString();
                        galaProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityT).ToString();
                        galaProcessMaster.Weight = 0;
                        galaProcessMaster.GalaNumberId = grvTransferItemDetails.GetRowCellValue(i, colTypeIdT).ToString();
                        galaProcessMaster.GalaWeight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString());
                        galaProcessMaster.LossWeight = 0;
                        galaProcessMaster.RejectionWeight = 0;
                        galaProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        galaProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        galaProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString();
                        galaProcessMaster.GalaCategoy = 0;
                        galaProcessMaster.Remarks = txtRemark.Text;

                        galaProcessMaster.TransferId = TransferId;
                        galaProcessMaster.TransferType = TransferType;
                        galaProcessMaster.TransferEntryId = TransferEntryId;
                        galaProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRateT).ToString());

                        galaProcessMaster.IsDelete = false;
                        galaProcessMaster.CreatedDate = DateTime.Now;
                        galaProcessMaster.CreatedBy = Common.LoginUserID;
                        galaProcessMaster.UpdatedDate = DateTime.Now;
                        galaProcessMaster.UpdatedBy = Common.LoginUserID;

                        galaProcessMasterRepository = new GalaProcessMasterRepository();
                        var Result1 = await galaProcessMasterRepository.AddGalaProcessAsync(galaProcessMaster);
                        galaProcessMasterRepository = null;
                    }
                    else if (grvTransferItemDetails.GetRowCellValue(i, colCategoryT).ToString() == CategoryMaster.Number.ToString())
                    {
                        numberProcessMaster = new NumberProcessMaster();
                        numberProcessMaster.Id = Guid.NewGuid().ToString();
                        numberProcessMaster.NumberNo = 0;
                        numberProcessMaster.JangadNo = 0;
                        //galaProcessMaster.BoilJangadNo = Convert.ToInt32(lueKapan.GetColumnValue("BoilJangadNo").ToString());
                        numberProcessMaster.CompanyId = lueCompany.EditValue.ToString();
                        numberProcessMaster.BranchId = grvTransferItemDetails.GetRowCellValue(i, colBranchT).ToString();
                        numberProcessMaster.EntryDate = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                        numberProcessMaster.EntryTime = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                        numberProcessMaster.FinancialYearId = Common.LoginFinancialYear;
                        numberProcessMaster.NumberProcessType = Convert.ToInt32(ProcessType.Receive);
                        numberProcessMaster.KapanId = grvTransferItemDetails.GetRowCellValue(i, colKapanT).ToString();
                        numberProcessMaster.ShapeId = grvTransferItemDetails.GetRowCellValue(i, colShapeT).ToString();
                        numberProcessMaster.SizeId = grvTransferItemDetails.GetRowCellValue(i, colSizeT).ToString();
                        numberProcessMaster.PurityId = grvTransferItemDetails.GetRowCellValue(i, colPurityT).ToString();
                        numberProcessMaster.Weight = 0;
                        //numberProcessMaster.GalaNumberId = grvTransferItemDetails.GetRowCellValue(i, colTypeId).ToString();
                        numberProcessMaster.NumberId = grvTransferItemDetails.GetRowCellValue(i, colTypeIdT).ToString();
                        numberProcessMaster.NumberWeight = Convert.ToDecimal(grvTransferItemDetails.GetRowCellValue(i, colCaratT).ToString());
                        numberProcessMaster.LossWeight = 0;
                        numberProcessMaster.RejectionWeight = 0;
                        numberProcessMaster.HandOverById = lueTransferBy.EditValue.ToString();
                        numberProcessMaster.HandOverToId = lueTransferBy.EditValue.ToString();
                        numberProcessMaster.SlipNo = grvTransferItemDetails.GetRowCellValue(i, colSlipNo).ToString();
                        numberProcessMaster.NumberCategoy = 0;
                        numberProcessMaster.Remarks = txtRemark.Text;

                        numberProcessMaster.TransferId = TransferId;
                        numberProcessMaster.TransferType = TransferType;
                        numberProcessMaster.TransferEntryId = TransferEntryId;
                        numberProcessMaster.TransferCaratRate = Convert.ToDouble(grvTransferItemDetails.GetRowCellValue(i, colRateT).ToString());

                        numberProcessMaster.IsDelete = false;
                        numberProcessMaster.CreatedDate = DateTime.Now;
                        numberProcessMaster.CreatedBy = Common.LoginUserID;
                        numberProcessMaster.UpdatedDate = DateTime.Now;
                        numberProcessMaster.UpdatedBy = Common.LoginUserID;

                        numberProcessMasterRepository = new NumberProcessMasterRepository();
                        var Result1 = await numberProcessMasterRepository.AddNumberProcessAsync(numberProcessMaster);
                        numberProcessMasterRepository = null;
                    }
                }

                TransferMaster transferMaster = new TransferMaster();
                transferMaster.Id = TransferId;
                transferMaster.JangadNo = Convert.ToInt32(txtSerialNo.Text);
                transferMaster.Date = Convert.ToDateTime(dtDate.Text).ToString("yyyyMMdd");
                transferMaster.Time = Convert.ToDateTime(dtTime.Text).ToString("hh:mm:ss ttt");
                transferMaster.TRansferById = lueTransferBy.EditValue.ToString();
                transferMaster.Remark = txtRemark.Text;
                if (Image1.Image != null)
                    transferMaster.Image1 = ImageToByteArray(Image1.Image);
                if (Image2.Image != null)
                    transferMaster.Image2 = ImageToByteArray(Image2.Image);
                if (Image3.Image != null)
                    transferMaster.Image3 = ImageToByteArray(Image3.Image);

                transferMaster.CompanyId = lueCompany.EditValue.ToString();
                transferMaster.FinancialYearId = Common.LoginFinancialYear;
                transferMaster.IsDelete = false;
                transferMaster.CreatedDate = DateTime.Now;
                transferMaster.CreatedBy = Common.LoginUserID;
                transferMaster.UpdatedDate = DateTime.Now;
                transferMaster.UpdatedBy = Common.LoginUserID;

                var Result = await _transferMasterRepository.AddTransferAsync(transferMaster);

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

        private bool CheckValidation()
        {
            if (lueTransferBy.EditValue == null)
            {
                MessageBox.Show("Please select Transfer By name", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lueTransferBy.Focus();
                return false;
            }
            else if (grvTransferItemDetails.RowCount == 0)
            {
                MessageBox.Show("Please select Particulars Details", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                grvTransferItemDetails.Focus();
                return false;
            }
            return true;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private void Reset()
        {
            grdTransferItemDetails.DataSource = null;
            dtDate.EditValue = DateTime.Now;
            dtTime.EditValue = DateTime.Now;

            LoadTransferItemDetails();

            txtRemark.Text = "";
            Image1.Image = null;
            Image2.Image = null;
            Image3.Image = null;

            lueTransferBy.Focus();
            lueTransferBy.Select();
        }
    }
}