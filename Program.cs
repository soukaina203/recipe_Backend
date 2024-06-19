using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Context;
using Services;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettingsSection = Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<Crypto>();
builder.Services.AddScoped<TokenHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .SetIsOriginAllowed(origin => true)
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

        app.UseStaticFiles();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
