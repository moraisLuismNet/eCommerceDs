using eCommerceDs.AutoMappers;
using eCommerceDs.DTOs;
using eCommerceDs.Models;
using eCommerceDs.Repository;
using eCommerceDs.Services;
using eCommerceDs.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<eCommerceDsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
    // Disable tracking
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Configure security
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(builder.Configuration["JWTKey"]))
               });

// Setting up security in Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authentication Using Bearer Scheme. \r\n\r " +
        "Enter the word 'Bearer' followed by a space and the authentication token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });

});

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});

//CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Validators
builder.Services.AddScoped<IValidator<GroupInsertDTO>, GroupInsertValidator>();
builder.Services.AddScoped<IValidator<GroupUpdateDTO>, GroupUpdateValidator>();
builder.Services.AddScoped<IValidator<MusicGenreInsertDTO>, MusicGenreInsertValidator>();
builder.Services.AddScoped<IValidator<MusicGenreUpdateDTO>, MusicGenreUpdateValidator>();
builder.Services.AddScoped<IValidator<RecordInsertDTO>, RecordInsertValidator>();
builder.Services.AddScoped<IValidator<RecordUpdateDTO>, RecordUpdateValidator>();
builder.Services.AddScoped<IValidator<UserInsertDTO>, UserInsertValidator>();
builder.Services.AddScoped<IValidator<UserUpdateDTO>, UserUpdateValidator>();

// Services
builder.Services.AddTransient<IFileManagerService, FileManagerService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IMusicGenreService, MusicGenreService>();
builder.Services.AddScoped<IRecordService, RecordService>();
builder.Services.AddScoped<IeCommerceDsService<UserDTO, UserInsertDTO, UserUpdateDTO>, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<HashService>();

// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repository
//builder.Services.AddScoped<IGroupRepository, GroupRepository>();
//builder.Services.AddScoped<IMusicGenreRepository, MusicGenreRepository>();
//builder.Services.AddScoped<IRecordRepository, RecordRepository>();

//builder.Services.AddScoped<IeCommerceDsRepository<Group>, GroupRepository>();
//builder.Services.AddScoped<IGroupRepository<Group>, GroupRepository>();
//builder.Services.AddScoped<IeCommerceDsRepository<MusicGenre>, MusicGenreRepository>();
//builder.Services.AddScoped<IMusicGenreRepository<MusicGenre>, MusicGenreRepository>();
//builder.Services.AddScoped<IeCommerceDsRepository<Record>, RecordRepository>();
//builder.Services.AddScoped<IRecordRepository<Record>, RecordRepository>();


builder.Services.AddScoped<IGroupRepository<Group>, GroupRepository>();
builder.Services.AddScoped<IMusicGenreRepository<MusicGenre>, MusicGenreRepository>();
builder.Services.AddScoped<IRecordRepository<Record>, RecordRepository>();
builder.Services.AddScoped<IeCommerceDsRepository<User>, UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseStaticFiles(); 

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
