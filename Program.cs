using System.Text;
using Microsoft.EntityFrameworkCore;
using jwtToken.Data;
using jwtToken.Models;
using AutoMapper;
using jwtToken.Interfaces;
using jwtToken.Repositories;
using jwtToken.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("conn"))
);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.Configure<jwtConfig>(
    builder.Configuration.GetSection("jwtConfig")
);
builder.Services.AddAuthentication(options => {
    //! the default authentication mechanism we will utilize is the jwt token scheme.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(
    jwt => {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("jwtConfig:Secret").Value);
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            //! for every single token we will recieve, we will check the credentials/key that is used to encrypt this token 
            ValidateIssuerSigningKey = true,
            //! the recieved key must match our key that we used to encrypt that token when its generated
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // for dev
            ValidateAudience = false, // for dev
            RequireExpirationTime = false, // for dev -- needs to be updated when refresh token is added
            ValidateLifetime = true
        };
    } 
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
