using CourseCatalogAPI.Models;
using CourseCatalogAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();  // ✅ Ersätter AddOpenApi()
builder.Services.AddSwaggerGen();             // ✅ Ersätter AddOpenApi()

builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlite("Data Source=courses.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();      // ✅ Ersätter MapOpenApi()
    app.UseSwaggerUI();    // ✅ Ersätter MapOpenApi()
}

app.UseHttpsRedirection();

app.Run(); 