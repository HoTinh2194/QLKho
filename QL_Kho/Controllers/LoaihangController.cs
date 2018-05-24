using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class LoaihangController : Controller
    {
        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Index()
        {
            return View(kho.Loais.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Loai l)
        {
            kho.Loais.InsertOnSubmit(l);
            kho.SubmitChanges();
            return RedirectToAction("Index", "Loaihang");
        }

        public ActionResult Details(int id)
        {
            Loai l = kho.Loais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = l.MaLoai;
            if (l == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(l);

        }
        public ActionResult Delete(int id)
        {
            Loai l = (from s in kho.Loais
                         where s.MaLoai == id
                         select s).SingleOrDefault(); // truy vấn lấy sách ra sách tại id hiện tại
            if (l == null) //nếu câu truy vấn trên ko tồn tại, trả về lỗi http
            {
                return HttpNotFound();
            }
            return View(l);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanxoa(int id)
        {

            try //bắt lỗi khi xóa sách
            {
                kho.Loais.DeleteOnSubmit(
                    kho.Loais.Where(s => s.MaLoai == id).SingleOrDefault()); //thực hiện xóa dữ liệu đang được truy vấn tại Masach=id hiện tại
                kho.SubmitChanges(); //lưu vào database nếu xóa thành công
                return RedirectToAction("Index", "Loaihang"); // trả về trang quản lý sách
            }
            catch //bắt lỗi
            {
                //nếu có lỗi khi xóa sách sẽ in ra tempdata
                TempData["LoiXoaSach"] = "Không thể xóa loại hàng này vì vẫn còn mặt hàng thuộc loại đó";
                return RedirectToAction("Delete"); //nếu lỗi sẽ trả lại về view Xoasach
            }



        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Loai l = kho.Loais.SingleOrDefault(n => n.MaLoai == id);
            ViewBag.MaLoai = l.MaLoai;
            if (l == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(l);
        }

        [HttpPost]

        [ValidateInput(false)]

        public ActionResult Edit(Loai l)
        {
            Loai lo = kho.Loais.ToList().Find(n => n.MaLoai == l.MaLoai);
            lo.TenLoai = l.TenLoai;
            kho.SubmitChanges();
            return RedirectToAction("Index", "Kho");
        }


    }
}

