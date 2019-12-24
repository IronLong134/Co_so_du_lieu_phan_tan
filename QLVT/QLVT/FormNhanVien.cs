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
    public partial class FormNhanVien : Form
    {
        int vitri = 0;
        private bool kt_btnThem = false; //kiểm tra có đang bấm nút Thêm hay k
        private bool kt_btnSua = false;
        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void NhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.nhanVienBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetQLVT);

        }
        private int kiemTraChoPhepXoa(int maNV)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRACHOPHEPXOA_NV", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MANV", maNV);
                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }

        }
        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedValue = Program.mChinhanh;

            btnLuu.Enabled = false;
            txtTen.Enabled = txtHo.Enabled = txtMaNV.Enabled = txtDiaChi.Enabled = seLuong.Enabled = dateEditNgaySinh.Enabled = txtMaCN.Enabled = false;
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhuchoi.Enabled = btnRefresh.Enabled = true;
                cmbChiNhanh.Enabled = false;
            }
            else if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhuchoi.Enabled = btnRefresh.Enabled = false;
                cmbChiNhanh.Enabled = true;
            }
            else
            {
                cmbChiNhanh.Enabled = false;
            }
            dataSetQLVT.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dataSetQLVT.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.dataSetQLVT.NhanVien);

           
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
                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.dataSetQLVT.NhanVien);
            }
        }
        private int TaoMaNV()
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_TAO_MANV", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;

                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        private void BtnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtTen.Enabled = txtHo.Enabled = txtDiaChi.Enabled = seLuong.Enabled = dateEditNgaySinh.Enabled = true;
            kt_btnThem = true;
            vitri = nhanVienBindingSource.Position;
            groupBox1.Enabled = true;
            nhanVienGridControl.Enabled = false; //Che bảng NV
            gridView1.AddNewRow();
            txtMaCN.Text = ((DataRowView)nhanVienBindingSource[0])["MACN"].ToString(); //định dạng mã chi nhánh ở dòng đầu tiên
            txtMaCN.Enabled = false;
            //((DataRowView)nhanVienBindingSource.Current)["TrangThaiXoa"] = 0;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnPhuchoi.Enabled = btnLuu.Enabled = btnRefresh.Enabled = btnThoat.Enabled = true;

        }

        private void BtnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (txtMaNV.Text.Trim() == "")
            //{
            //    MessageBox.Show("Mã nhân viên không được thiếu!", "", MessageBoxButtons.OK);
            //    txtMaNV.Focus();
            //    return;
            //}
            if (txtHo.Text.Trim() == "")
            {
                MessageBox.Show("Họ nhân viên không được thiếu!", "", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (txtTen.Text.Trim() == "")
            {
                MessageBox.Show("Tên nhân viên không được thiếu!", "", MessageBoxButtons.OK);
                txtTen.Focus();
                return;
            }

            try
            {
                if (kt_btnThem == true)
                {
                    int maNV = TaoMaNV();
                    txtMaNV.Text = maNV.ToString();

                    nhanVienBindingSource.EndEdit(); // thông báo k cho chỉnh sửa nữa
                    nhanVienBindingSource.ResetCurrentItem();
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                    kt_btnThem = false;
                    kt_btnSua = false;
                }
                else if (kt_btnSua == true)
                {
                    nhanVienBindingSource.EndEdit(); // thông báo k cho chỉnh sửa nữa
                    nhanVienBindingSource.ResetCurrentItem();
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                    kt_btnThem = false;
                    kt_btnSua = false;
                }
                else
                {
                    nhanVienBindingSource.EndEdit(); // thông báo k cho chỉnh sửa nữa
                    nhanVienBindingSource.ResetCurrentItem();
                    this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.nhanVienTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi nhân viên.\n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }

            nhanVienGridControl.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhuchoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;

            groupBox1.Enabled = false;
        }

        private void BtnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            txtTen.Enabled = txtHo.Enabled = txtDiaChi.Enabled = seLuong.Enabled = dateEditNgaySinh.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = false;
            vitri = nhanVienBindingSource.Position;
            kt_btnSua = true;
        }

        private void BtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maNV = "";
            maNV = ((DataRowView)nhanVienBindingSource[nhanVienBindingSource.Position])["MANV"].ToString();

            if (kiemTraChoPhepXoa(Int32.Parse(maNV)) == 1)
            {
                MessageBox.Show("không được phép xoá vì nhân viên đã tồn tại trong hoá đơn, phiếu nhập hoặc phiếu xuất");
            }
            else
            {
                if (MessageBox.Show("Bạn có thật sự muốn xóa vật tư này ?? ", "Xác nhận",
                                     MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        maNV = ((DataRowView)nhanVienBindingSource[nhanVienBindingSource.Position])["MANV"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                        nhanVienBindingSource.RemoveCurrent();
                        this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.nhanVienTableAdapter.Update(this.dataSetQLVT.NhanVien);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa vật tư. Bạn hãy xóa lại\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                        this.nhanVienTableAdapter.Fill(this.dataSetQLVT.NhanVien);
                        nhanVienBindingSource.Position = nhanVienBindingSource.Find("MANV", maNV);
                        return;
                    }
                }
            }

        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.nhanVienTableAdapter.Fill(this.dataSetQLVT.NhanVien);
                nhanVienGridControl.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhuchoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;
                txtTen.Enabled = txtHo.Enabled = txtDiaChi.Enabled = seLuong.Enabled = dateEditNgaySinh.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void BtnPhuchoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            nhanVienBindingSource.CancelEdit();
            if (btnThem.Enabled == false) nhanVienBindingSource.Position = vitri;
            nhanVienGridControl.Enabled = true;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhuchoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;
        }
    }
}
