using Bogus;
using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace KoiFishAuction.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void SeedingData(this ModelBuilder modelBuilder)
        {
            var imageUrls = new List<string>
            {
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F7123ea67-b7d4-4bb7-8e3c-88ba3a137ec9_z5964522287255_817108d9e4a4bb1021d0067b43e12311.jpg?alt=media&token=12586d8e-383b-456f-b0d7-4b9c4e795f5e",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fca1e94e1-4e5d-4440-9f41-bc1b72d64008_z5964522287272_fa2418df806688a9845d8f61aaaf8356.jpg?alt=media&token=4cd799f8-53ad-40d7-b0c2-401f2504e3b6",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F10ddf115-af35-4135-8c78-7332326a6ec7_z5964522287273_ce54dd4fc05b92aeb7d60ba81b636951.jpg?alt=media&token=878e3de2-03f2-46ee-be84-ecdd08d332bf",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fcbcd27d7-f9df-41fa-b636-c3d9712318a6_z5964522300950_c06ded23e1a5ac463f3777f638e669a2.jpg?alt=media&token=b9c00fde-27cb-4182-90b0-a9a8f58d2524",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fbc9b8f8b-e091-4109-af6b-ed50cd33a5c5_z5964522300966_84ebeca4ddd53bf4897f68c38ce04f52.jpg?alt=media&token=a469e481-3e95-4f05-b159-fd406a7117d9",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F5efea568-b46c-4f16-b1e5-ddd970fa3193_z5964522300967_ee21967b28954471b234c1e730e7d3e2.jpg?alt=media&token=69bf1880-49b1-41d9-b6a0-b4ad78ab3d70",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F64e8d57b-eb6d-4901-8407-e652df2214a1_z5964522300968_b18b9a6484d58723af65f220f1e26645.jpg?alt=media&token=32b929df-6f23-455c-8fd9-2369a5e30b0e",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F015f3efe-65c1-454b-a4ff-c70e5c0c8576_z5964522300981_c3ff9804d8c01562b0ca2875d7bbe14c.jpg?alt=media&token=9cabe8ce-0ed7-4570-9f07-0bc2f7949e52",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F386a2dc2-8659-4cbe-bdc2-035922868e21_z5964522300982_d636340f7c9af809bb3498fb033ee4ab.jpg?alt=media&token=1cf786b3-14bf-42f8-87cb-13a25c11a839",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F8f2643c8-509c-472e-9f6b-a21a8c834afd_z5964522313997_4d1564fb784c83f3328193edbb3e9ba8.jpg?alt=media&token=4da21ba5-f085-4dfe-9c46-3829d703d6c6",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Ff334c81b-6b27-4716-9740-de830e59d184_z5964522314009_036670decd2b32a41206f59e5ff80328.jpg?alt=media&token=0420c340-e738-493d-a11a-74b8791e976a",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F98ed17cc-7ae4-46f1-9fe4-d21eaa008532_z5964522323936_8241fd1fce084b7a8ed279caf3b02969.jpg?alt=media&token=4f79a562-f68d-4356-8de5-74c5e5e72b62",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Ffc31be31-ae53-4468-9e6d-0681e01b5d52_z5964522323937_a70043c1ed13df5be4a2d4d8a18dfe57.jpg?alt=media&token=f70ef703-914b-4eaa-aedd-c5a90560f704",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F86032796-6f68-48a0-945d-906f5e51953d_z5964522323938_45ac9c0bf64f44c5f94b641d706c2f22.jpg?alt=media&token=06ba130f-d25e-4379-a2ef-6b321c5ed3d6",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F833fc8c9-5c21-4cb3-8d6b-40c1c77e96c2_z5964522334284_c62b1ee3657867906195e28f96996553.jpg?alt=media&token=a8803f22-89a9-4e7f-9838-5d64127b6705",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F40155cab-952e-4196-95ec-45151c9bf8b9_z5964522334285_270905a0053d0a68e08dc73cf06021f8.jpg?alt=media&token=883027dc-bef5-466e-b05f-9bf8747aabff",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fdcbb2824-bd17-4c79-9ade-0f27734ca74a_z5964522334297_db4648522151c8cd83ac3a6f2588da6b.jpg?alt=media&token=3fd25c8d-ed0a-4acc-a761-266b08dba6d9",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F8ac1d09b-7c20-436e-becd-badb67858560_z5964522334298_10d0c3da79c2a41b3b18292c698812b0.jpg?alt=media&token=869661ed-6113-4782-a7c3-47e8c1ef68f0",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F33ba52ef-b831-462c-b90a-5ec1ff61c8a2_z5964522334317_528a6526805a6d47997d8b6add96a4d9.jpg?alt=media&token=b261d87f-d1a2-4bad-b0f1-977c54e50719",
                  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F78081dc8-af59-4a8e-b732-dbe0c68fd04e_z5964522334318_ebc7bc0fed4e9b0800c1a83b98722969.jpg?alt=media&token=3bec3dfd-9bfe-4d83-adf5-cd0afd2970aa"

            };

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Balance, f => f.Finance.Amount())
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.JoinDate, f => f.Date.Past())
                .RuleFor(u => u.LastLogin, f => f.Date.Recent());

            var koiFishFaker = new Faker<KoiFish>()
                .RuleFor(k => k.Id, f => f.IndexFaker + 1)
                .RuleFor(k => k.Name, f => f.Commerce.ProductName())
                .RuleFor(k => k.Description, f => f.Lorem.Paragraph())
                .RuleFor(k => k.StartingPrice, f => f.Finance.Amount())
                .RuleFor(k => k.CurrentPrice, f => f.Finance.Amount())
                .RuleFor(k => k.Age, f => f.Random.Int(1, 10))
                .RuleFor(k => k.Origin, f => f.Address.Country())
                .RuleFor(k => k.Weight, f => f.Random.Decimal(1, 10))
                .RuleFor(k => k.Length, f => f.Random.Decimal(10, 100))
                .RuleFor(k => k.ColorPattern, f => f.Commerce.Color())
                .RuleFor(k => k.SellerId, f => f.Random.Int(1, 10));

            var auctionSessionFaker = new Faker<AuctionSession>()
                .RuleFor(a => a.Id, f => f.IndexFaker + 1)
                .RuleFor(a => a.Name, f => f.Commerce.ProductName())
                .RuleFor(a => a.Note, f => f.Lorem.Sentence())
                .RuleFor(a => a.KoiFishId, f => f.Random.Int(1, 10))
                .RuleFor(a => a.StartTime, f => f.Date.Future())
                .RuleFor(a => a.EndTime, f => f.Date.Future())
                .RuleFor(a => a.Status, f => f.Random.Int(0, 1))
                .RuleFor(a => a.WinnerId, f => f.Random.Int(1, 10))
                .RuleFor(a => a.CreatorId, f => f.Random.Int(1, 10))
                .RuleFor(a => a.MinIncrement, f => f.Finance.Amount());

            var bidFaker = new Faker<Bid>()
                .RuleFor(b => b.Id, f => f.IndexFaker + 1)
                .RuleFor(b => b.AuctionSessionId, f => f.Random.Int(1, 10))
                .RuleFor(b => b.BidderId, f => f.Random.Int(1, 10))
                .RuleFor(b => b.Amount, f => f.Finance.Amount())
                .RuleFor(b => b.Note, f => f.Lorem.Sentence())
                .RuleFor(b => b.IsWinning, f => f.Random.Bool())
                .RuleFor(b => b.Currency, f => f.Finance.Currency().Code)
                .RuleFor(b => b.Timestamp, f => f.Date.Recent())
                .RuleFor(b => b.Location, f => f.Address.City());

            var koiImageFaker = new Faker<KoiImage>()
                .RuleFor(k => k.Id, f => f.IndexFaker + 1)
                .RuleFor(k => k.KoiFishId, f => f.Random.Int(1, 10))
                .RuleFor(k => k.ImageUrl, f => f.PickRandom(imageUrls));

            modelBuilder.Entity<User>().HasData(userFaker.Generate(10));
            modelBuilder.Entity<KoiFish>().HasData(koiFishFaker.Generate(10));
            modelBuilder.Entity<AuctionSession>().HasData(auctionSessionFaker.Generate(10));
            modelBuilder.Entity<Bid>().HasData(bidFaker.Generate(10));
            modelBuilder.Entity<KoiImage>().HasData(koiImageFaker.Generate(10));
        }
    }
}


/*
    [
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F7123ea67-b7d4-4bb7-8e3c-88ba3a137ec9_z5964522287255_817108d9e4a4bb1021d0067b43e12311.jpg?alt=media&token=12586d8e-383b-456f-b0d7-4b9c4e795f5e",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fca1e94e1-4e5d-4440-9f41-bc1b72d64008_z5964522287272_fa2418df806688a9845d8f61aaaf8356.jpg?alt=media&token=4cd799f8-53ad-40d7-b0c2-401f2504e3b6",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F10ddf115-af35-4135-8c78-7332326a6ec7_z5964522287273_ce54dd4fc05b92aeb7d60ba81b636951.jpg?alt=media&token=878e3de2-03f2-46ee-be84-ecdd08d332bf",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fcbcd27d7-f9df-41fa-b636-c3d9712318a6_z5964522300950_c06ded23e1a5ac463f3777f638e669a2.jpg?alt=media&token=b9c00fde-27cb-4182-90b0-a9a8f58d2524",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fbc9b8f8b-e091-4109-af6b-ed50cd33a5c5_z5964522300966_84ebeca4ddd53bf4897f68c38ce04f52.jpg?alt=media&token=a469e481-3e95-4f05-b159-fd406a7117d9",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F5efea568-b46c-4f16-b1e5-ddd970fa3193_z5964522300967_ee21967b28954471b234c1e730e7d3e2.jpg?alt=media&token=69bf1880-49b1-41d9-b6a0-b4ad78ab3d70",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F64e8d57b-eb6d-4901-8407-e652df2214a1_z5964522300968_b18b9a6484d58723af65f220f1e26645.jpg?alt=media&token=32b929df-6f23-455c-8fd9-2369a5e30b0e",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F015f3efe-65c1-454b-a4ff-c70e5c0c8576_z5964522300981_c3ff9804d8c01562b0ca2875d7bbe14c.jpg?alt=media&token=9cabe8ce-0ed7-4570-9f07-0bc2f7949e52",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F386a2dc2-8659-4cbe-bdc2-035922868e21_z5964522300982_d636340f7c9af809bb3498fb033ee4ab.jpg?alt=media&token=1cf786b3-14bf-42f8-87cb-13a25c11a839",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F8f2643c8-509c-472e-9f6b-a21a8c834afd_z5964522313997_4d1564fb784c83f3328193edbb3e9ba8.jpg?alt=media&token=4da21ba5-f085-4dfe-9c46-3829d703d6c6",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Ff334c81b-6b27-4716-9740-de830e59d184_z5964522314009_036670decd2b32a41206f59e5ff80328.jpg?alt=media&token=0420c340-e738-493d-a11a-74b8791e976a",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F98ed17cc-7ae4-46f1-9fe4-d21eaa008532_z5964522323936_8241fd1fce084b7a8ed279caf3b02969.jpg?alt=media&token=4f79a562-f68d-4356-8de5-74c5e5e72b62",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Ffc31be31-ae53-4468-9e6d-0681e01b5d52_z5964522323937_a70043c1ed13df5be4a2d4d8a18dfe57.jpg?alt=media&token=f70ef703-914b-4eaa-aedd-c5a90560f704",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F86032796-6f68-48a0-945d-906f5e51953d_z5964522323938_45ac9c0bf64f44c5f94b641d706c2f22.jpg?alt=media&token=06ba130f-d25e-4379-a2ef-6b321c5ed3d6",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F833fc8c9-5c21-4cb3-8d6b-40c1c77e96c2_z5964522334284_c62b1ee3657867906195e28f96996553.jpg?alt=media&token=a8803f22-89a9-4e7f-9838-5d64127b6705",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F40155cab-952e-4196-95ec-45151c9bf8b9_z5964522334285_270905a0053d0a68e08dc73cf06021f8.jpg?alt=media&token=883027dc-bef5-466e-b05f-9bf8747aabff",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fdcbb2824-bd17-4c79-9ade-0f27734ca74a_z5964522334297_db4648522151c8cd83ac3a6f2588da6b.jpg?alt=media&token=3fd25c8d-ed0a-4acc-a761-266b08dba6d9",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F8ac1d09b-7c20-436e-becd-badb67858560_z5964522334298_10d0c3da79c2a41b3b18292c698812b0.jpg?alt=media&token=869661ed-6113-4782-a7c3-47e8c1ef68f0",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F33ba52ef-b831-462c-b90a-5ec1ff61c8a2_z5964522334317_528a6526805a6d47997d8b6add96a4d9.jpg?alt=media&token=b261d87f-d1a2-4bad-b0f1-977c54e50719",
  "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F78081dc8-af59-4a8e-b732-dbe0c68fd04e_z5964522334318_ebc7bc0fed4e9b0800c1a83b98722969.jpg?alt=media&token=3bec3dfd-9bfe-4d83-adf5-cd0afd2970aa"
]
 */