using KARShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace KARShop.Controllers
{
    public class UserController : Controller
    {
        KARShopContext db = new KARShopContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KHACHHANG kh, FormCollection f)
        {
            var nhaplaimatkhau = f["Nhaplaimatkhau"];
            if (string.IsNullOrEmpty(kh.HoTen))
            {
                ViewData["Loi1"] = "Họ tên không được để trống";
            }
            else if (string.IsNullOrEmpty(kh.Taikhoan))
            {
                ViewData["Loi2"] = "Tài khoản không được để trống";
            }
            else if (string.IsNullOrEmpty(kh.Matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (string.IsNullOrEmpty(kh.Email))
            {
                ViewData["Loi5"] = "Email không được để trống";
            }
            else if (string.IsNullOrEmpty(kh.DienthoaiKH))
            {
                ViewData["Loi6"] = "Phải nhập số điện thoại";
            }
            else
            {
                db.KHACHHANG.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection f)
        {
            var tendn = f["TenDN"];
            var matkhau = f["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANG.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}