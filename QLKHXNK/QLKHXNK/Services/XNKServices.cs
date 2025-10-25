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
    }
}
