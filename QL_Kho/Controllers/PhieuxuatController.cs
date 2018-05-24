using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class PhieuxuatController : Controller
    {
        // GET: Phieuxuat
        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Index()
        {
            return View(kho.Phieuxuats.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Phieuxuat px)
        {
            kho.Phieuxuats.InsertOnSubmit(px);
            kho.SubmitChanges();
            return RedirectToAction("Index", "Phieuxuat");
        }

        public ActionResult Details(int id)
        {
            ViewBag.MaPhieu = id;
            var TenNhanVien =
            (from nv in kho.Nhanviens
             join px in kho.Phieuxuats on nv.MaNV equals px.NhanVien into pnGroup
             from px2 in pnGroup
             where px2.MaXuat == id
             select nv.Ten).SingleOrDefault();
            ViewBag.TenNhanVien = TenNhanVien;

            var LayNgayXuat =
            (from nv in kho.Nhanviens
             join px in kho.Phieuxuats on nv.MaNV equals px.NhanVien into pnGroup
             from px2 in pnGroup
             where px2.MaXuat == id
             select px2.Ngaylap).SingleOrDefault();
            ViewBag.NgayLap = LayNgayXuat;

            var data = kho.CTPHIEUXUATs.Where(x => x.Maxuat == id).ToList();
            return View(data);

        }
        public ActionResult DetailsCT(int? id)
        {
            var idPhieu = id;
            var data = kho.CTPHIEUXUATs.Where(x => x.Maxuat== id).ToList();
            return View(data);
        }

       
        
    }
}