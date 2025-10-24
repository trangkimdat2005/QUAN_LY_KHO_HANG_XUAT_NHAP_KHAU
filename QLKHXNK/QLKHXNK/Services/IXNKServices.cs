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
    }
}
