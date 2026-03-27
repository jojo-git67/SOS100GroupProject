using BookingAPI.Data;
using BookingAPI.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace BookingAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<BookingDbContext>(options =>
            options.UseSqlite("Data Source=booking.db"));

        builder.Services.AddScoped<IRoomBookingService, RoomBookingService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            dbContext.Database.EnsureCreated();

            if (!dbContext.Rooms.Any())
            {
                dbContext.Rooms.AddRange(
                    new BookingAPI.Models.Room { RoomName = "Grupprum 1", Capacity = 4 },
                    new BookingAPI.Models.Room { RoomName = "Grupprum 2", Capacity = 6 },
                    new BookingAPI.Models.Room { RoomName = "Sal A", Capacity = 30 },
                    new BookingAPI.Models.Room { RoomName = "Sal B", Capacity = 45 },
                    new BookingAPI.Models.Room { RoomName = "Datorsal 1", Capacity = 20 }
                );
                dbContext.SaveChanges();
            }
        }
        app.MapOpenApi();
        app.MapScalarApiReference();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}