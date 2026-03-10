using Microsoft.EntityFrameworkCore;
using NoName.Application;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Services;
using NoName.Infrastructure.EF;
using NoName.Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NoNameDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NoNameDB")));


//DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IProductAppService, ProductAppService>();


// Resigter  MediatR 
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationAssemblyMarker).Assembly));
// Resigter AutoMapper
builder.Services.AddAutoMapper(typeof(IApplicationAssemblyMarker).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
