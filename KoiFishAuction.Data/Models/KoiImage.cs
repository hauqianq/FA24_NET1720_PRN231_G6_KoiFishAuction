namespace KoiFishAuction.Data.Models
{
    public class KoiImage
    {
        public int Id { get; set; }
        public int KoiFishId { get; set; }
        public string ImageUrl { get; set; }

        public virtual KoiFish KoiFish { get; set; }
    }
}
