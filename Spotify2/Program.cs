using Spotify2;
using Spotify2.Data;
using Spotify2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Settings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// Para dizer ao Asp.Net que estamos usando Controllers
builder.Services.AddControllers();
// Aqui estamos deixando o DbContext como um serviço.
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Spotify2", Version = "v1" });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<TokenService>();

var app = builder.Build();
// Essa linha faz aquela vez quando a gente usava o mapget, mappost, etc... 
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spotify2 V1");
});
app.UseAuthentication();
app.UseAuthorization();

app.Run();