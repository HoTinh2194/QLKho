using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class MathangController : Controller
    {
        // GET: Mathang

        KhohangDataContext kho = new KhohangDataContext();

        public ActionResult Index()
        {

            return View(kho.Mathangs.ToList());
        }
        [HttpGet]
        public ActionResult ThemMH()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemMH(FormCollection collection, Mathang mh)
        {
            //var tenmh = collection["TenMH"];
            //var loaihang = collection["Loai"];
            //var soluong = collection["SoLuong"]);
            //var dongia = string.Format("{}", collection["Dongia"]);
            //if (String.IsNullOrEmpty(tenmh))
            //{
            //    ViewData["Loi1"] = "Tên mặt hàng không được để trống";
            //}
            //else if (string.IsNullOrEmpty(loaihang))
            //{
            //    ViewData["Loi2"] = " Phải chọn loại hàng";
            //}
            //else if (string.IsNullOrEmpty(soluong))
            //{
            //    ViewData["Loi3"] = " Phải nhập số lượng";
            //}
            //else if (string.IsNullOrEmpty(dongia))
            //{
            //    ViewData["Loi4"] = " Phải nhập đơn giá";
            //}
            //else
            //{
            //    mh.TenMH = tenmh;
            //    mh.Loai = loaihang;
            //    mh.SoLuong = soluong;
            //    mh.Dongia = dongia;
            ViewBag.MaLoai = new SelectList(kho.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            kho.Mathangs.InsertOnSubmit(mh);
                kho.SubmitChanges();
                return RedirectToAction("Index", "Mathang");

            }
        //    return this.ThemMH();
        //}

        public ActionResult Details(int id)
        {
            var sach = from s in kho.Mathangs where s.MaMH == id select s;
            return View(sach.Single());

        }
        public ActionResult Delete(int id)
        {
            Mathang mh = (from s in kho.Mathangs
                      where s.MaMH == id
                      select s).SingleOrDefault(); // truy vấn lấy sách ra sách tại id hiện tại
            if (mh == null) //nếu câu truy vấn trên ko tồn tại, trả về lỗi http
            {
                return HttpNotFound();
            }
            return View(mh);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanxoa(int id)
        {

            try //bắt lỗi khi xóa sách
            {
                kho.Mathangs.DeleteOnSubmit(
                    kho.Mathangs.Where(s => s.MaMH == id).SingleOrDefault()); //thực hiện xóa dữ liệu đang được truy vấn tại Masach=id hiện tại
                kho.SubmitChanges(); //lưu vào database nếu xóa thành công
                return RedirectToAction("Index", "Mathang"); // trả về trang quản lý sách
            }
            catch //bắt lỗi
            {
                //nếu có lỗi khi xóa sách sẽ in ra tempdata
                TempData["LoiXoaSach"] = "Không thể xóa mặt hàng này vì số lượng vẫn còn trong kho";
                return RedirectToAction("Delete"); //nếu lỗi sẽ trả lại về view Xoasach
            }



        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Mathang mh = kho.Mathangs.SingleOrDefault(n => n.MaMH == id);
            ViewBag.MaMH = mh.MaMH;
            
            if (mh == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(mh);
        }

        [HttpPost]

        [ValidateInput(false)]

        public ActionResult Edit(Mathang mh)
        {
            Mathang m  = kho.Mathangs.ToList().Find(n => n.MaMH == mh.MaMH);
            m.MaMH = mh.MaMH;
            m.TenMH = mh.TenMH;
            m.SoLuong = mh.SoLuong;
            m.Dongia = mh.Dongia;
            kho.SubmitChanges();

            return RedirectToAction("Index","Mathang");
        }

    }
}