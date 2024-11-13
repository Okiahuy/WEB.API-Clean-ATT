using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
//quyền
namespace APPLICATIONCORE.Models
{
    public class RoleModel
    {
        [Key]
        public int roleID { get; set; }

        [Required(ErrorMessage = "Tên quyền Không được để trống")]
        public string? RoleName { get; set; }
        [JsonIgnore]
        public ICollection<AccountModel>? Accounts { get; set; }

    }
}
