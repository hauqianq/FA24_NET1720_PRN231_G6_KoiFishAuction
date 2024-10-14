using KoiFishAuction.Data.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

builder.Services.AddDbContext<FA24_NET1720_PRN231_G6_KoiFishAuctionContext>(options =>
    options.UseSqlServer(app.Configuration.GetConnectionString("DefaultConnection")));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




