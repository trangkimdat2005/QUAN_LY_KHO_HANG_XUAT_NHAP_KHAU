namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HangHoa")]
    public partial class HangHoa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HangHoa()
        {
            ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
            ChiTietPhieuXuats = new HashSet<ChiTietPhieuXuat>();
        }

        [Key]
        [StringLength(10)]
        public string MaHH { get; set; }

        [Required]
        [StringLength(150)]
        public string TenHH { get; set; }

        [Required]
        [StringLength(20)]
        public string DonViTinh { get; set; }

        [StringLength(50)]
        public string XuatXu { get; set; }

        [StringLength(10)]
        public string MaNCC { get; set; }

        [StringLength(10)]
        public string MaKho { get; set; }

        public int? SoLuongTon { get; set; }

        public decimal? DonGiaNhap { get; set; }

        public decimal? DonGiaBan { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        [StringLength(50)]
        public string TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }

        public virtual Kho Kho { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
