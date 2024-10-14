using System.ComponentModel.DataAnnotations;

namespace JewelryAuction.Business.RequestModels.User
{
    public class RegisterRequestModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
