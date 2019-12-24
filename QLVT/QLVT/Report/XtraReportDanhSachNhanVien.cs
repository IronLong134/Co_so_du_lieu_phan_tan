using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Windows.Forms;

namespace QLVT.Report
{
    public partial class XtraReportDanhSachNhanVien : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportDanhSachNhanVien(string maCN)
        {
            InitializeComponent();

            dataSetQLVT1.EnforceConstraints = false;

            try
            {
                this.sP_DS_NHANVIENTableAdapter.Fill(dataSetQLVT1.SP_DS_NHANVIEN, maCN);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi " + ex.Message, "", MessageBoxButtons.OK);
            }
        }

    }
}
