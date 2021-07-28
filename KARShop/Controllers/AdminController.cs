using KARShop.Models;
using PagedList;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KARShop.Controllers
{
    public class AdminController : Controller
    {
        KARShopContext db = new KARShopContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TaikhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return RedirectToAction("Sanpham", "Admin");
        }
        public ActionResult Sanpham(int? page)
        {
            int pagesize = 7;
            int pagenum = (page ?? 1);
            if (Session["TaikhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.SANPHAM.ToList().OrderByDescending(n => n.MaSP).ToPagedList(pagenum, pagesize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            var tendn = f["username"];
            var matkhau = f["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {

                //Gán giá trị cho đối tượng được tạo mới (kh)
                Admin ad = db.Admin.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // Luu thong tin khach hang da dang nhap;
                    Session["TaikhoanAdmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSP()
        {
            if (Session["TaikhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.MaTL = new SelectList(db.THELOAI.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSP(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaTL = new SelectList(db.THELOAI.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
            if (fileUpload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/IMG/sanpham"), fileName);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    fileUpload.SaveAs(path);
                    sp.Anhbia = fileName;
                }
            }
            db.SANPHAM.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Sanpham");

        }
        public ActionResult ChitietSP(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sach = from s in db.SANPHAM where s.MaSP == id select s;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SANPHAM sp = db.SANPHAM.SingleOrDefault(n => n.MaSP == id);
                return View(sp);
            }
        }
        [HttpPost, ActionName("Xoasp")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["TaikhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SANPHAM sp = db.SANPHAM.SingleOrDefault(n => n.MaSP == id);
                db.SANPHAM.Remove(sp);
                db.SaveChanges();
                return RedirectToAction("Sanpham", "Admin");
            }
        }
        [HttpGet]
        public ActionResult Suasp(int id)
        {
            SANPHAM sp = db.SANPHAM.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaTL = new SelectList(db.THELOAI.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai", sp.MaTL);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasp(SANPHAM sp, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaTL = new SelectList(db.THELOAI.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return null;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/IMG/sanpham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
/*                    SANPHAM result = db.SANPHAM.Find(sp.MaSP);*/
                    sp.Anhbia = fileName;
                    db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Sanpham");
            }

        }
    }
}