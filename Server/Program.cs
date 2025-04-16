using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;
using Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt; // necessário para limpar o mapeamento de claims

var builder = WebApplication.CreateBuilder(args);

// Limpar o mapeamento padrão para usar os nomes originais dos claims (ex: "nameid")
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependência dos serviços customizados
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuração do banco de dados
builder.Services.AddDbContext<DbContextServer>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Identity + Entity Framework
builder.Services.AddIdentityCore<MyUser>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DbContextServer>()
.AddDefaultTokenProviders();


// Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"])
            ),
            ValidateIssuer = false,
            ValidateAudience = false,
            NameClaimType = "nameid" // Define o claim que será usado como nome/ID do usuário
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Swagger (UI e geração de docs)
app.UseSwagger();
app.UseSwaggerUI();

// Configurações para desenvolvimento e produção
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Middlewares padrão
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Middleware para retornar erro de API 404 mais claro
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404 &&
        context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{\"error\": \"API endpoint not found.\"}");
    }
});

// Mapear endpoints
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
