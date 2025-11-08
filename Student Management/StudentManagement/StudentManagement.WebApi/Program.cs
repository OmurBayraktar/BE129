using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagement.WebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(p =>
    
        p.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );

});

builder.Services.AddDbContext<StudentManagementDbContext>(option =>
{
    string? connectionString = builder.Configuration.GetConnectionString("Default");

    if(connectionString is null)
    {
        throw new InvalidOperationException("Connection string is not found!");
    }

    option.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StudentManagementDbContext>();

    await dbContext.Database.EnsureDeletedAsync();
    await dbContext.Database.EnsureCreatedAsync();

    await DataSeeder.SeedAsync(dbContext);
}

app.Run();
