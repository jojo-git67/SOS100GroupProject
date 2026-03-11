using KommunicationAPI.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace KommunicationAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        //AddDbContext-calls must be before builder.Build since generated files can be readOnly.
        builder.Services.AddDbContext<KommunicationDbContext>(options =>
        {
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        
        var app = builder.Build();

        
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        
        app.UseHttpsRedirection();

        app.MapControllers();
        
        app.Run();

        
    }
}