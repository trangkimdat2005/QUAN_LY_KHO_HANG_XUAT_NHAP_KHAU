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
        List<HangHoa> DSHangHoa();
        List<NhaCungCap> DSNhaCungCap();
        List<NhanVien> DSNhanVien();
        List<KhachHang> DSKhachHang();
        List<Kho> DSKho();
        List<PhieuNhap> DSPhieuNhap();
        List<ChiTietPhieuNhap> DSChiTietPhieuNhap();
        List<PhieuXuat> DSPhieuXuat();
        List<ChiTietPhieuXuat> DSChiTietPhieuXuat();
        List<BaoCao> DSBaoCao();


        //--------------------------------------------------------


        HangHoa GetHangHoa(string MaHH);
        NhaCungCap GetNhaCungCap(string MaNCC);
        NhanVien GetNhanVien(string MaNV);
        KhachHang GetKhachHang(string MaKH);
        Kho GetKho(string MaKho);
        PhieuNhap GetPhieuNhap(string MaPN);
        ChiTietPhieuNhap GetChiTietPhieuNhap(string MaPN, string MaHH);
        PhieuXuat GetPhieuXuat(string MaPX);
        ChiTietPhieuXuat GetChiTietPhieuXuat(string MaPX, string MaHH);
        BaoCao GetBaoCao(string MaBC);
    }
}
