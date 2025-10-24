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
    }
}
