using CarAPI;
using CarAPI.Entities;
using CarAPI.Middleware;
using CarAPI.Models;
using CarAPI.Models.Validators;
using CarAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text.Json.Serialization;
using System.Text;
using CarAPI.Seeder;
using Microsoft.AspNetCore.Authorization;
using CarAPI.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseNLog();
builder.Services.AddControllers();
builder.Services.AddScoped<IAuthorizationHandler, CarOperationRequirementHandler>();
builder.Services.AddDbContext<CarDbContext>();
builder.Services.AddScoped<CarSeeder>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ITechnicalReviewService,TechnicalReviewService>();
builder.Services.AddScoped<IRepairService, RepairService>();
builder.Services.AddScoped<IInsuranceService, InsuranceService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped <IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped <IValidator<Query>, CarQueryValidator>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());  
});
var authSeettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authSeettings);
builder.Services.AddSingleton(authSeettings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authSeettings.JwtIssuer,
        ValidAudience = authSeettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSeettings.JwtKey))
    };
});
var app = builder.Build();
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<CarSeeder>();
seeder.Seed();
// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car API");


    }); 


app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

