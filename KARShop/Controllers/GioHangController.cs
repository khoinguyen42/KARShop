using KARShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KARShop.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        KARShopContext db = new KARShopContext();
        // GET: GioHang   
        public List<GioHang> Laygiohang()
        {
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang == null)
            {
                listgiohang = new List<GioHang>();
                Session["GioHang"] = listgiohang;
            }
            return listgiohang;
        }
        public ActionResult ThemGioHang(int iMaSP, string strURL)
        {
            SANPHAM sp = db.SANPHAM.SingleOrDefault(n => n.MaSP == iMaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listgiohang = Laygiohang();
            GioHang gh = listgiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (gh != null)
            {
                if (sp.Soluongton < gh.iSoluong)
                {
                    return View("ThongBao");
                }
                gh.iSoluong++;
                return Redirect(strURL);
            }

            GioHang itmgh = new GioHang(iMaSP);
            if (sp.Soluongton < itmgh.iSoluong)
            {
                return View("ThongBao");
            }
            listgiohang.Add(itmgh);
            return Redirect(strURL);

        }
        private int TinhTongSoLuong()
        {
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang == null)
            {
                return 0;
            }
            return listgiohang.Sum(n => n.iSoluong);
        }
        private double TinhTongTien()
        {
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang == null)
            {
                return 0;
            }
            return listgiohang.Sum(n => n.dThanhtien);
        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        public ActionResult CapNhat(int iMaSP, FormCollection f)
        {
            List<GioHang> listgiohang = Laygiohang();
            GioHang gh = listgiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            SANPHAM sp = db.SANPHAM.SingleOrDefault(n => n.MaSP == iMaSP);

            if (gh != null)
            {
                gh.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            if (sp.Soluongton < gh.iSoluong)
            {
                return View("ThongBao");
            }
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaSp(int iMaSP)
        {
            List<GioHang> listgiohang = Laygiohang();
            GioHang sanpham = listgiohang.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (sanpham != null)
            {
                listgiohang.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("XemGioHang");
            }
            return RedirectToAction("XemGioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            List<GioHang> listgiohang = Laygiohang();
            ViewBag.Tongsoluong = TinhTongSoLuong();
            ViewBag.Tongtien = TinhTongTien();
            return View(listgiohang);
        }
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GioHang> listgiohang = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", f["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            db.DONDATHANG.Add(ddh);
            db.SaveChanges();
            foreach (var item in listgiohang)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSP = item.iMaSP;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                db.CHITIETDONTHANG.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        public ActionResult XemGioHang()
        {
            List<GioHang> listgiohang = Laygiohang();
            ViewBag.TongTien = TinhTongTien();
            return View(listgiohang);
        }
    }
}