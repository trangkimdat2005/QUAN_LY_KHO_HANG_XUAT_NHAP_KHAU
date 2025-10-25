using QLKHXNK.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKHXNK.Services
{
    public class XNKServices : IXNKServices
    {

        public readonly XNKContextDB _context;

        public XNKServices()
        {
            _context = new XNKContextDB();
        }

        public List<HangHoa> DSHangHoa()
        {
            try
            {
                var hangHoas = _context.HangHoas.ToList();
                return hangHoas;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving HangHoa list: {ex.Message}");
                return new List<HangHoa>();
            }
        }

        public List<NhaCungCap> DSNhaCungCap()
        {
            try
            {
                var nhaCungCaps = _context.NhaCungCaps.ToList();
                return nhaCungCaps;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving NhaCungCap list: {ex.Message}");
                return new List<NhaCungCap>();
            }
        }

        public List<NhanVien> DSNhanVien()
        {
            try
            {
                var nhanViens = _context.NhanViens.ToList();
                return nhanViens;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving NhanVien list: {ex.Message}");
                return new List<NhanVien>();
            }
        }

        public List<KhachHang> DSKhachHang()
        {
            try
            {
                var khachHangs = _context.KhachHangs.ToList();
                return khachHangs;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving KhachHang list: {ex.Message}");
                return new List<KhachHang>();
            }
        }

        public List<Kho> DSKho()
        {
            try
            {
                var khos = _context.Khoes.ToList();
                return khos;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving Kho list: {ex.Message}");
                return new List<Kho>();
            }
        }

        public List<PhieuNhap> DSPhieuNhap()
        {
            try
            {
                var phieuNhaps = _context.PhieuNhaps.ToList();
                return phieuNhaps;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving PhieuNhap list: {ex.Message}");
                return new List<PhieuNhap>();
            }
        }

        public List<ChiTietPhieuNhap> DSChiTietPhieuNhap()
        {
            try
            {
                var chiTietPhieuNhaps = _context.ChiTietPhieuNhaps.ToList();
                return chiTietPhieuNhaps;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuNhap list: {ex.Message}");
                return new List<ChiTietPhieuNhap>();
            }
        }

        public List<PhieuXuat> DSPhieuXuat()
        {
            try
            {
                var phieuXuats = _context.PhieuXuats.ToList();
                return phieuXuats;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving PhieuXuat list: {ex.Message}");
                return new List<PhieuXuat>();
            }
        }

        public List<ChiTietPhieuXuat> DSChiTietPhieuXuat()
        {
            try
            {
                var chiTietPhieuXuats = _context.ChiTietPhieuXuats.ToList();
                return chiTietPhieuXuats;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuXuat list: {ex.Message}");
                return new List<ChiTietPhieuXuat>();
            }
        }

        public List<BaoCao> DSBaoCao()
        {
            try
            {
                var baoCaos = _context.BaoCaos.ToList();
                return baoCaos;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving BaoCao list: {ex.Message}");
                return new List<BaoCao>();
            }
        }

        //--------------------------------------------------------

        public HangHoa GetHangHoa(string MaHH)
        {
            try
            {
                var hangHoa = _context.HangHoas.SingleOrDefault(hh => hh.MaHH == MaHH);
                return hangHoa;
            }
            catch(Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving HangHoa with MaHH={MaHH}: {ex.Message}");
                return null;
            }
        }

        public NhaCungCap GetNhaCungCap(string MaNCC)
        {
            try
            {
                var nhaCungCap = _context.NhaCungCaps.SingleOrDefault(ncc => ncc.MaNCC == MaNCC);
                return nhaCungCap;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving NhaCungCap with MaNCC={MaNCC}: {ex.Message}");
                return null;
            }
        }

        public NhanVien GetNhanVien(string MaNV)
        {
            try
            {
                var nhanVien = _context.NhanViens.SingleOrDefault(nv => nv.MaNV == MaNV);
                return nhanVien;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving NhanVien with MaNV={MaNV}: {ex.Message}");
                return null;
            }
        }

        public KhachHang GetKhachHang(string MaKH)
        {
            try
            {
                var khachHang = _context.KhachHangs.SingleOrDefault(kh => kh.MaKH == MaKH);
                return khachHang;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving KhachHang with MaKH={MaKH}: {ex.Message}");
                return null;
            }
        }

        public Kho GetKho(string MaKho)
        {
            try
            {
                var kho = _context.Khoes.SingleOrDefault(k => k.MaKho == MaKho);
                return kho;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving Kho with MaKho={MaKho}: {ex.Message}");
                return null;
            }
        }

        public PhieuNhap GetPhieuNhap(string MaPN)
        {
            try
            {
                var phieuNhap = _context.PhieuNhaps.SingleOrDefault(pn => pn.MaPN == MaPN);
                return phieuNhap;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving PhieuNhap with MaPN={MaPN}: {ex.Message}");
                return null;
            }
        }

        public ChiTietPhieuNhap GetChiTietPhieuNhap(string MaPN, string MaHH)
        {
            try
            {
                var chiTietPhieuNhap = _context.ChiTietPhieuNhaps.SingleOrDefault(ctpn => ctpn.MaPN == MaPN && ctpn.MaHH == MaHH);
                return chiTietPhieuNhap;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuNhap with MaPN={MaPN} and MaHH={MaHH}: {ex.Message}");
                return null;
            }
        }

        public PhieuXuat GetPhieuXuat(string MaPX)
        {
            try
            {
                var phieuXuat = _context.PhieuXuats.SingleOrDefault(px => px.MaPX == MaPX);
                return phieuXuat;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving PhieuXuat with MaPX={MaPX}: {ex.Message}");
                return null;
            }
        }

        public ChiTietPhieuXuat GetChiTietPhieuXuat(string MaPX, string MaHH)
        {
            try
            {
                var chiTietPhieuXuat = _context.ChiTietPhieuXuats.SingleOrDefault(ctpx => ctpx.MaPX == MaPX && ctpx.MaHH == MaHH);
                return chiTietPhieuXuat;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving ChiTietPhieuXuat with MaPX={MaPX} and MaHH={MaHH}: {ex.Message}");
                return null;
            }
        }

        public BaoCao GetBaoCao(string MaBC)
        {
            try
            {
                var baoCao = _context.BaoCaos.SingleOrDefault(bc => bc.MaBC == MaBC);
                return baoCao;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
                Console.WriteLine($"An error occurred while retrieving BaoCao with MaBC={MaBC}: {ex.Message}");
                return null;
            }
        }
    }
}
