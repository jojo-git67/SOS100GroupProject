using BookingAPI.Data;
using BookingAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace BookingAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlite("Data Source=booking.db"));

        builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            dbContext.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}