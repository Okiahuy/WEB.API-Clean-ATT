using System.ComponentModel.DataAnnotations;

namespace APPLICATIONCORE.Models
{

    public class FavouriteModel
    {
        [Key]
        public int favouriteID { get; set; }

        public int? accountID { get; set; } // Khóa ngoại để liên kết với Khách hàng
        public AccountModel? Account { get; set; }

        public int? productID { get; set; }
        public ProductModel? Product { get; set; }

        public long? count { get; set; }


    }
}
