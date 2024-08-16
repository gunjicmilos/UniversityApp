using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniversityApp.Repository;
using UniversityApp.Repository.IRepository;
using UniversityManagament.Data;
using UniversityManagament.Models;
using UniversityManagament.Services;
using UniversityManagament.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString = "Server=127.0.0.1,1433;Database=UniversityDB;User Id=sa;Password=Nemamsifru.99;TrustServerCertificate=True;";
//string connectionString = "Server=localhost; Database=UniversityManagament; Trusted_Connection=True;";

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("AssistantPolicy", policy => policy.RequireRole("Assistant"));
    options.AddPolicy("ProfessorPolicy", policy => policy.RequireRole("Professor"));
});

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IExamPeriodService, ExamPeriodService>();
builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFinanceService, FinanceService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();

builder.Services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
builder.Services.AddScoped<IExamPeriodRepository, ExamPeriodRepository>();
builder.Services.AddScoped<IExamRepository, ExamRepository>();
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();




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