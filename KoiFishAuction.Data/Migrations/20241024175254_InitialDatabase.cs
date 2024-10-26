using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoiFishAuction.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KoiFishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ColorPattern = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiFishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoiFishes_Users_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuctionSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KoiFishId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    WinnerId = table.Column<int>(type: "int", nullable: true),
                    CreatorId = table.Column<int>(type: "int", nullable: false),
                    MinIncrement = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionSessions_KoiFishes_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuctionSessions_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuctionSessions_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KoiImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KoiFishId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KoiImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KoiImages_KoiFishes_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionSessionId = table.Column<int>(type: "int", nullable: false),
                    WinnerId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    WinningAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinningDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeliveryStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FeedbackStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    KoiFishId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionHistories_AuctionSessions_AuctionSessionId",
                        column: x => x.AuctionSessionId,
                        principalTable: "AuctionSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuctionHistories_KoiFishes_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuctionHistories_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuctionHistories_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuctionSessionId = table.Column<int>(type: "int", nullable: false),
                    BidderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsWinning = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KoiFishId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_AuctionSessions_AuctionSessionId",
                        column: x => x.AuctionSessionId,
                        principalTable: "AuctionSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bids_KoiFishes_KoiFishId",
                        column: x => x.KoiFishId,
                        principalTable: "KoiFishes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bids_Users_BidderId",
                        column: x => x.BidderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HistoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Confirmation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_AuctionHistories_HistoryId",
                        column: x => x.HistoryId,
                        principalTable: "AuctionHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BidId = table.Column<int>(type: "int", nullable: true),
                    SenderId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Bids_BidId",
                        column: x => x.BidId,
                        principalTable: "Bids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notifications_KoiFishes_ItemId",
                        column: x => x.ItemId,
                        principalTable: "KoiFishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Balance", "Email", "FullName", "JoinDate", "LastLogin", "Password", "PhoneNumber", "Username" },
                values: new object[,]
                {
                    { 1, "844 Reichert Cove, South Carriemouth, Mozambique", 196.77m, "Marielle_Crona@hotmail.com", "Buddy Pacocha", new DateTime(2024, 4, 19, 12, 37, 43, 448, DateTimeKind.Local).AddTicks(9618), new DateTime(2024, 10, 24, 17, 48, 27, 810, DateTimeKind.Local).AddTicks(1848), "1vgZzMnRUE", "1-311-789-2115", "Dallas_Adams1" },
                    { 2, "2366 Kelsie Motorway, Port Chandler, Brazil", 311.14m, "Sammie.Lindgren@yahoo.com", "Amber Hane", new DateTime(2024, 6, 20, 9, 49, 52, 637, DateTimeKind.Local).AddTicks(8137), new DateTime(2024, 10, 24, 1, 19, 50, 688, DateTimeKind.Local).AddTicks(2432), "FmljAxTBr6", "667-709-8372", "Lois_Hessel" },
                    { 3, "4195 Terry Islands, South Vincechester, Palestinian Territory", 274.68m, "Jamie82@gmail.com", "Ignatius Botsford", new DateTime(2024, 8, 8, 4, 59, 49, 143, DateTimeKind.Local).AddTicks(2400), new DateTime(2024, 10, 24, 3, 44, 57, 764, DateTimeKind.Local).AddTicks(3655), "7RTb9Wjxe1", "(297) 399-7780 x590", "Everardo76" },
                    { 4, "586 Smitham Vista, West Demetrisfort, Belize", 747.79m, "Matilda_Schaden81@hotmail.com", "Skylar Schumm", new DateTime(2024, 6, 29, 11, 2, 10, 265, DateTimeKind.Local).AddTicks(2118), new DateTime(2024, 10, 24, 17, 14, 40, 386, DateTimeKind.Local).AddTicks(6884), "AlAM0gQPFe", "806-916-4293 x703", "Iva_Senger42" },
                    { 5, "948 Raymond Stravenue, Lake Ricardofort, United Kingdom", 547.22m, "Grace.Hessel94@yahoo.com", "Milton McDermott", new DateTime(2024, 2, 8, 5, 46, 13, 154, DateTimeKind.Local).AddTicks(2330), new DateTime(2024, 10, 24, 13, 18, 52, 359, DateTimeKind.Local).AddTicks(8201), "tumL1XvYG4", "273-352-9440", "Jaylan53" },
                    { 6, "1871 Judson Lodge, Reichertbury, Republic of Korea", 979.99m, "Kianna77@gmail.com", "Ottilie Dach", new DateTime(2023, 11, 12, 11, 35, 29, 627, DateTimeKind.Local).AddTicks(7064), new DateTime(2024, 10, 24, 12, 16, 14, 894, DateTimeKind.Local).AddTicks(1019), "Pe7RjCu0WJ", "989.298.1559", "Emmanuel73" },
                    { 7, "30487 Hane Village, Lake Darrel, Tonga", 644.96m, "Wyatt.Effertz@yahoo.com", "Lauriane Effertz", new DateTime(2023, 11, 1, 23, 8, 44, 141, DateTimeKind.Local).AddTicks(2442), new DateTime(2024, 10, 24, 21, 58, 47, 447, DateTimeKind.Local).AddTicks(757), "6KtYOen16T", "545-356-5877", "Frederic.Bins34" },
                    { 8, "64385 Tremblay Summit, East Emilioton, Thailand", 369.63m, "Deangelo_Lynch@hotmail.com", "Enrico Bergnaum", new DateTime(2024, 3, 28, 14, 29, 41, 419, DateTimeKind.Local).AddTicks(9283), new DateTime(2024, 10, 24, 19, 55, 16, 869, DateTimeKind.Local).AddTicks(8893), "dqIv8kYFXq", "928-688-4002", "Terrence6" },
                    { 9, "218 John Streets, Ziemefurt, Cook Islands", 914.58m, "Dean.Walsh53@yahoo.com", "Leann Gerlach", new DateTime(2024, 1, 3, 10, 16, 10, 664, DateTimeKind.Local).AddTicks(5196), new DateTime(2024, 10, 24, 15, 7, 52, 440, DateTimeKind.Local).AddTicks(6663), "qJ0KyJMs9b", "526.200.3179", "Isobel_Kautzer" },
                    { 10, "378 Kendrick Branch, Delphineport, Botswana", 634.84m, "Adrianna59@hotmail.com", "Haven Bosco", new DateTime(2024, 8, 18, 10, 54, 25, 424, DateTimeKind.Local).AddTicks(4995), new DateTime(2024, 10, 24, 20, 12, 28, 58, DateTimeKind.Local).AddTicks(3954), "Q2JJd3qHk3", "(580) 880-2227 x8529", "Hans.Wehner43" }
                });

            migrationBuilder.InsertData(
                table: "KoiFishes",
                columns: new[] { "Id", "Age", "ColorPattern", "CurrentPrice", "Description", "Length", "Name", "Origin", "SellerId", "StartingPrice", "Weight" },
                values: new object[,]
                {
                    { 1, 10, "blue", 693.08m, "Velit sed est sint ut et. Soluta illum ut laudantium beatae et. Delectus voluptatem aperiam libero hic odit aperiam consectetur. Doloremque cupiditate molestiae omnis. Qui aut id molestias est excepturi. Nulla voluptas quam id voluptatem ratione ratione dolores eum.", 23.902140241808380m, "Intelligent Soft Soap", "Singapore", 7, 51.65m, 4.115905455367531m },
                    { 2, 6, "indigo", 138.23m, "Enim quo animi facilis. Ut assumenda velit iusto earum consequatur consequatur. Dolorem temporibus ipsam molestiae totam. Autem mollitia non maiores explicabo non.", 76.325434027523470m, "Sleek Plastic Table", "Gambia", 2, 481.80m, 6.742184309051239m },
                    { 3, 3, "orange", 744.92m, "Id molestiae officia aut earum aut vel. Consequuntur quod laboriosam modi. Excepturi et facilis. Qui occaecati soluta itaque quo at.", 69.727458078228610m, "Fantastic Plastic Chicken", "Tokelau", 4, 282.37m, 4.814528396013451m },
                    { 4, 2, "green", 408.73m, "Non aut at. Suscipit omnis deleniti ab. Rerum quibusdam qui. Ut amet est ut temporibus. Corporis et dolorem.", 37.235984640559570m, "Unbranded Cotton Ball", "Vietnam", 10, 661.74m, 5.36831699491036m },
                    { 5, 9, "mint green", 367.31m, "Ut est ullam recusandae sapiente cumque iure rem earum dolorem. Commodi ut perspiciatis inventore. Asperiores et deserunt in et neque vitae. Fugiat eaque quia pariatur voluptate omnis ducimus tempora.", 82.14512687932240m, "Sleek Fresh Bike", "Georgia", 9, 194.48m, 7.630364399406445m },
                    { 6, 2, "turquoise", 397.81m, "Veniam voluptas alias et. Placeat corporis officia. Ipsum sed impedit ipsam aliquid. Provident ipsa ut saepe et.", 71.195135805875170m, "Practical Wooden Chair", "Belarus", 10, 590.38m, 5.484862827609004m },
                    { 7, 6, "maroon", 857.38m, "Reiciendis dolorum doloribus et. Dolorem facilis nihil animi fuga consectetur enim quo aliquam. Iste ut deserunt modi.", 61.187052637244080m, "Tasty Metal Towels", "Saint Kitts and Nevis", 7, 486.39m, 3.933145368395929m },
                    { 8, 7, "maroon", 647.24m, "Itaque natus fuga placeat. Dolores maiores voluptatem laborum et. Voluptatem doloribus cumque aperiam vero ad voluptatibus molestiae maiores. Hic ea hic vel quidem molestiae placeat. Exercitationem aliquid modi et veritatis hic quisquam neque. Maiores fuga ipsum dolorem reiciendis eum.", 12.268226005181020m, "Intelligent Steel Keyboard", "Iraq", 10, 950.10m, 7.901375992435729m },
                    { 9, 1, "pink", 327.56m, "Nostrum animi ducimus id ut maiores autem voluptatem sint. Delectus unde aliquam animi a aspernatur dolor. Veniam dolorum accusantium amet et facere suscipit animi libero. Optio non et quasi.", 93.23108770034350m, "Ergonomic Frozen Hat", "Armenia", 4, 326.29m, 4.509828866746954m },
                    { 10, 6, "grey", 672.11m, "Laboriosam deleniti laborum dolores dolorem. Voluptatem voluptas esse sit numquam fuga maiores sequi. Dolorum minima aut pariatur omnis ipsam. Porro nobis consequatur. Dolor voluptatem atque distinctio. Deserunt porro nesciunt perspiciatis veritatis eum accusamus.", 88.939971637706260m, "Unbranded Granite Gloves", "Panama", 10, 702.45m, 9.204779649509137m }
                });

            migrationBuilder.InsertData(
                table: "AuctionSessions",
                columns: new[] { "Id", "CreatorId", "EndTime", "KoiFishId", "MinIncrement", "Name", "Note", "StartTime", "Status", "WinnerId" },
                values: new object[,]
                {
                    { 1, 4, new DateTime(2025, 5, 31, 21, 1, 42, 445, DateTimeKind.Local).AddTicks(8359), 6, 599.71m, "Tasty Plastic Bike", "Non ipsam eius delectus illum repudiandae non ut.", new DateTime(2025, 8, 7, 15, 8, 59, 914, DateTimeKind.Local).AddTicks(8112), 0, 4 },
                    { 2, 9, new DateTime(2024, 11, 1, 16, 41, 27, 532, DateTimeKind.Local).AddTicks(3827), 3, 656.60m, "Handcrafted Cotton Pants", "Similique dolorem est modi consequatur.", new DateTime(2025, 10, 21, 13, 31, 48, 951, DateTimeKind.Local).AddTicks(38), 0, 9 },
                    { 3, 4, new DateTime(2025, 7, 7, 5, 22, 43, 748, DateTimeKind.Local).AddTicks(1747), 8, 607.71m, "Rustic Steel Pants", "Consectetur voluptatum et modi porro recusandae.", new DateTime(2025, 9, 11, 6, 19, 13, 536, DateTimeKind.Local).AddTicks(4074), 1, 1 },
                    { 4, 10, new DateTime(2025, 9, 17, 6, 29, 2, 753, DateTimeKind.Local).AddTicks(6944), 8, 972.80m, "Tasty Concrete Mouse", "Qui sed sunt totam autem dolorem.", new DateTime(2025, 6, 16, 18, 52, 37, 841, DateTimeKind.Local).AddTicks(4481), 0, 2 },
                    { 5, 8, new DateTime(2025, 10, 23, 18, 12, 51, 607, DateTimeKind.Local).AddTicks(6411), 2, 726.83m, "Rustic Rubber Bacon", "Cumque nemo dolores eligendi dolorum perspiciatis laborum iusto.", new DateTime(2025, 2, 15, 3, 46, 19, 847, DateTimeKind.Local).AddTicks(2319), 0, 7 },
                    { 6, 9, new DateTime(2024, 11, 3, 14, 34, 5, 197, DateTimeKind.Local).AddTicks(4706), 7, 971.95m, "Small Concrete Bike", "Consequatur ad ea voluptatem reprehenderit suscipit.", new DateTime(2025, 6, 20, 1, 17, 14, 124, DateTimeKind.Local).AddTicks(2912), 1, 8 },
                    { 7, 9, new DateTime(2025, 5, 13, 6, 1, 6, 606, DateTimeKind.Local).AddTicks(2410), 4, 279.95m, "Licensed Frozen Bike", "Natus non eum pariatur.", new DateTime(2025, 5, 22, 8, 1, 19, 911, DateTimeKind.Local).AddTicks(8713), 1, 8 },
                    { 8, 1, new DateTime(2025, 7, 21, 21, 38, 37, 76, DateTimeKind.Local).AddTicks(7717), 1, 717.60m, "Incredible Fresh Chair", "Animi eos architecto ea.", new DateTime(2025, 2, 11, 17, 3, 14, 734, DateTimeKind.Local).AddTicks(380), 1, 1 },
                    { 9, 10, new DateTime(2025, 1, 10, 14, 26, 24, 487, DateTimeKind.Local).AddTicks(8150), 4, 508.49m, "Tasty Cotton Car", "Quia id consectetur est eius doloremque sit illo nulla quia.", new DateTime(2024, 12, 25, 11, 2, 2, 43, DateTimeKind.Local).AddTicks(522), 0, 6 },
                    { 10, 7, new DateTime(2025, 8, 30, 2, 23, 48, 147, DateTimeKind.Local).AddTicks(8124), 3, 350.49m, "Handcrafted Steel Fish", "Ut et voluptate.", new DateTime(2025, 6, 20, 6, 44, 7, 207, DateTimeKind.Local).AddTicks(4603), 1, 10 }
                });

            migrationBuilder.InsertData(
                table: "KoiImages",
                columns: new[] { "Id", "ImageUrl", "KoiFishId" },
                values: new object[,]
                {
                    { 1, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F8f2643c8-509c-472e-9f6b-a21a8c834afd_z5964522313997_4d1564fb784c83f3328193edbb3e9ba8.jpg?alt=media&token=4da21ba5-f085-4dfe-9c46-3829d703d6c6", 8 },
                    { 2, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F7123ea67-b7d4-4bb7-8e3c-88ba3a137ec9_z5964522287255_817108d9e4a4bb1021d0067b43e12311.jpg?alt=media&token=12586d8e-383b-456f-b0d7-4b9c4e795f5e", 9 },
                    { 3, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F386a2dc2-8659-4cbe-bdc2-035922868e21_z5964522300982_d636340f7c9af809bb3498fb033ee4ab.jpg?alt=media&token=1cf786b3-14bf-42f8-87cb-13a25c11a839", 4 },
                    { 4, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F386a2dc2-8659-4cbe-bdc2-035922868e21_z5964522300982_d636340f7c9af809bb3498fb033ee4ab.jpg?alt=media&token=1cf786b3-14bf-42f8-87cb-13a25c11a839", 7 },
                    { 5, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fca1e94e1-4e5d-4440-9f41-bc1b72d64008_z5964522287272_fa2418df806688a9845d8f61aaaf8356.jpg?alt=media&token=4cd799f8-53ad-40d7-b0c2-401f2504e3b6", 1 },
                    { 6, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fcbcd27d7-f9df-41fa-b636-c3d9712318a6_z5964522300950_c06ded23e1a5ac463f3777f638e669a2.jpg?alt=media&token=b9c00fde-27cb-4182-90b0-a9a8f58d2524", 1 },
                    { 7, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F86032796-6f68-48a0-945d-906f5e51953d_z5964522323938_45ac9c0bf64f44c5f94b641d706c2f22.jpg?alt=media&token=06ba130f-d25e-4379-a2ef-6b321c5ed3d6", 6 },
                    { 8, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2F98ed17cc-7ae4-46f1-9fe4-d21eaa008532_z5964522323936_8241fd1fce084b7a8ed279caf3b02969.jpg?alt=media&token=4f79a562-f68d-4356-8de5-74c5e5e72b62", 6 },
                    { 9, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fdcbb2824-bd17-4c79-9ade-0f27734ca74a_z5964522334297_db4648522151c8cd83ac3a6f2588da6b.jpg?alt=media&token=3fd25c8d-ed0a-4acc-a761-266b08dba6d9", 1 },
                    { 10, "https://firebasestorage.googleapis.com/v0/b/fir-project-31c70.appspot.com/o/KoiFishes%2Fdcbb2824-bd17-4c79-9ade-0f27734ca74a_z5964522334297_db4648522151c8cd83ac3a6f2588da6b.jpg?alt=media&token=3fd25c8d-ed0a-4acc-a761-266b08dba6d9", 2 }
                });

            migrationBuilder.InsertData(
                table: "Bids",
                columns: new[] { "Id", "Amount", "AuctionSessionId", "BidderId", "Currency", "IsWinning", "KoiFishId", "Location", "Note", "Timestamp" },
                values: new object[,]
                {
                    { 1, 255.20m, 8, 6, "ISK", true, null, "Howechester", "Tempore quia qui dicta exercitationem nihil qui quia cupiditate.", new DateTime(2024, 10, 24, 1, 36, 54, 593, DateTimeKind.Local).AddTicks(2540) },
                    { 2, 366.06m, 9, 2, "SCR", true, null, "East Juliaberg", "Sed molestiae quod.", new DateTime(2024, 10, 24, 2, 45, 50, 810, DateTimeKind.Local).AddTicks(3929) },
                    { 3, 908.55m, 9, 1, "SRD", false, null, "Wintheiserview", "Deserunt officia natus ea eum temporibus.", new DateTime(2024, 10, 24, 21, 44, 6, 444, DateTimeKind.Local).AddTicks(5059) },
                    { 4, 798.89m, 5, 3, "XTS", true, null, "Lake Bo", "Temporibus culpa perspiciatis dolor aut iusto debitis.", new DateTime(2024, 10, 24, 20, 46, 13, 708, DateTimeKind.Local).AddTicks(9296) },
                    { 5, 543.09m, 7, 8, "XBC", true, null, "Andersontown", "Quia fuga minus eaque commodi omnis quo.", new DateTime(2024, 10, 24, 8, 33, 4, 695, DateTimeKind.Local).AddTicks(1766) },
                    { 6, 170.51m, 7, 6, "XBC", false, null, "West Amiechester", "Consequatur eum placeat a officiis similique quia.", new DateTime(2024, 10, 24, 10, 51, 39, 552, DateTimeKind.Local).AddTicks(4265) },
                    { 7, 54.56m, 1, 7, "XAF", false, null, "Trompchester", "Similique alias voluptate.", new DateTime(2024, 10, 24, 11, 8, 37, 549, DateTimeKind.Local).AddTicks(3855) },
                    { 8, 392.88m, 1, 1, "SEK", true, null, "New Ulises", "Et fuga ea accusamus eos voluptas delectus dignissimos est aut.", new DateTime(2024, 10, 24, 17, 3, 59, 29, DateTimeKind.Local).AddTicks(1446) },
                    { 9, 484.60m, 3, 5, "PYG", true, null, "Jacintheview", "Eum et numquam voluptas libero dolores eius porro sunt sequi.", new DateTime(2024, 10, 24, 20, 59, 4, 211, DateTimeKind.Local).AddTicks(719) },
                    { 10, 209.05m, 5, 5, "MUR", true, null, "Aricshire", "Dolorem tempora est.", new DateTime(2024, 10, 24, 7, 7, 47, 690, DateTimeKind.Local).AddTicks(1872) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistories_AuctionSessionId",
                table: "AuctionHistories",
                column: "AuctionSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistories_KoiFishId",
                table: "AuctionHistories",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistories_OwnerId",
                table: "AuctionHistories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionHistories_WinnerId",
                table: "AuctionHistories",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionSessions_CreatorId",
                table: "AuctionSessions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionSessions_KoiFishId",
                table: "AuctionSessions",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionSessions_WinnerId",
                table: "AuctionSessions",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionSessionId",
                table: "Bids",
                column: "AuctionSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BidderId",
                table: "Bids",
                column: "BidderId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_KoiFishId",
                table: "Bids",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiFishes_SellerId",
                table: "KoiFishes",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_KoiImages_KoiFishId",
                table: "KoiImages",
                column: "KoiFishId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BidId",
                table: "Notifications",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ItemId",
                table: "Notifications",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_HistoryId",
                table: "Payments",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KoiImages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "AuctionHistories");

            migrationBuilder.DropTable(
                name: "AuctionSessions");

            migrationBuilder.DropTable(
                name: "KoiFishes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
