namespace JewelryAuction.Business.RequestModels.User
{
    public class UpdateUserRequestModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
    }
}
