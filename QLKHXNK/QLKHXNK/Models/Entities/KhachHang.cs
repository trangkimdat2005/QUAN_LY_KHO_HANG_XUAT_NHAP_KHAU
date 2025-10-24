namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            PhieuXuats = new HashSet<PhieuXuat>();
        }

        [Key]
        [StringLength(10)]
        public string MaKH { get; set; }

        [Required]
        [StringLength(150)]
        public string TenKH { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [StringLength(15)]
        public string SoDienThoai { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string LoaiKH { get; set; }

        [StringLength(20)]
        public string MaSoThue { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuXuat> PhieuXuats { get; set; }
    }
}
