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
    public partial class FormListHoaDon : Form
    {
        private bool btn_ThemDDH = false;
        private bool btn_ThemCTHD = false;
        private bool btn_SuaCTDDH = false;
        private bool btn_SuaDDH = false;
        private int viTri_DDH = 0;
        private int viTri_CTHD = 0;
        public FormListHoaDon()
        {
            InitializeComponent();
        }
        private int KiemTraMaDDH(string maDDH)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MADDH", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MADDH", maDDH);

                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        public static int kiemTraMaKho(string maKho)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MAKHO_PN", conn))
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
        public static int kiemTraMaVT(string maVT)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_KIEMTRA_MAVT_TONTAI", conn))
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
        public int kiemTraChoPhepXoaDDH(string maDDH)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_CHOPHEPXOA_DDH", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MasoDDH", maDDH);

                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                return (int)returnParameter.Value;
            }
        }
        public void XoaAllCTDDH(string maDDH)
        {
            using (SqlConnection conn = new SqlConnection(Program.connstr))
            using (SqlCommand cmd = new SqlCommand("SP_XOA_ALL_DDH", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("MasoDDH", maDDH);

                var returnParameter = cmd.Parameters.Add("@result", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                conn.Open();
                cmd.ExecuteNonQuery();
                
            }
        }
        private void DatHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.datHangBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSetQLVT);

        }

        private void ListHoaDon_Load(object sender, EventArgs e)
        {
            btnThemCTHD.Enabled = btnThemDDH.Enabled = btnSuaCTDDH.Enabled = btnSuaDDH.Enabled = btnXoaDDH.Enabled = btnXoaCTDDH.Enabled = btnPhucHoi.Enabled = false;
            if (Program.mGroup.ToString() == "CHINHANH" || (Program.mGroup.ToString() == "USER"))
            {
                // btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnLuu.Enabled = btnPhuchoi.Enabled = btnRefresh.Enabled = true;
                // MessageBox.Show("group là " + Program.mGroup, "", MessageBoxButtons.OK);
                btnThemCTHD.Enabled = btnThemDDH.Enabled = btnSuaCTDDH.Enabled = btnSuaDDH.Enabled = btnXoaDDH.Enabled = btnXoaCTDDH.Enabled = true;
                btnPhucHoi.Enabled = btnLuu.Enabled = false;
                cmbChiNhanh.Enabled = false;
            }
            else if (Program.mGroup == "CONGTY")
            {
                btnThemCTHD.Enabled = btnThemDDH.Enabled = btnSuaCTDDH.Enabled = btnSuaDDH.Enabled = btnXoaDDH.Enabled = btnXoaCTDDH.Enabled = btnPhucHoi.Enabled = false;
                cmbChiNhanh.Enabled = true;
            }
            //this.vattuTableAdapter.Fill(this.dataSetQLVT.Vattu);
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập  qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedValue = Program.mChinhanh;
            

            dataSetQLVT.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dataSetQLVT.Kho' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.dataSetQLVT.DatHang);
            // TODO: This line of code loads data into the 'dataSetQLVT.CTDDH' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Fill(this.dataSetQLVT.CTDDH);
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.dataSetQLVT.Vattu);

            groupBoxDDH.Enabled = false;
            groupBoxCTDDH.Enabled = false;
            btnLuu.Enabled = false;
            txtMaKho.Enabled = true;
            txtMaVT.Text = cmbMaVT.SelectedValue.ToString();
            // txtMaKho.Text = Program.mGroup;

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
                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.dataSetQLVT.DatHang);
            }
        }
        private void ResetDefault()
        {
            btnThemCTHD.Enabled = btnThemDDH.Enabled = btnSuaDDH.Enabled = btnSuaCTDDH.Enabled = btnXoaDDH.Enabled = btnXoaCTDDH.Enabled = btnRefresh.Enabled = btnThoat.Enabled = true;
            btnLuu.Enabled = btnPhucHoi.Enabled = false;
            groupBoxCTDDH.Enabled = groupBoxDDH.Enabled = false;
            datHangGridControl.Enabled = cTDDHGridControl.Enabled = true;
        }
        private void DisableButton()
        {
            btnThemDDH.Enabled = btnThemCTHD.Enabled = btnSuaDDH.Enabled = btnSuaCTDDH.Enabled = btnXoaDDH.Enabled = btnXoaCTDDH.Enabled = false;
            btnLuu.Enabled = btnPhucHoi.Enabled = btnRefresh.Enabled = btnThoat.Enabled = true;
        }
        private void BtnThemDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            datHangGridControl.Enabled = false; //Che bảng NV

            //   txtMaKho.Text = Program.mlogin;
            groupBoxDDH.Enabled = true;
            DisableButton();

            btn_ThemDDH = true;
            btn_ThemCTHD = false;
            btn_SuaDDH = false;
            btn_SuaCTDDH = false;
            btnLuu.Enabled = true;
            gridView1.AddNewRow();
            txtMaNV.Text = Program.username;
            // txtMaSoDDH.Text = ((DataRowView)datHangBindingSource[0])["MasoDDH"].ToString();
        }

        private void BtnLuu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (btn_ThemDDH == true)
            {

                if (txtMaSoDDH.Text.Trim().ToString() == "")
                {
                    MessageBox.Show("bạn chưa điền mã DDH", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
             
                else if (dateEditNgay.Text.Trim().ToString() == "" || txtNhaCC.Text.Trim().ToString() == "" || txtNhaCC.Text.Trim() == "")
                {
                    MessageBox.Show("bạn chưa điền đầy đủ thông tin", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (KiemTraMaDDH(txtMaSoDDH.Text.Trim().ToString()) == 1)
                {
                    MessageBox.Show("Mã đơn đặt hàng đã tồn tại", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (kiemTraMaKho(txtMaKho.Text.Trim().ToString()) == 0)
                {
                    MessageBox.Show("mã kho bạn nhập không có ở chi nhánh này ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {      // txtMaNV.Text = Program.username;
                       //MessageBox.Show(txtMaNV.Text);
                       //dataSetQLVT.EnforceConstraints = false;
                    try
                    {
                        datHangBindingSource.EndEdit(); //k cho chỉnh sửa nữa
                        datHangBindingSource.ResetCurrentItem();
                        this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.datHangTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL 
                        MessageBox.Show("Thêm đơn đặt hàng thành công", "", MessageBoxButtons.OK);
                        ResetDefault();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi thêm đơn đặt hàng. Xin thử lại, lỗi :\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                    }

                }

            }
            else if (btn_ThemCTHD == true)
            {
                if (kiemTraMaVT(txtMaVT.Text.Trim().ToString()) == 1)
                {
                    try
                    {
                        ((DataRowView)cTDDHBindingSource.Current)["MAVT"] = txtMaVT.Text;
                        cTDDHBindingSource.EndEdit(); //k cho chỉnh sửa nữa
                        cTDDHBindingSource.ResetCurrentItem();
                        this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.cTDDHTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                        MessageBox.Show("thêm chi tiết DDH thành công", "", MessageBoxButtons.OK);
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
            else if (btn_SuaDDH)
            {
                try
                {
                    datHangBindingSource.EndEdit(); // thông báo k cho chỉnh sửa nữa
                    datHangBindingSource.ResetCurrentItem();
                    this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.datHangTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                                                                       // btn_SuaCTDDH = false;
                    btn_SuaDDH = false;
                    MessageBox.Show("sửa thành công", "", MessageBoxButtons.OK);
                    ResetDefault();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi sửa đơn đặt hàng. Xin thử lại, lỗi :\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                }

            }
            else if (btn_SuaCTDDH == true)
            {
                if (kiemTraMaVT(txtMaVT.Text.Trim().ToString()) == 1)
                {
                    try
                    {
                        cTDDHBindingSource.EndEdit(); // thông báo k cho chỉnh sửa nữa
                        cTDDHBindingSource.ResetCurrentItem();
                        this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.cTDDHTableAdapter.Update(this.dataSetQLVT); // Đẩy về CSDL
                                                                         // btn_SuaCTDDH = false;
                        btn_SuaDDH = false;
                        btn_ThemDDH = false;
                        btn_SuaCTDDH = false;
                        MessageBox.Show("sửa thành công", "", MessageBoxButtons.OK);
                        ResetDefault();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi sửa chi tiết đơn đặt hàng. Xin thử lại, lỗi :\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("MÃ VẬT TƯ NHẬP SAI", "", MessageBoxButtons.OK);
                }



            }
            txtMaSoDDH.Enabled = true;
        }

        private void BtnThemCTHD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DisableButton();
            cTDDHGridControl.Enabled = false;
            btn_ThemCTHD = true;
            btn_ThemDDH = false;
            btn_SuaDDH = false;
            btn_SuaCTDDH = false;
            groupBoxDDH.Enabled = false;
            groupBoxCTDDH.Enabled = true;
            btnLuu.Enabled = true;
            gridView2.AddNewRow();
            txtMaSoCTDDH.Text = txtMaSoDDH.Text;
            txtMaSoDDH.Text = ((DataRowView)datHangBindingSource[0])["MasoDDH"].ToString();
            txtMaVT.Text = cmbMaVT.SelectedValue.ToString();
            //((DataRowView)cTDDHBindingSource.Current)["MAVT"] = txtMaVT.Text;
        }

  
        private void BtnPhucHoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //MessageBox.Show("bạn chưa điền mã DDH " + Program.mGroup, "", MessageBoxButtons.OK);
            datHangGridControl.Enabled = true;
            cTDDHGridControl.Enabled = true;
            btnThemCTHD.Enabled = btnThemDDH.Enabled = btnSuaDDH.Enabled = btnSuaCTDDH.Enabled = btnXoaDDH.Enabled = btnXoaCTDDH.Enabled = true;
            if (btn_ThemDDH == true || btn_SuaDDH == true)
            {
                datHangBindingSource.CancelEdit();
                if (btn_ThemDDH == false) datHangBindingSource.Position = viTri_DDH;
                datHangGridControl.Enabled = true;
                groupBoxDDH.Enabled = false;
                groupBoxCTDDH.Enabled = false;

            }
            else if (btn_ThemCTHD == true || btn_SuaCTDDH == true)
            {
                cTDDHBindingSource.CancelEdit();
                if (btn_ThemCTHD == false) cTDDHBindingSource.Position = viTri_CTHD;
                datHangGridControl.Enabled = true;
                groupBoxDDH.Enabled = false;
                groupBoxCTDDH.Enabled = false;


            }

        }

        //private void BtnSuaCTDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    DisableButton();
        //    btn_ThemCTHD = false;
        //    btn_ThemDDH = false;
        //    btn_SuaDDH = false;
        //    btn_SuaCTDDH = true;
        //    groupBoxCTDDH.Enabled = true;
        //    groupBoxDDH.Enabled = false;
        //    viTri_CTHD = cTDDHBindingSource.Position;
        //    cTDDHGridControl.Enabled = false;
        //}

        private void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ResetDefault();
        }

        //private void BtnXoaDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    string maDDH = "";
        //    maDDH = ((DataRowView)datHangBindingSource[datHangBindingSource.Position])["MasoDDH"].ToString();
        //    if (kiemTraChoPhepXoaDDH(maDDH) == 1)
        //    {
        //        MessageBox.Show("không được phép xoá vì đã nhập phiếu nhập");
        //    }
        //    else
        //    {
        //        if (MessageBox.Show("Bạn có thật sự muốn xóa vật tư này ?? ", "Xác nhận",
        //                            MessageBoxButtons.OKCancel) == DialogResult.OK)
        //        {
        //            try
        //            {
        //                XoaAllCTDDH(maDDH);
        //                maDDH = ((DataRowView)datHangBindingSource[datHangBindingSource.Position])["MasoDDH"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
        //                datHangBindingSource.RemoveCurrent();
        //                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
        //                this.datHangTableAdapter.Update(this.dataSetQLVT.DatHang);
                       
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Lỗi xóa kho. Bạn hãy xóa lại\n" + ex.Message, "",
        //                    MessageBoxButtons.OK);
        //                this.datHangTableAdapter.Fill(this.dataSetQLVT.DatHang);
        //                datHangBindingSource.Position = datHangBindingSource.Find("MasoDDH", maDDH);
        //                return;
        //            }
        //        }
        //    }
        //}

        //private void BtnXoaCTDDH_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (MessageBox.Show("Bạn có thật sự muốn xóa chi tiết đặt hàng này ?? ", "Xác nhận",
        //                           MessageBoxButtons.OKCancel) == DialogResult.OK)
        //    {
        //        try
        //        {
        //           // makho = ((DataRowView)khoBindingSource[khoBindingSource.Position])["MAKHO"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
        //            cTDDHBindingSource.RemoveCurrent();
        //            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
        //            this.cTDDHTableAdapter.Update(this.dataSetQLVT.CTDDH);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Lỗi xóa chi tiết DDH. Bạn hãy xóa lại\n" + ex.Message, "",
        //                MessageBoxButtons.OK);
        //            this.cTDDHTableAdapter.Fill(this.dataSetQLVT.CTDDH);
        //            //khoBindingSource.Position = khoBindingSource.Find("MAKHO", makho);
        //            return;
        //        }
        //    }
        //}

        private void BtnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void BtnSuaDDH_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btn_SuaDDH = true;
            btn_SuaCTDDH = false;
            btn_ThemDDH = false;
            btn_ThemCTHD = false;
            groupBoxDDH.Enabled = true;
            txtMaSoDDH.Enabled = false;
            DisableButton();
            viTri_DDH = datHangBindingSource.Position;
            datHangGridControl.Enabled = false;
        }

        private void BtnXoaDDH_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maDDH = "";
            maDDH = ((DataRowView)datHangBindingSource[datHangBindingSource.Position])["MasoDDH"].ToString();
            if (kiemTraChoPhepXoaDDH(maDDH) == 1)
            {
                MessageBox.Show("không được phép xoá vì đã nhập phiếu nhập");
            }
            else
            {
                if (MessageBox.Show("Bạn có thật sự muốn xóa vật tư này ?? ", "Xác nhận",
                                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    try
                    {
                        XoaAllCTDDH(maDDH);
                        maDDH = ((DataRowView)datHangBindingSource[datHangBindingSource.Position])["MasoDDH"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                        datHangBindingSource.RemoveCurrent();
                        this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                        this.datHangTableAdapter.Update(this.dataSetQLVT.DatHang);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa kho. Bạn hãy xóa lại\n" + ex.Message, "",
                            MessageBoxButtons.OK);
                        this.datHangTableAdapter.Fill(this.dataSetQLVT.DatHang);
                        datHangBindingSource.Position = datHangBindingSource.Find("MasoDDH", maDDH);
                        return;
                    }
                }
            }
        }

        private void BtnXoaCTDDH_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn xóa chi tiết đặt hàng này ?? ", "Xác nhận",
                                   MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    // makho = ((DataRowView)khoBindingSource[khoBindingSource.Position])["MAKHO"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                    cTDDHBindingSource.RemoveCurrent();
                    this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTDDHTableAdapter.Update(this.dataSetQLVT.CTDDH);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa chi tiết DDH. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.cTDDHTableAdapter.Fill(this.dataSetQLVT.CTDDH);
                    //khoBindingSource.Position = khoBindingSource.Find("MAKHO", makho);
                    return;
                }
            }
        }

        private void BtnSuaCTDDH_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DisableButton();
            btn_ThemCTHD = false;
            btn_ThemDDH = false;
            btn_SuaDDH = false;
            btn_SuaCTDDH = true;
            groupBoxCTDDH.Enabled = true;
            groupBoxDDH.Enabled = false;
            viTri_CTHD = cTDDHBindingSource.Position;
            cTDDHGridControl.Enabled = false;
            ((DataRowView)cTDDHBindingSource.Current)["MAVT"] = txtMaVT.Text;
        }

        private void CmbMaVT_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaVT.ValueMember != "" && cmbMaVT.SelectedValue != null)
                txtMaVT.Text = cmbMaVT.SelectedValue.ToString();
        }

        private void CTDDHGridControl_Click(object sender, EventArgs e)
        {
            Point clickPoint = cTDDHGridControl.PointToClient(Control.MousePosition);
            var hitInfo = gridView2.CalcHitInfo(clickPoint);
            if (hitInfo.InRowCell)
            {
                // hàng đang click vào
                int rowHandle = hitInfo.RowHandle;
                // cột đang click vào
                GridColumn column = hitInfo.Column;

                txtMaVT.Text = ((DataRowView)cTDDHBindingSource[rowHandle])["MAVT"].ToString();
            }
        }
    }
}
