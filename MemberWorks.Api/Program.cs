using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MemberWorks.Api.Common;
using MemberWorks.Application;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Infrastructure;
using MemberWorks.Infrastructure.Authentication;
using MemberWorks.Persistence.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });
builder.Services.AddAuthorization();

const string corsPolicy = "Spa";
builder.Services.AddCors(options => options.AddPolicy(corsPolicy, policy => policy
    .WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:4200"])
    .AllowAnyHeader()
    .AllowAnyMethod()));

builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
        // Serialize/accept enums as their names ("OrgAdmin", "Parent") so the API contract is
        // stable and the Angular client can use string unions instead of magic numbers.
        o.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MemberWorks API", Version = "v1" });

    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = [] });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();

