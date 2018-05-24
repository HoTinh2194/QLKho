using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class KhoController : Controller
    {
        // GET: Kho
        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Index()
        {
            return View(kho.KhoHangs.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
 
        public ActionResult Create(FormCollection collection, KhoHang k)
        {
            
           

            kho.KhoHangs.InsertOnSubmit(k);
            kho.SubmitChanges();
            return RedirectToAction("Index", "Kho");
        }

        public ActionResult Details(int id)
        {
            KhoHang k = kho.KhoHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = k.MaKH;
            if (k == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(k);

        }
        public ActionResult Delete(int id)
        {
            KhoHang k = (from s in kho.KhoHangs
                          where s.MaKH == id
                          select s).SingleOrDefault(); // truy vấn lấy sách ra sách tại id hiện tại
            if (k == null) //nếu câu truy vấn trên ko tồn tại, trả về lỗi http
            {
                return HttpNotFound();
            }
            return View(k);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanxoa(int id)
        {

            try //bắt lỗi khi xóa sách
            {
                kho.KhoHangs.DeleteOnSubmit(
                    kho.KhoHangs.Where(s => s.MaKH == id).SingleOrDefault()); //thực hiện xóa dữ liệu đang được truy vấn tại Masach=id hiện tại
                kho.SubmitChanges(); //lưu vào database nếu xóa thành công
                return RedirectToAction("Index", "Kho"); // trả về trang quản lý sách
            }
            catch //bắt lỗi
            {
                //nếu có lỗi khi xóa sách sẽ in ra tempdata
                TempData["LoiXoaSach"] = "Không thể xóa kho này vì kho này vẫn còn hàng hóa";
                return RedirectToAction("Delete"); //nếu lỗi sẽ trả lại về view Xoasach
            }



        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            KhoHang k = kho.KhoHangs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = k.MaKH;
            if (k == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(k);
        }

        [HttpPost]

        [ValidateInput(false)]

        public ActionResult Edit(KhoHang k)
        {
            KhoHang kh = kho.KhoHangs.ToList().Find(n => n.MaKH == k.MaKH);
            kh.MaKH = k.MaKH;
            kh.TenKH = k.TenKH;
            kh.MaNV = k.MaNV;
            kh.Diachi = k.Diachi;
            kh.MaDD = k.MaDD;
            kh.Loai = k.Loai;
            kh.SDT = k.SDT;
            kho.SubmitChanges();
            return RedirectToAction("Index", "Kho");
        }
    

    }
}

