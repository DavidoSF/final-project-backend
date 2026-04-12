using System.Text;
using final_project_backend.Data;
using final_project_backend.Models;
using final_project_backend.Repositories.Implementations;
using final_project_backend.Repositories.Interfaces;
using final_project_backend.Services.Implementations;
using final_project_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// ── Database ──────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// ── Authentication (JWT) ──────────────────────────────────────────────────────
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing from configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ── Application services ──────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Controllers / OpenAPI ─────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token in the field below."
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", doc),
            []
        }
    });
});

var app = builder.Build();

// ── Seed data ─────────────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await SeedDataAsync(db);
}

// ── HTTP pipeline ─────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// ── Seed helper ───────────────────────────────────────────────────────────────
static async Task SeedDataAsync(AppDbContext db)
{
    if (db.Users.Any()) return;

    var users = new List<User>
    {
        new()
        {
            Id = Guid.Parse("11111111-0000-0000-0000-000000000001"),
            FullName = "Alice Admin",
            Email = "admin@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = "Admin",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = Guid.Parse("22222222-0000-0000-0000-000000000002"),
            FullName = "Sam Staff",
            Email = "staff@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Staff@123"),
            Role = "Staff",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = Guid.Parse("33333333-0000-0000-0000-000000000003"),
            FullName = "Inactive User",
            Email = "inactive@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Inactive@123"),
            Role = "Staff",
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        }
    };

    var services = new List<Service>
    {
        new()
        {
            Id = Guid.Parse("aaaa0000-0000-0000-0000-000000000001"),
            Name = "Web Development",
            Description = "Full-stack web application development",
            Price = 1500.00m,
            DurationInMinutes = 2880,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = Guid.Parse("aaaa0000-0000-0000-0000-000000000002"),
            Name = "IT Consulting",
            Description = "Technical consulting and architecture review",
            Price = 250.00m,
            DurationInMinutes = 120,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        }
    };

    var clients = new List<Client>
    {
        new()
        {
            Id = Guid.Parse("bbbb0000-0000-0000-0000-000000000001"),
            FullName = "John Doe",
            Email = "john@acme.com",
            PhoneNumber = "+33600000001",
            CompanyName = "Acme Corp",
            CreatedAt = DateTime.UtcNow
        },
        new()
        {
            Id = Guid.Parse("bbbb0000-0000-0000-0000-000000000002"),
            FullName = "Jane Smith",
            Email = "jane@globex.com",
            PhoneNumber = "+33600000002",
            CompanyName = "Globex",
            CreatedAt = DateTime.UtcNow
        }
    };

    db.Users.AddRange(users);
    db.Services.AddRange(services);
    db.Clients.AddRange(clients);
    await db.SaveChangesAsync();
}