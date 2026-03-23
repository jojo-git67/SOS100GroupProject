using CourseCatalogAPI.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlite("Data Source=courses.db"));

// Lägg till CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowMVC");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();