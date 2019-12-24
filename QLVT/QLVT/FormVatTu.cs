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
    public partial class FormVatTu : Form
    {
        int vitri = 0;
        bool kt_Them = false;
        bool kt_Sua = false;

        public FormVatTu()
        {
            InitializeComponent();
        }

        private int kiemTraChoPhepXoa_VT(string maVT)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRACHOPHEPXOA_VT", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MAVT", maVT);

                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        private void FormVatTu_Load(object sender, EventArgs e)
        {
            txtTENVT.Enabled = txtMAVT.Enabled = txtDVT.Enabled = seSOLUONGTON.Enabled = false;
            dataSetQLVT.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dataSetQLVT.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.dataSetQLVT.Vattu);
            if (Program.mGroup == "CONGTY")
            {
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhucHoi.Enabled = btnRefresh.Enabled = false;
            }
            groupBox1.Enabled = false;
        }

        private void VattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.vattuBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetQLVT);
        }

        private void BtnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            txtTENVT.Enabled = txtMAVT.Enabled = txtDVT.Enabled = seSOLUONGTON.Enabled = true;
            gridView1.AddNewRow();
            vattuGridControl.Enabled = false;//che bảng
            kt_Them = true;
            kt_Sua = false;
            groupBox1.Enabled = true;
            btnThem.Enabled = false;

            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnRefresh.Enabled = btnLuu.Enabled = btnRefresh.Enabled = btnThoat.Enabled = true;
        }

        private void BtnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (kt_Them == true && KiemTraMaVT(txtMAVT.Text.Trim().ToString(), txtTENVT.Text.Trim().ToString(),1)==1)
            {
                MessageBox.Show("Mã VT đã tồn tại.\n Vui lòng nhập lại", "", MessageBoxButtons.OK);
                return;
            }
            else if (kt_Them == true && KiemTraMaVT(txtMAVT.Text.Trim().ToString(), txtTENVT.Text.Trim().ToString(), 1) == 2)
            {
                MessageBox.Show("Tên VT đã tồn tại.\n Vui lòng nhập lại", "", MessageBoxButtons.OK);
                return;
            }
            else if (kt_Sua == true && KiemTraMaVT(txtMAVT.Text.Trim().ToString(), txtTENVT.Text.Trim().ToString(), 2) == 1)
            {
                MessageBox.Show("Mã VT đã tồn tại.\n Vui lòng nhập lại", "", MessageBoxButtons.OK);
                return;
            }
            else if ( kt_Sua == true && KiemTraMaVT(txtMAVT.Text.Trim().ToString(), txtTENVT.Text.Trim().ToString(),2) == 1)
            {
                MessageBox.Show("Tên VT đã tồn tại.\n Vui lòng nhập lại", "", MessageBoxButtons.OK);
                return;
            }
            if (txtMAVT.Text.Trim() == "")
            {
                MessageBox.Show("Mã vật tư không được thiếu!", "", MessageBoxButtons.OK);
                txtMAVT.Focus();
                return;
            }
            if (txtTENVT.Text.Trim() == "")
            {
                MessageBox.Show("Tên vật tư không được thiếu!", "", MessageBoxButtons.OK);
                txtTENVT.Focus();
                return;
            }
            if (txtDVT.Text.Trim() == "")
            {
                MessageBox.Show("Đơn vị tính không được thiếu!", "", MessageBoxButtons.OK);
                txtDVT.Focus();
                return;
            }
            try
            {
                if (kt_Sua == true || kt_Them == true)
                {
                    vattuBindingSource.EndEdit(); //k cho chỉnh sửa nữa
                    vattuBindingSource.ResetCurrentItem();
                    this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.vattuTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi vật tư.\n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
            vattuGridControl.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;

            groupBox1.Enabled = false;
        }

        private static int KiemTraMaVT(string maVT, string tenVT, int action )
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MAVATTU", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MAVT", maVT);
                cmd.Parameters.AddWithValue("TENVT", tenVT);
                cmd.Parameters.AddWithValue("ACTION", action);


                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }

        //sửa
        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            vattuGridControl.Enabled = false;//che bảng
            groupBox1.Enabled = true;
            txtTENVT.Enabled  = txtDVT.Enabled = seSOLUONGTON.Enabled = true;
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = false;
            vitri = vattuBindingSource.Position;
            kt_Them = false;
            kt_Sua = true;
            txtMAVT.Enabled = false;
            //txtTENVT.Enabled = txtMAVT.Enabled = false;
        }

        //xoa
        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string mavt = "";
            mavt = ((DataRowView)vattuBindingSource[vattuBindingSource.Position])["MAVT"].ToString();
            if (kiemTraChoPhepXoa_VT(mavt) == 1)
            {
                MessageBox.Show("Không được phép xoá vật tư này vì vật tư đã tồn tại trong chi tiết phiếu nhập, phiếu xuất hoặc hoá đơn");
            }
            else
            {
                if (MessageBox.Show("Bạn có thật sự muốn xóa vật tư này ?? ", "Xác nhận",
                                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        mavt = ((DataRowView)vattuBindingSource[vattuBindingSource.Position])["MAVT"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                        vattuBindingSource.RemoveCurrent();
                        this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.vattuTableAdapter.Update(this.dataSetQLVT.Vattu);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa vật tư. Bạn hãy xóa lại\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                        this.vattuTableAdapter.Fill(this.dataSetQLVT.Vattu);
                        vattuBindingSource.Position = vattuBindingSource.Find("MAVT", mavt);
                        return;
                    }
                }
            }

        }

        private void BtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vattuBindingSource.CancelEdit();
            if (btnThem.Enabled == false) vattuBindingSource.Position = vitri;
            vattuGridControl.Enabled = true;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;
        }

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.vattuTableAdapter.Fill(this.dataSetQLVT.Vattu);
                vattuGridControl.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPhucHoi.Enabled = btnThoat.Enabled = btnLuu.Enabled = btnRefresh.Enabled = true;
                txtTENVT.Enabled = txtMAVT.Enabled = txtDVT.Enabled = seSOLUONGTON.Enabled = false;
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
    }
}
