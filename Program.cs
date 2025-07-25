using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Context;
using Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
var appSettingsSection = Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var connectionString = builder.Configuration.GetConnectionString("dockerTest");

builder.Services.AddDbContext<MyContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<Crypto>();
builder.Services.AddScoped<TokenHandlerService>();


var key = Encoding.ASCII.GetBytes(Configuration["AppSettings:Secret"]);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               //    options.Events = new JwtBearerEvents
               //    {
               //        OnTokenValidated = context =>
               //        {
               //                  var route = context.HttpContext.Request.RouteValues;
               //                 //  var userId = int.Parse(context.Principal.Identity.Name);
               //                 //  var user = userService.GetById(userId);
               //                 //  if (user == null)
               //                 //  {
               //                 //      // return unauthorized if user no longer exists
               //                 //      context.Fail("Unauthorized");
               //                 //  }
               //               return Task.CompletedTask;
               //        }
               //    };

               options.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   }
               };
                  /**
                  * this just for the sake of signalR
                  */
                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var accessToken = context.Request.Query["access_token"];
                          if (string.IsNullOrEmpty(accessToken) == false)
                          {
                              context.Token = accessToken;
                          }
                          return Task.CompletedTask;
                      }
                  };

               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           });

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
        app.UseAuthentication();
app.MapControllers();

app.Run();
