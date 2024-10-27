namespace KoiFishAuction.Common.ViewModels.KoiFish
{
    public class KoiFishDetailViewModel : KoiFishViewModel
    {
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public List<string> Images { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public string SellerUserName { get; set; }
    }
}
