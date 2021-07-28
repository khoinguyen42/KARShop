using System;
using System.Linq;

namespace KARShop.Models
{
    public class GioHang
    {
        public int iMaSP { get; set; }
        public string sTenSP { get; set; }
        public string sAnhbia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }
        public GioHang(int MaSP)
        {
            KARShopContext context = new KARShopContext();
            iMaSP = MaSP;
            SANPHAM sp = context.SANPHAM.Single(n => n.MaSP == iMaSP);
            sTenSP = sp.TenSP;
            sAnhbia = sp.Anhbia;
            iSoluong = 1;
            dDongia = double.Parse(sp.Giaban.ToString());
        }
        public GioHang(int MaSP, int sl)
        {
            KARShopContext context = new KARShopContext();
            iMaSP = MaSP;
            SANPHAM sp = context.SANPHAM.Single(n => n.MaSP == iMaSP);
            sTenSP = sp.TenSP;
            sAnhbia = sp.Anhbia;
            dDongia = double.Parse(sp.Giaban.ToString());
            iSoluong = sl;
        }
    }
}