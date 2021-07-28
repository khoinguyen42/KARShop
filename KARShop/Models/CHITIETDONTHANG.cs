namespace KARShop.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CHITIETDONTHANG")]
    public partial class CHITIETDONTHANG
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaDonHang { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaSP { get; set; }

        public int? Soluong { get; set; }

        public decimal? Dongia { get; set; }

        public virtual DONDATHANG DONDATHANG { get; set; }

        public virtual SANPHAM SANPHAM { get; set; }
    }
}
