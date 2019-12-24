using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLVT
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MA.Text = "Mã số: " + Program.username;
            TEN.Text = "Họ tên: " + Program.mHoten;
            NHOM.Text = "Nhóm: " + Program.mGroup;
        }

        //Kho
   

  

        private void BtnVattu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormVatTu));
            if (form != null)
                form.Activate();
            else
            {
                FormVatTu f = new FormVatTu();
                f.MdiParent = this;
                f.Show();
            }
        }

        //log out
        private void BarButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormLogin));
            if (form == null)
            {
                FormLogin f = new FormLogin();
                f.Show();
            }
            else
                form.Activate();

            this.Close();
        }

        private void BtnNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormNhanVien));
            if (form != null)
                form.Activate();
            else
            {
                FormNhanVien f = new FormNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void BtnKho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormKho));
            if (form != null)
                form.Activate();
            else
            {
                FormKho f = new FormKho();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void BtnListHoaDon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(FormListHoaDon));
            if (form != null)
                form.Activate();
            else
            {
                FormListHoaDon f = new FormListHoaDon();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void BtnDanhSachPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(ListPhieuNhap));
            if (form != null)
                form.Activate();
            else
            {
                ListPhieuNhap f = new ListPhieuNhap();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void BtnPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(ListPhieuXuat));
            if (form != null)
                form.Activate();
            else
            {
                ListPhieuXuat f = new ListPhieuXuat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void BtnInDanhSachNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form form = this.CheckExists(typeof(Report.FormInDanhSachNhanVien));
            if (form != null)
                form.Activate();
            else
            {
                Report.FormInDanhSachNhanVien f = new Report.FormInDanhSachNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}
