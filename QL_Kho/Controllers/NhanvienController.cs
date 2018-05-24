using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_Kho.Models;

namespace QL_Kho.Controllers
{
    public class NhanvienController : Controller
    {
        // GET: Nhanvien
        KhohangDataContext kho = new KhohangDataContext();
        public ActionResult Index()
        {
            return View(kho.Nhanviens.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Nhanvien nv)
        {
            kho.Nhanviens.InsertOnSubmit(nv);
            kho.SubmitChanges();
            return RedirectToAction("Index", "Nhanvien");
        }

        public ActionResult Details(int id)
        {
            Nhanvien nv = kho.Nhanviens.SingleOrDefault(n => n.MaNV == id);
            ViewBag.MaNV = nv.MaNV;
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nv);

        }
        public ActionResult Delete(int id)
        {
            Nhanvien nv = (from s in kho.Nhanviens
                      where s.MaNV == id
                      select s).SingleOrDefault(); // truy vấn lấy sách ra sách tại id hiện tại
            if (nv == null) //nếu câu truy vấn trên ko tồn tại, trả về lỗi http
            {
                return HttpNotFound();
            }
            return View(nv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Xacnhanxoa(int id)
        {

            try //bắt lỗi khi xóa sách
            {
                kho.Nhanviens.DeleteOnSubmit(
                    kho.Nhanviens.Where(s => s.MaNV == id).SingleOrDefault()); //thực hiện xóa dữ liệu đang được truy vấn tại Masach=id hiện tại
                kho.SubmitChanges(); //lưu vào database nếu xóa thành công
                return RedirectToAction("Index", "Nhanvien"); // trả về trang quản lý sách
            }
            catch //bắt lỗi
            {
                //nếu có lỗi khi xóa sách sẽ in ra tempdata
                TempData["LoiXoaSach"] = "Không thể xóa nhân viên này vì đang quản lí tại 1 một địa điểm";
                return RedirectToAction("Delete"); //nếu lỗi sẽ trả lại về view Xoasach
            }



        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Nhanvien nv = kho.Nhanviens.SingleOrDefault(n => n.MaNV == id);
            ViewBag.MaNV = nv.MaNV;
            if (nv == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nv);
        }

        [HttpPost]

        [ValidateInput(false)]

        public ActionResult Edit(Nhanvien nv)
        {
            Nhanvien nn = kho.Nhanviens.ToList().Find(n => n.MaNV == nv.MaNV);
            nn.MaNV = nv.MaNV;
            nn.Ho = nv.Ho;
            nn.Ten = nv.Ten;
            nn.Phai = nv.Phai;
            nn.Namsinh = nv.Namsinh;
            nn.Diachi = nv.Diachi;
            nn.SDT = nv.SDT;
            nn.taikhoan = nv.taikhoan;
            nn.matkhau = nv.matkhau;

            kho.SubmitChanges();
            return RedirectToAction("Index", "Nhanvien");
        }


    }
}

