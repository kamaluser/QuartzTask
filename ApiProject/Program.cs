using ApiProject.Middlewares;
using AutoMapper;
using Core.Entities;
using Data;
using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.Dtos.CategoryDtos;
using Service.Exceptions;
using Service.Profiles;
using Service.Services.Implementations;
using Service.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {

        var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0)
        .Select(x => new RestExceptionError(x.Key, x.Value.Errors.First().ErrorMessage)).ToList();

        return new BadRequestObjectResult(new { message = "", errors });
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});


builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapProfile(provider.GetService<IHttpContextAccessor>()));
}).CreateMapper());

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;    
    opt.Password.RequireUppercase = false;
})
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFlowerService, FlowerService>();
builder.Services.AddScoped<IFlowerRepository, FlowerRepository>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();

builder.Services.AddAutoMapper(typeof(MapProfile).Assembly);


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CategoryEditDtoValidator>();

builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
        ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };

});

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.Run();