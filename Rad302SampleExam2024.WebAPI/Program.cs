using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rad302SampleExam2024.WebAPI.Models;
using Rad302SampleExam2024.DataModel;
using Tracker.WebAPIClient;
using Microsoft.EntityFrameworkCore;
using System.Text;


public class Program
{
    public static async Task Main(string[] args)
    {
        string localAllowSpecificOrigins = "_localAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        // Tracker
        ActivityAPIClient.Track(
            StudentID: "S00219971",
            StudentName: "Ulas Karamustafaoglu",
            activityName: "Rad302 Mock Exam 2025",
            Task: "Creating Programme Schema"
        );

        // DB Contexts
        builder.Services.AddDbContext<ProgrammeDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("BusinessModelConnection")));

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("IdentityModelConnection")));
        
        // Register Generic Repo Service
        /*builder.Services.AddScoped(typeof(IGenericDataService<>), typeof(GenericDataService<>));*/

        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: localAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("https://localhost:7235")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });

        // Auth
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddCookie().AddJwtBearer(cfg =>
        {
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Tokens:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Tokens:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
            };
        });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Web API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization Header using Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    }, new string[] {}
                }
            });
        });

        var app = builder.Build();

        // Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductWebAPI v1");
        });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(localAllowSpecificOrigins);
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // 🔐 SEED ADMIN + REGISTRAR USERS
        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Admin role/user
            string adminRole = "Admin";
            string adminEmail = "admin@atu.ie";
            string adminPassword = "P@ssword1";

            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Firstname = "Admin",
                    Lastname = "User"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }

            // Registrar role/user
            string registrarRole = "Registrar";
            string registrarEmail = "registrar@atu.ie";
            string registrarPassword = "P@ssword1";

            if (!await roleManager.RoleExistsAsync(registrarRole))
                await roleManager.CreateAsync(new IdentityRole(registrarRole));

            var registrarUser = await userManager.FindByEmailAsync(registrarEmail);
            if (registrarUser == null)
            {
                registrarUser = new ApplicationUser
                {
                    UserName = registrarEmail,
                    Email = registrarEmail,
                    EmailConfirmed = true,
                    Firstname = "Registrar",
                    Lastname = "User"
                };

                var result = await userManager.CreateAsync(registrarUser, registrarPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(registrarUser, registrarRole);
                }
            }
        }

        // ✅ Run app
        app.Run();
    }
}
