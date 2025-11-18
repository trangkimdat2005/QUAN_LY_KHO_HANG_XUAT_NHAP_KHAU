using QLKHXNK.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace QLKHXNK.Services
{
    public class XNKServices : IXNKServices
    {


        public XNKServices()
        {
        }



        public bool Add<T>(T entity) where T : class
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    _context.Set<T>().Add(entity);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while adding entity of type {typeof(T).Name}: {ex.Message}");
                    return false;
                }
            }
        }

        public bool Update<T>(T entity) where T : class
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    var entry = _context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        _context.Set<T>().Attach(entity);
                    }

                    // Đánh dấu thực thể là đã thay đổi
                    entry.State = EntityState.Modified;

                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while updating entity of type {typeof(T).Name}: {ex.Message}");
                    return false;
                }
            }
        }

        public bool Delete<T>(T entity) where T : class
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    var entry = _context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        _context.Set<T>().Attach(entity);
                    }

                    _context.Set<T>().Remove(entity);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while deleting entity of type {typeof(T).Name}: {ex.Message}");
                    return false;
                }
            } 
        }

        public List<T> GetAll<T>() where T : class
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    var data = _context.Set<T>().ToList(); // Load tất cả vào memory trước

                    var property = typeof(T).GetProperty("isDelete");
                    if (property != null)
                    {
                        return data.Where(t =>
                        {
                            var value = property.GetValue(t);
                            return value is bool isDelete && !isDelete;
                        }).ToList();
                    }

                    return data;
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while retrieving entities of type {typeof(T).Name}: {ex.Message}");
                    return new List<T>();
                }
            }
        }

        public T GetById<T>(string id) where T : class
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    var data = _context.Set<T>().Find(id);
                    

                    return data;
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while retrieving entity of type {typeof(T).Name} with ID {id}: {ex.Message}");
                    return null;
                }
            }
        }

        public bool SoftDelete<T>(T entity) where T : class
        {
            using (var _context = new XNKContextDB())
            {
                try
                {

                    var entry = _context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        _context.Set<T>().Attach(entity);
                    }

                    // Lấy property từ ENTITY, không phải entry
                    var property = typeof(T).GetProperty("isDelete"); // hoặc "isDelete"
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(entity, true); // Set vào ENTITY
                        entry.State = EntityState.Modified; // Đánh dấu là đã thay đổi
                        _context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("No 'IsDelete' property found in entity.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while soft deleting entity of type {typeof(T).Name}: {ex.Message}");
                    return false;
                }
            }
        }

        public ChiTietPhieuNhap GetChiTietPhieuNhapByIds(string soPhieuNhap, string maHangHoa)
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    return _context.ChiTietPhieuNhaps.FirstOrDefault(ctpn => ctpn.MaPN == soPhieuNhap && ctpn.MaHH == maHangHoa);
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while retrieving ChiTietPhieuNhap with SoPhieuNhap {soPhieuNhap} and MaHangHoa {maHangHoa}: {ex.Message}");
                    return null;
                }
            }
        }

        public ChiTietPhieuXuat GetChiTietPhieuXuatByIds(string soPhieuXuat, string maHangHoa)
        {
            using (var _context = new XNKContextDB())
            {
                try
                {
                    return _context.ChiTietPhieuXuats.FirstOrDefault(ctpx => ctpx.MaPX == soPhieuXuat && ctpx.MaHH == maHangHoa);
                }
                catch (Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"An error occurred while retrieving ChiTietPhieuXuat with SoPhieuXuat {soPhieuXuat} and MaHangHoa {maHangHoa}: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
