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
    public partial class FormKho : Form
    {
        private bool kt_btnThem = false;
        private bool kt_btnSua = false;
        int vitri = 0;
        public FormKho()
        {
            InitializeComponent();
        }

        private void KhoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.khoBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetQLVT);

        }
        private int KiemTraMaKho(string maKho, string tenKho,int action)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MAKHO", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MAKHO", maKho);
                cmd.Parameters.AddWithValue("TENKHO", tenKho);
                cmd.Parameters.AddWithValue("ACTION", action);

                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        private int KiemTraMaKhoChoXoa(string maKho)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MAKHO_CHOPHEPXOA", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MAKHO", maKho);    
                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        private void FormKho_Load(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            if (Program.mGroup == "CHINHANH" || Program.mGroup == "USER")
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhucHoi.Enabled = btnRefresh.Enabled = true;
                cmbChiNhanh.Enabled = false;
            }
            else if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhucHoi.Enabled = btnRefresh.Enabled = false;
                cmbChiNhanh.Enabled = true;
            }
            else
            {
                cmbChiNhanh.Enabled = false;
            }
            dataSetQLVT.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dataSetQLVT.ChiNhanh' table. You can move, or remove it, as needed.
            this.chiNhanhTableAdapter.Fill(this.dataSetQLVT.ChiNhanh);

            // TODO: This line of code loads data into the 'dataSetQLVT.Kho' table. You can move, or remove it, as needed.
            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.dataSetQLVT.Kho);



            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
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
                this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.khoTableAdapter.Fill(this.dataSetQLVT.Kho);
            }
        }

        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            kt_btnThem = true;
            kt_btnSua = false;
            groupBox1.Enabled = true;
            txtMaKho.Enabled = true;
            vitri = khoBindingSource.Position;
            khoGridControl.Enabled = false; //Che bảng NV
            gridView1.AddNewRow();
            txtMaCN.Text = ((DataRowView)khoBindingSource[0])["MACN"].ToString(); //định dạng mã chi nhánh ở dòng đầu tiên
            txtMaCN.Enabled = false;

            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnPhucHoi.Enabled = btnLuu.Enabled = btnRefresh.Enabled = btnThoat.Enabled = true;
        }

        private void BtnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            groupBox1.Enabled = true;
            khoGridControl.Enabled = false; //Che bảng NV
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = false;
            vitri = khoBindingSource.Position;
            kt_btnSua = true;
            kt_btnThem = false;
        }

        private void BtnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtTenKho.Text.Trim() == "")
            {
                MessageBox.Show("bạn chưa điền tên kho hoặc tên kho chưa hợp lệ", "", MessageBoxButtons.OK);
                txtTenKho.Focus();
                return;
            }
            else if (txtMaKho.Text.Trim() == "")
            {
                MessageBox.Show("bạn chưa điền mã kho", "", MessageBoxButtons.OK);
                txtMaKho.Focus();
                return;
            }
            else if (txtDiaChi.Text.Trim() == "")
            {
                MessageBox.Show("bạn chưa điền địa chỉ", "", MessageBoxButtons.OK);
                txtDiaChi.Focus();
                return;
            }
            else if (kt_btnThem == true && KiemTraMaKho(txtMaKho.Text.Trim().ToString(), txtTenKho.Text.Trim().ToString(),1) == 1)
            {
                MessageBox.Show("Mã Kho đã tồn tại.\n Vui lòng nhập lại" , "", MessageBoxButtons.OK);
                return;
            }
            else if ( kt_btnThem == true && KiemTraMaKho(txtMaKho.Text.ToString(), txtTenKho.Text.Trim().ToString(),1) == 2)
            {
                MessageBox.Show("Tên Kho đã tồn tại.\n Vui lòng nhập lại", "", MessageBoxButtons.OK);
                return;
            }
            else if (kt_btnSua == true && KiemTraMaKho(txtMaKho.Text.ToString(), txtTenKho.Text.Trim().ToString(), 2) == 2)
            {
                MessageBox.Show("Tên Kho đã tồn tại.\n Vui lòng nhập lại", "", MessageBoxButtons.OK);
                return;
            }
            else
            {
                try
                {
                    if (kt_btnSua == true || kt_btnThem == true)
                    {
                        khoBindingSource.EndEdit(); //k cho chỉnh sửa nữa
                        khoBindingSource.ResetCurrentItem();
                        this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.khoTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi ghi vật tư.\n" + ex.Message, "", MessageBoxButtons.OK);
                    return;
                }
            }


            khoGridControl.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;

            groupBox1.Enabled = false;
        }

        private void BtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            khoBindingSource.CancelEdit();
            if (btnThem.Enabled == false) khoBindingSource.Position = vitri;
            khoGridControl.Enabled = true;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;
        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.khoTableAdapter.Fill(this.dataSetQLVT.Kho);
                khoGridControl.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;
                groupBox1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void BtnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void BtnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string makho = "";
            makho = ((DataRowView)khoBindingSource[khoBindingSource.Position])["MAKHO"].ToString();
            if (KiemTraMaKhoChoXoa(makho) == 1)
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
                        makho = ((DataRowView)khoBindingSource[khoBindingSource.Position])["MAKHO"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                        khoBindingSource.RemoveCurrent();
                        this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.khoTableAdapter.Update(this.dataSetQLVT.Kho);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa kho. Bạn hãy xóa lại\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                        this.khoTableAdapter.Fill(this.dataSetQLVT.Kho);
                        khoBindingSource.Position = khoBindingSource.Find("MAKHO", makho);
                        return;
                    }
                }
            }
        }
    }
}
