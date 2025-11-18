using QLKHXNK.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKHXNK.Services
{
    public interface IXNKServices
    {
        T GetById<T>(String id) where T : class;
        bool Add<T>(T entity) where T : class;
        bool Update<T>(T entity) where T : class;
        bool Delete<T>(T entity) where T : class;
        List<T> GetAll<T>() where T : class;
        bool SoftDelete<T>(T entity) where T : class;





        ChiTietPhieuNhap GetChiTietPhieuNhapByIds(string soPhieuNhap, string maHangHoa);
        ChiTietPhieuXuat GetChiTietPhieuXuatByIds(string soPhieuXuat, string maHangHoa);
    }
}
