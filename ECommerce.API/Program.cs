
using ECommerce.API.Extensions;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Extensions ile sadeleştir
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddCustomServices(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerWithJwt()
    .AddCorsPolicy();

var app = builder.Build();

app.UseCors("ReactPolicy");

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

app.UseMiddleware<ECommerce.API.Middleware.ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Şifre hash işlemi (isteğe bağlı)
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

app.Run();