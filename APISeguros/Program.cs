using APISeguros.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => builder
            .WithOrigins("http://localhost:4200") // Aquí se especifica el origen permitido
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SeguroConnection"))
);

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var contex = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    contex.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseCors("AllowMyOrigin"); // Aplica la política de CORS aquí

app.UseAuthorization();

app.MapControllers();

app.Run();
