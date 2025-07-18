using Microsoft.OpenApi.Models;
using ECommerce.API.Data;
using ECommerce.API.Repository.Abstract;
using ECommerce.API.Repository.Concrete;
using ECommerce.API.Services.Abstract;
using ECommerce.API.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommerce.API.Utilities;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
});

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

// Services
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();


// JWT Service
builder.Services.AddScoped<JwtService>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// API & Swagger
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen 'Bearer {token}' şeklinde girin.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new string[] {}
        }
    });
});



// App pipeline
var app = builder.Build();

// CORS burada kullanlmal
app.UseCors("ReactPolicy");

// Image klasörünü statik olarak sun
app.UseStaticFiles(); // wwwroot için
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Image")),
    RequestPath = "/Image"
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



//veritabanına admin eklemek için yapılır sonradan sil ya da yorumsatırı haline getir.
/* using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerce.API.Data.MyDbContext>();

    // Boş veya eksik user'ları sil
    var emptyUsers = context.Users.Where(u => string.IsNullOrEmpty(u.Email) || string.IsNullOrEmpty(u.FirstName));
    if (emptyUsers.Any())
    {
        context.Users.RemoveRange(emptyUsers);
        context.SaveChanges();
    }

    if (!context.Users.Any(u => u.Email == "admin@mail.com"))
    {
        var adminUser = new ECommerce.API.Entities.Concrete.User
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            Role = ECommerce.API.Entities.Concrete.UserRole.Admin,
            EmailConfirmed = true,
            Phone = "0000000000", // NOT NULL alanlar için örnek değer
            BirthDate = DateTime.Now // NOT NULL alanlar için örnek değer
        };
        context.Users.Add(adminUser);
        context.SaveChanges();

        var admin = new ECommerce.API.Entities.Concrete.Admin
        {
            UserId = adminUser.Id
        };
        context.Admins.Add(admin);
        context.SaveChanges();
    }
} */

// Tüm kullanıcı şifrelerini hash'le (sadece hash'li olmayanları)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerce.API.Data.MyDbContext>();
    var users = context.Users.ToList();
    int updated = 0;
    foreach (var user in users)
    {
        if (!string.IsNullOrEmpty(user.PasswordHash) && !user.PasswordHash.StartsWith("$2"))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            updated++;
        }
    }
    if (updated > 0)
        context.SaveChanges();
    Console.WriteLine($"Hashlenen kullanıcı sayısı: {updated}");
}

app.Run();
