﻿namespace KoiFishAuction.Common.ViewModels.KoiFish
{
    public class KoiFishViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public string ImageUrl { get; set; }
        public int Age { get; set; }
        public string Origin { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public string ColorPattern { get; set; }
        public string SellerUserName { get; set; }
    }
}
