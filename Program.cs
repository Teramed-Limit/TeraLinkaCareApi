using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using TeraLinkaCareApi.Application.Extensions;
using TeraLinkaCareApi.Application.Services;
using TeraLinkaCareApi.Application.Services.Interfaces;
using TeraLinkaCareApi.Core.Domain.Interfaces;
using TeraLinkaCareApi.Core.Repository;
using TeraLinkaCareApi.Infrastructure.Authentication;
using TeraLinkaCareApi.Infrastructure.Mappings;
using TeraLinkaCareApi.Infrastructure.Persistence;
using TeraLinkaCareApi.Infrastructure.Persistence.UnitOfWork;
using TeraLinkaCareApi.Infrastructure.Persistence.UnitOfWork.Interfaces;
using FellowOakDicom;
using FellowOakDicom.Network;
using FellowOakDicom.Network.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 設定 Serilog 作為日誌處理器
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

// 使用 Serilog 作為日誌處理器
builder.Host.UseSerilog();

// 配置 fo-dicom
new DicomSetupBuilder()
    .RegisterServices(s => s.AddFellowOakDicom())
    .Build();

// 獲取應用配置
var configuration = builder.Configuration;

// 配置 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Your API",
            Version = "v1",
            Description = "Your API Description"
        }
    );

    // 添加 JWT 認證配置
    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

// 配置 JWT 認證
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "teramed",
            ValidateAudience = true,
            ValidAudience = "teramed",
            ValidateLifetime = true, // 驗證時間
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromSeconds(3), // 時間偏移量
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(UserService.Secret))
        };
    })
    .AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>("ApiKey", _ => { });

// 設定靜態文件服務
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/build";
});

// 配置 CORS 以允許所有來源的請求
builder.Services.AddCors(options =>
{
    var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
        }
    );
    
    // 新增允許所有來源的政策
    options.AddPolicy(
        "AllowEverything",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

// 配置多個 DbContext
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CRSDbContext>(options =>
{
    options.UseSqlServer(connectionString).EnableSensitiveDataLogging();
    if (Convert.ToBoolean(configuration["SQLDebug"]))
        options.LogTo(Console.WriteLine);
});

// 設定 AutoMapper
builder.Services.AddSingleton(
    provider =>
        new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfiles());
            // cfg.AddProfile(new AutoMapperProfiles(provider.GetService<ConfigService>()));
        }).CreateMapper()
);

// 註冊 UnitOfWork
builder.Services.AddScoped<IUnitOfWork, GenericUnitOfWork<CRSDbContext>>();

// 註冊 Repositories
builder.Services.AddScoped(typeof(IRepository<,>), typeof(GenericRepository<,>));
builder.Services.AddScoped<CodeListRepository>();

// 註冊 Services
builder.Services.AddScoped<ShiftTimeService>();
builder.Services.AddScoped<UnitShiftService>();
builder.Services.AddScoped<PatientEncounterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddMediatR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WoundCareApi V1");
        c.OAuthClientId("swagger-ui");
        c.OAuthAppName("Swagger UI for WoundCareApi");
    });
}

// 配置應用以服務靜態文件和 SPA
app.UseDefaultFiles();
app.UseSpaStaticFiles();

app.UseRouting();

// 添加認證中間件
// app.UseHttpsRedirection();

// 使用 CORS
app.UseCors("AllowAll");

// 添加 Serilog 請求記錄
if (app.Environment.IsDevelopment())
    app.UseSerilogRequestLogging();

// 添加認證中間件
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";
});

app.Run();
