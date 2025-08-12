
using HotelBookingApi.Data;
using HotelBookingApi.IRepository;
using HotelBookingApi.MappingConfig;
using HotelBookingApi.Models;
using HotelBookingApi.Repository;
using HotelBookingApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace HotelBookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options => {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Please enter token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IHotelRepository, HotelRepository>();
            builder.Services.AddScoped<EmailSender>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(op => op.AddProfile<MappConfig>());
            builder.Services.AddScoped<ISeasonRepo, SeasonRepo>();
            builder.Services.AddScoped<IHotelReviewsRepo, HotelReviewsRepo>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IImageUrlService, ImageUrlService>();
            builder.Services.AddHostedService<RoomAvailabilityService>();


            builder.Services.AddScoped<IRoomRepo, RoomRepo>();
            builder.Services.AddScoped<IBookingRepo, BookingRepo>();
            builder.Services.AddScoped<IFavoriteRepo, FavoriteRepo>();

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
             ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
             ValidAudience = builder.Configuration["JwtSettings:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
         };
     });
            builder.Services.AddHttpClient<AIAgentService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient",
                    policy =>
                    {
                        //policy.AllowAnyOrigin()
                        //      .AllowAnyHeader()
                        //      .AllowAnyMethod();
                        policy.WithOrigins("http://localhost:4200") // واجهة Angular
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
            Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            var app = builder.Build();
            var env = app.Environment;
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            { 
             app.UseSwagger();
             app.UseSwaggerUI();
             }
            //builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            //StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            //builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
           // StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            //Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
            //app.UseCors("AllowAll");
            // app.UseCors("AllowAngularClient");
            app.UseHttpsRedirection();
            app.UseCors("AllowAngularClient");
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, "images", "rooms")),
                RequestPath = "/uploads"
            });
            app.UseAuthentication();
            app.UseAuthorization();
           


            app.MapControllers();
           
            app.Run();
        }
    }
}