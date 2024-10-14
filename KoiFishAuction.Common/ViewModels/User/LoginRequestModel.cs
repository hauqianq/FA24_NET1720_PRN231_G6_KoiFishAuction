using System.ComponentModel.DataAnnotations;

namespace JewelryAuction.Business.RequestModels.User
{
    public class LoginRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
