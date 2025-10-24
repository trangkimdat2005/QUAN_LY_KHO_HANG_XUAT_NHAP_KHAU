namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BaoCao")]
    public partial class BaoCao
    {
        [Key]
        [StringLength(10)]
        public string MaBC { get; set; }

        [Required]
        [StringLength(100)]
        public string TenBaoCao { get; set; }

        [Required]
        [StringLength(50)]
        public string LoaiBaoCao { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ThoiGianBatDau { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ThoiGianKetThuc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayLap { get; set; }

        [Required]
        [StringLength(10)]
        public string MaNV { get; set; }

        public decimal? TongGiaTri { get; set; }

        public string NoiDung { get; set; }

        public virtual NhanVien NhanVien { get; set; }
    }
}
