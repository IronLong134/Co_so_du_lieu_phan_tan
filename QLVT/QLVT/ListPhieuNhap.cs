using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT
{
    public partial class ListPhieuNhap : Form
    {
        private bool btn_ThemPN = false;
        private bool btn_ThemCTPN = false;
        private bool btn_SuaCTPN = false;
        private bool btn_SuaPN = false;
        private int viTri_PN = 0;
        private int viTri_CTPN = 0;

        public ListPhieuNhap()
        {
            InitializeComponent();
        }

        private void PhieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.phieuNhapBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetQLVT);

        }

        private void ResetDefault()
        {
            btnThemCTPN.Enabled = btnThemPhieuNhap.Enabled = btnSuaPN.Enabled  = btnXoaPN.Enabled  = btnRefresh.Enabled = btnThoat.Enabled = true;
            btnLuu.Enabled = btnPhucHoi.Enabled = false;
            groupBoxPhieuNhap.Enabled = groupBoxCTPN.Enabled = false;
            phieuNhapGridControl.Enabled = cTPNGridControl.Enabled = true;
        }
        private void DisableButton()
        {
            btnThemPhieuNhap.Enabled = btnThemCTPN.Enabled = btnSuaPN.Enabled  = btnXoaPN.Enabled  = false;
            btnLuu.Enabled = btnPhucHoi.Enabled = btnRefresh.Enabled = btnThoat.Enabled = true;
        }
        public void capNhatSoLuongVatTu(string maVT,int soLuong)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_CAPNHATSOLUONG_VT", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("maVT", maVT);
                cmd.Parameters.AddWithValue("SoLuong", soLuong);
                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
               
            }
        }
        public int KiemTraDDHDaNhap(string masoDDH)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MADDH_DANHAPHANG", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("masoDDH", masoDDH);
               
                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        private void ListPhieuNhap_Load(object sender, EventArgs e)
        {
            dataSetQLVT.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dataSetQLVT.Kho' table. You can move, or remove it, as needed.
            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.dataSetQLVT.Kho);
            // TODO: This line of code loads data into the 'dataSetQLVT.CTPX' table. You can move, or remove it, as needed.
            //this.cTPXTableAdapter.Fill(this.dataSetQLVT.CTPX);
            btnThemCTPN.Enabled = btnThemPhieuNhap.Enabled = btnSuaPN.Enabled =  btnRefresh.Enabled = btnThoat.Enabled = false;
            btnLuu.Enabled = btnPhucHoi.Enabled = false;
            if (Program.mGroup.ToString() == "CHINHANH" || (Program.mGroup.ToString() == "USER"))
            {
                btnThemCTPN.Enabled = btnThemPhieuNhap.Enabled = btnSuaPN.Enabled =  btnRefresh.Enabled = btnThoat.Enabled = true;
                cmbChiNhanh.Enabled = false;
                // btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhuchoi.Enabled = btnRefresh.Enabled = true;
                // MessageBox.Show("group là " + Program.mGroup, "", MessageBoxButtons.OK);

            }
            else if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = true;
                btnRefresh.Enabled = btnThoat.Enabled = true;

            }
            // TODO: This line of code loads data into the 'dataSetQLVT.CTPN' table. You can move, or remove it, as needed.

           
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedValue = Program.mChinhanh;
            
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.dataSetQLVT.PhieuNhap);
            this.cTPNTableAdapter.Fill(this.dataSetQLVT.CTPN);

            txtMaKho.Text = cmbMaKho.SelectedValue.ToString();
            // TODO: This line of code loads data into the 'dataSetQLVT.PhieuNhap' table. You can move, or remove it, as needed.

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
                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.dataSetQLVT.PhieuNhap);
            }
        }

        private void BtnThemPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            phieuNhapGridControl.Enabled = false; //Che bảng NV

            //   txtMaKho.Text = Program.mlogin;
            groupBoxPhieuNhap.Enabled = true;
            DisableButton();

            btn_ThemPN = true;
            btn_ThemCTPN = false;
            btn_SuaPN = false;
            btn_SuaCTPN = false;
            btnLuu.Enabled = true;
            gridView1.AddNewRow();
            txtMaCTPN.Text = txtMaPN.Text;
            txtMaNV.Text = Program.username;
            txtMaKho.Text = cmbMaKho.SelectedValue.ToString();
        }

        private void BtnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btn_ThemPN == true)
            {
                if (txtMaPN.Text.Trim().ToString() == "")
                {
                    MessageBox.Show("bạn chưa điền mã DDH", "", MessageBoxButtons.OK);
                }
                else if (DateEditNgay.Text.Trim().ToString() == "" || txtmasoDDH.Text.Trim().ToString() == "" || txtMaKho.Text.Trim() == "")
                {
                    MessageBox.Show("bạn chưa điền đầy đủ thông tin", "", MessageBoxButtons.OK);
                }
                else if (FormListHoaDon.kiemTraMaKho(txtMaKho.Text.Trim().ToString()) == 0)
                {
                    MessageBox.Show("mã kho bạn nhập không có ở chi nhánh này ", "", MessageBoxButtons.OK);
                }
                else if (KiemTraDDHDaNhap(txtmasoDDH.Text.Trim().ToString()) == 1)
                {
                    MessageBox.Show(" đơn đặt hàng này đã nhập hàng ", "", MessageBoxButtons.OK);
                }
                else
                {      // txtMaNV.Text = Program.username;
                       //MessageBox.Show(txtMaNV.Text);
                       //dataSetQLVT.EnforceConstraints = false;
                    try
                    {
                        ((DataRowView)phieuNhapBindingSource.Current)["MAKHO"] = txtMaKho.Text;
                        phieuNhapBindingSource.EndEdit(); //k cho chỉnh sửa nữa
                        phieuNhapBindingSource.ResetCurrentItem();
                        this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.phieuNhapTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL 
                       
                        MessageBox.Show("Thêm phiếu nhập thành công", "", MessageBoxButtons.OK);
                        ResetDefault();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm đơn đặt hàng. Xin thử lại, lỗi :\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                    }

                }

            }
            else if (btn_ThemCTPN==true)
            {
                if (FormListHoaDon.kiemTraMaVT(txtMaVT.Text.Trim().ToString()) == 1)
                {
                    try
                    {
                        cTPNBindingSource.EndEdit(); //k cho chỉnh sửa nữa
                        cTPNBindingSource.ResetCurrentItem();
                        this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.cTPNTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                        capNhatSoLuongVatTu(txtMaVT.Text.Trim().ToString(), Int32.Parse(SpinEditSoLuong.Text.ToString()));
                        MessageBox.Show("thêm chi tiết PN thành công", "", MessageBoxButtons.OK);

                        ResetDefault();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm chi tiết đơn đặt hàng. Xin thử lại, lỗi :\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("MÃ VẬT TƯ NHẬP SAI", "", MessageBoxButtons.OK);
                }
            }
        }

        private void BtnThemCTPN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            DisableButton();
            cTPNGridControl.Enabled = false;
            btn_ThemCTPN = true;
            btn_ThemPN = false;
            btn_SuaPN = false;
            btn_SuaCTPN = false;
            groupBoxPhieuNhap.Enabled = false;
            groupBoxCTPN.Enabled = true;
            btnLuu.Enabled = true;
            gridView2.AddNewRow();
            txtMaPN.Text = ((DataRowView)phieuNhapBindingSource[0])["MAPN"].ToString();
        }

        private void CmbMaKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaKho.ValueMember != "" && cmbMaKho.SelectedValue != null)
                txtMaKho.Text = cmbMaKho.SelectedValue.ToString();
        }

        private void PhieuNhapGridControl_Click(object sender, EventArgs e)
        {
            Point clickPoint = phieuNhapGridControl.PointToClient(Control.MousePosition);
            var hitInfo = gridView1.CalcHitInfo(clickPoint);
            if (hitInfo.InRowCell)
            {
                // hàng đang click vào
                int rowHandle = hitInfo.RowHandle;
                // cột đang click vào
                GridColumn column = hitInfo.Column;

                txtMaKho.Text = ((DataRowView)phieuNhapBindingSource[rowHandle])["MAKHO"].ToString();
            }
        }
    }
}
