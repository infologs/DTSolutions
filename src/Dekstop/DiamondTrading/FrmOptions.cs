﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiamondTrading
{
    public partial class FrmOptions : DevExpress.XtraEditors.XtraForm
    {
        public FrmOptions()
        {
            InitializeComponent();
        }

        private void EnableDisableApplyButton(bool Value)
        {
            btnApply.Enabled = Value;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
            EnableDisableApplyButton(false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadRegistry()
        {
            txtFormTitle.Text = Common.FormTitle;
            chkPrintSlip.Checked = Common.PrintPurchaseSlip;
            chkPrintPF.Checked = Common.PrintPurchasePF;
            chkAllowToSelectPaymentDueDate.Checked = Common.AllowToSelectPurchaseDueDate;
        }

        private void SaveSettings()
        {
            if (btnApply.Enabled)
            {
                #region "General"
                Common.FormTitle = txtFormTitle.Text;
                RegistryHelper.SaveSettings(RegistryHelper.OtherSection, RegistryHelper.FormTitle, txtFormTitle.Text);
                #endregion

                #region "Advanced"
                Common.PrintPurchaseSlip = chkPrintSlip.Checked;
                RegistryHelper.SaveSettings(RegistryHelper.OtherSection, RegistryHelper.PrintPurchaseSlip, chkPrintSlip.Checked.ToString());

                Common.PrintPurchasePF = chkPrintPF.Checked;
                RegistryHelper.SaveSettings(RegistryHelper.OtherSection, RegistryHelper.PrintPurchasePF, chkPrintPF.Checked.ToString());

                Common.AllowToSelectPurchaseDueDate = chkAllowToSelectPaymentDueDate.Checked;
                RegistryHelper.SaveSettings(RegistryHelper.OtherSection, RegistryHelper.AllowToSelectPurchaseDueDate, chkAllowToSelectPaymentDueDate.Checked.ToString());
                #endregion
            }
        }

        private void FrmOptions_Load(object sender, EventArgs e)
        {
            LoadRegistry();
        }

        private void txtFormTitle_TextChanged(object sender, EventArgs e)
        {
            EnableDisableApplyButton(true);
        }

        private void chkPrintSlip_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableApplyButton(true);
        }

        private void chkAllowToSelectPaymentDueDate_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableApplyButton(true);
        }

        private void chkPrintPF_CheckedChanged(object sender, EventArgs e)
        {
            EnableDisableApplyButton(true);
        }

        private void checkEditClearReportLayout_CheckedChanged(object sender, EventArgs e)
        {
            if(checkEditClearReportLayout.Checked)
            {
                RegistryHelper.DeleteSettings();
            }
        }
    }
}