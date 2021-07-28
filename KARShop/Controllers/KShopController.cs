using KARShop.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace KARShop.Controllers
{
    public class KShopController : Controller
    {
        KARShopContext context = new KARShopContext();
        private List<SANPHAM> layspmoi(int count)
        {
            return context.SANPHAM.OrderBy(n => n.Ngaycapnhat).Take(count).ToList();
        }
        // GET: KShop
        public ActionResult Home(int? page)
        {
            int pagesize = 12;
            int pageNum = (page ?? 1);
            var spmoi = layspmoi(48);
            return View(spmoi.ToPagedList(pageNum, pagesize));
        }
        public ActionResult Category(int? page)
        {
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var spmoi = layspmoi(32);
            return PartialView(spmoi.ToPagedList(pageNum, pagesize));
        }
        public ActionResult Theloai()
        {
            KARShopContext context = new KARShopContext();
            var theloai = from tl in context.THELOAI select tl;
            return PartialView(theloai);
        }
        public ActionResult ProductCategory(int id)
        {
            KARShopContext context = new KARShopContext();
            ViewBag.MaTL = new SelectList(context.THELOAI.ToList().OrderBy(n => n.TenTheLoai), "MaTL", "TenTheLoai");
            ViewBag.TenTL = context.THELOAI.OrderBy(n => n.TenTheLoai);
            var clth = from cl in context.SANPHAM where cl.MaTL == id select cl;
            return PartialView(clth);
        }
        public ActionResult ChitietSP(int id)
        {
            KARShopContext context = new KARShopContext();
            var clth = from cl in context.SANPHAM where cl.MaSP == id select cl;
            return View(clth.Single());
        }
    }
}