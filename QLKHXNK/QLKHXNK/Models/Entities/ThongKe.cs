namespace QLKHXNK.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThongKe")]
    public partial class ThongKe
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string MaKho { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string MaHH { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Thang { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Nam { get; set; }

        public int? SoLuongNhap { get; set; }

        public int? SoLuongXuat { get; set; }

        public int? SoLuongTon { get; set; }

        [StringLength(200)]
        public string GhiChu { get; set; }

        public bool isDelete { get; set; }

        public virtual HangHoa HangHoa { get; set; }

        public virtual Kho Kho { get; set; }
    }
}
