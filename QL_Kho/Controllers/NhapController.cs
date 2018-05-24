using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;
namespace QL_Kho.Controllers
{
    public class NhapController : Controller
    {
        // GET: Nhap
        public ActionResult Index()
        {
            return View();
        }

        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Xacnhan()
        {
            return View();
        }
        public List<Nhap> Layds()
        {
            List<Nhap> lstGiohang = Session["Nhap"] as List<Nhap>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Nhap>();
                Session["Nhap"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGiohang(int iMaMH, string strURL)
        {
            List<Nhap> lstGioHang = Layds();
            Nhap sanpham = lstGioHang.Find(n => n.iMaMH == iMaMH);
            if (sanpham == null)
            {
                sanpham = new Nhap(iMaMH);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }


        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Nhap> lstGiohang = Session["Nhap"] as List<Nhap>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<Nhap> lstGiohang = Session["Nhap"] as List<Nhap>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);

            }
            return iTongTien;
        }

        public ActionResult Nhap()
        {
            List<Nhap> lstGiohang = Layds();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Mathang");
            }
            TempData["LoiSoLuong"] = null;
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            
            return View(lstGiohang);
        }

        public ActionResult NhapPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        public ActionResult XoaNhap(int iMaSP)
        {
            List<Nhap> lstGiohang = Layds();
            Nhap sanpham = lstGiohang.SingleOrDefault(n => n.iMaMH == iMaSP);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaMH == iMaSP);
                return RedirectToAction("Nhap");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Mathang");
            }
            return RedirectToAction("Nhap");
        }

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //int maxIdXuat = (from ctx in kho.CTPHIEUXUATs select ctx.Maxuat).Max();
            //var SoLuongXuat = (from ctx in kho.CTPHIEUXUATs
            //                   where ctx.MaMH == iMaSP && ctx.Maxuat == maxIdXuat
            //                   select ctx.SoluongXuat).SingleOrDefault();
            //var SoLuong = (from mh in kho.Mathangs
            //               where mh.MaMH == iMaSP
            //               select mh.SoLuong).SingleOrDefault();

            //if (SoLuongXuat > SoLuong)
            //{
            //    TempData["LoiSoLuong"] = "Số lượng xuất phải ít hơn số lượng mặt hàng trong kho!";
            //}

            List<Nhap> lstGiohang = Layds();
            
                Nhap sanpham = lstGiohang.SingleOrDefault(n => n.iMaMH == iMaSP);
                if (sanpham != null)
                {
                    sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
                }
                return RedirectToAction("Nhap");
            
        }
        public ActionResult Xoatatcagiohang()
        {
            List<Nhap> lstGiohang = Layds();
            lstGiohang.Clear();
            return RedirectToAction("Index", "Admin");
        }


        //Dat hang -------------------
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            if (Session["Nhap"] == null)
            {
                return RedirectToAction("Nhap", "Nhap");
            }
            List<Nhap> lstGiohang = Layds();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        //xac nhan thong tin --------------------------------
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            Phieuxuat ddh = new Phieuxuat();
            Nhanvien nv = (Nhanvien)Session["Taikhoan"];
            try
            {

                List<Nhap> gh = Layds();
                ddh.NhanVien = nv.MaNV;
                ddh.Ngaylap = DateTime.Now;

                kho.Phieuxuats.InsertOnSubmit(ddh);
                kho.SubmitChanges();
                foreach (var item in gh)
                {
                    CTPHIEUXUAT ctpn = new CTPHIEUXUAT();
                    ctpn.Maxuat = ddh.MaXuat;
                    ctpn.MaMH = item.iMaMH;
                    ctpn.SoluongXuat = item.iSoluong;
                    ctpn.GiaXuat = item.dDonggia;
                    ctpn.TongTien = item.dThanhtien;
                    kho.CTPHIEUXUATs.InsertOnSubmit(ctpn);

                }
                kho.SubmitChanges();
                //Session["Nhap"] = null;
                return RedirectToAction("Xacnhan", "Nhap");
            }
            catch
            {
                TempData["LoiSoLuong"] = "Số lượng xuất phải ít hơn số lượng mặt hàng trong kho!";
            return RedirectToAction("DatHang");
            }

        }

        ///////nhap hang
        [HttpGet]
        public ActionResult Chonhang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            if (Session["Nhap"] == null)
            {
                return RedirectToAction("Nhap", "Nhap");
            }
            List<Nhap> lstGiohang = Layds();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        //xac nhan thong tin --------------------------------
        [HttpPost]
        public ActionResult Chonhang(FormCollection collection)
        {
            Phieunhap ddh = new Phieunhap();
            Nhanvien nv = (Nhanvien)Session["Taikhoan"];
            List<Nhap> gh = Layds();
            ddh.NhanVien = nv.MaNV;
            ddh.Ngaylap = DateTime.Now;

            kho.Phieunhaps.InsertOnSubmit(ddh);
            kho.SubmitChanges();
            foreach (var item in gh)
            {
                CTPHIEUNHAP ctpx = new CTPHIEUNHAP();
                ctpx.Manhap = ddh.MaNhap;
                ctpx.MaMH = item.iMaMH;
                ctpx.SoluongNhap = item.iSoluong;
                ctpx.Gianhap = item.dDonggia;
                ctpx.Tongtien = item.dThanhtien;
                kho.CTPHIEUNHAPs.InsertOnSubmit(ctpx);
                //ctdh.Masach = item.iMasach;
                //ctdh.Soluong = item.iSoluong;
                //ctdh.Dongia = (decimal)item.dDonggia;
                //data.CTDATHANGs.InsertOnSubmit(ctdh);


            }
            kho.SubmitChanges();
            Session["Nhap"] = null;
            return RedirectToAction("Xacnhan", "Nhap");
        }

    }
}