namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietPhieuXuat")]
    public partial class ChiTietPhieuXuat
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string MaPX { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaHH { get; set; }

        public int SoLuong { get; set; }

        public bool isDelete { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual PhieuXuat PhieuXuat { get; set; }
    }
}
