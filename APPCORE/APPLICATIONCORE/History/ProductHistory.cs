using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APPLICATIONCORE.History
{
    public class ProductHistory
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public long Quantity { get; set; }
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow; // Thời gian xóa
        public int DeletedByRoleID { get; set; } // RoleID của người xóa sản phẩm
    }
}
