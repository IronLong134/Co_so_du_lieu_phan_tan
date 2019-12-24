using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class ListPhieuXuat : Form
    {
        public ListPhieuXuat()
        {
            InitializeComponent();
        }

        private void PhieuXuatBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.phieuXuatBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetQLVT);

        }

        private void ListPhieuXuat_Load(object sender, EventArgs e)
        {


            if (Program.mGroup.ToString() == "CHINHANH" || (Program.mGroup.ToString() == "USER"))
            {
                // btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhuchoi.Enabled = btnRefresh.Enabled = true;
                // MessageBox.Show("group là " + Program.mGroup, "", MessageBoxButtons.OK);

            }
            else if (Program.mGroup == "CONGTY")
            {

            }
            // TODO: This line of code loads data into the 'dataSetQLVT.CTPN' table. You can move, or remove it, as needed.


            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedValue = Program.mChinhanh;
            dataSetQLVT.EnforceConstraints = false;
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;


            // TODO: This line of code loads data into the 'dataSetQLVT.CTPX' table. You can move, or remove it, as needed.
            this.cTPXTableAdapter.Fill(this.dataSetQLVT.CTPX);
            // TODO: This line of code loads data into the 'dataSetQLVT.PhieuXuat' table. You can move, or remove it, as needed.
            this.phieuXuatTableAdapter.Fill(this.dataSetQLVT.PhieuXuat);

        }

        private void CmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbChiNhanh.SelectedValue == null) return;
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;
            Program.servername = cmbChiNhanh.SelectedValue.ToString();

            if (cmbChiNhanh.SelectedValue.ToString() != Program.mChinhanh)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.KetNoi() == 0)
                MessageBox.Show("Lỗi kết nối về cơ sở mới", "", MessageBoxButtons.OK);
            else
            {
                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.dataSetQLVT.PhieuXuat);
            }
        }

        private void BtnThemPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
