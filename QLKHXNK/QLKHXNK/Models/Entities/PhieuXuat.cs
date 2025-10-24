namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuXuat")]
    public partial class PhieuXuat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuXuat()
        {
            ChiTietPhieuXuats = new HashSet<ChiTietPhieuXuat>();
        }

        [Key]
        [StringLength(10)]
        public string MaPX { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayXuat { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNV { get; set; }

        [Required]
        [StringLength(10)]
        public string MaKH { get; set; }

        [Required]
        [StringLength(10)]
        public string MaKho { get; set; }

        public int? TongSoLuong { get; set; }

        public decimal? TongGiaTri { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }

        public virtual KhachHang KhachHang { get; set; }

        public virtual Kho Kho { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
