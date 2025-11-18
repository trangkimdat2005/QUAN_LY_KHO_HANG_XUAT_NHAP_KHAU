namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuNhap")]
    public partial class PhieuNhap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuNhap()
        {
            ChiTietPhieuNhaps = new HashSet<ChiTietPhieuNhap>();
        }

        [Key]
        [StringLength(10)]
        public string MaPN { get; set; }

        [Column(TypeName = "date")]
        public DateTime NgayNhap { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNCC { get; set; }

        [Required]
        [StringLength(10)]
        public string MaKho { get; set; }

        public int? TongSoLuong { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        public bool isDelete { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

        public virtual Kho Kho { get; set; }

        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
