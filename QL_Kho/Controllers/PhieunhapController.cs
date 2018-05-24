using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class PhieunhapController : Controller
    {
        // GET: Phieunhap
        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Index()
        {
            return View(kho.Phieunhaps.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection, Phieunhap pn)
        {
            kho.Phieunhaps.InsertOnSubmit(pn);
            kho.SubmitChanges();
            return RedirectToAction("Index", "Phieunhap");
        }

        public ActionResult Details(int id)
        {
            ViewBag.MaPhieu = id;
            var TenNhanVien =
            (from nv in kho.Nhanviens
             join pn in kho.Phieunhaps on nv.MaNV equals pn.NhanVien into pnGroup
             from pn2 in pnGroup
             where pn2.MaNhap == id
             select nv.Ten).SingleOrDefault();
            ViewBag.TenNhanVien = TenNhanVien;

            var LayNgayNhap =
            (from nv in kho.Nhanviens
             join pn in kho.Phieunhaps on nv.MaNV equals pn.NhanVien into pnGroup
             from pn2 in pnGroup
             where pn2.MaNhap == id
             select pn2.Ngaylap).SingleOrDefault();
            ViewBag.NgayLap = LayNgayNhap;

            var data = kho.CTPHIEUNHAPs.Where(x => x.Manhap == id).ToList();
            return View(data);
            //Phieunhap pn = kho.Phieunhaps.SingleOrDefault(n => n.MaNhap == id);
            //ViewBag.MaNhap = pn.MaNhap;
            //if (pn == null)
            //{
            //    Response.StatusCode = 404;
            //    return null;
            //}
            //return View(pn);


        }
        public ActionResult DetailsCT(int? id)
        {
            var idPhieu = id;
            var data = kho.CTPHIEUNHAPs.Where(x => x.Manhap == id).ToList();
            return View(data);
        }
        
    }
}