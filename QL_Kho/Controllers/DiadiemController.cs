using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class DiadiemController : Controller
    {

        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Index()
        {

            return View(kho.Diadiems.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection collection,Diadiem dd)
        {
           

            kho.Diadiems.InsertOnSubmit(dd);
            kho.SubmitChanges();
            return RedirectToAction("Index", "Diadiem");
        }

        public ActionResult Details(int id)
        {
            Diadiem dd = kho.Diadiems.SingleOrDefault(n => n.MaDD == id);
            ViewBag.MaDD = dd.MaDD;
            if (dd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dd);

        }
        public ActionResult Delete(int id)
        {
            Diadiem dd = (from s in kho.Diadiems
                          where s.MaDD == id
                         select s).SingleOrDefault(); // truy vấn lấy sách ra sách tại id hiện tại
            if (dd == null) //nếu câu truy vấn trên ko tồn tại, trả về lỗi http
            {
                return HttpNotFound();
            }
            return View(dd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanxoa(int id)
        {

            try //bắt lỗi khi xóa sách
            {
                kho.Diadiems.DeleteOnSubmit(
                    kho.Diadiems.Where(s => s.MaDD == id).SingleOrDefault()); //thực hiện xóa dữ liệu đang được truy vấn tại Masach=id hiện tại
                kho.SubmitChanges(); //lưu vào database nếu xóa thành công
                return RedirectToAction("Index","Diadiem"); // trả về trang quản lý sách
            }
            catch //bắt lỗi
            {
                //nếu có lỗi khi xóa sách sẽ in ra tempdata
                TempData["LoiXoaSach"] = "Không thể xóa địa điểm này vì vẫn còn kho hàng tại địa điểm đó";
                return RedirectToAction("Delete"); //nếu lỗi sẽ trả lại về view Xoasach
            }



        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Diadiem dd = kho.Diadiems.SingleOrDefault(n => n.MaDD == id);
            ViewBag.MaDD = dd.MaDD;
            if (dd == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(dd);
        }
   
        [HttpPost]

        [ValidateInput(false)]

        public ActionResult Edit(Diadiem dd)
        {
            Diadiem Đ = kho.Diadiems.ToList().Find(n => n.MaDD == dd.MaDD);
            Đ.MaDD = dd.MaDD;
            Đ.TenDD = dd.TenDD;
            Đ.Quanly = dd.Quanly;
            kho.SubmitChanges();

            return RedirectToAction("Index", "Diadiem");
        }

    }
}


